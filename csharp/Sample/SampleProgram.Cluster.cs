using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;
using Polly;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task WaitForCluster(IClustersApi clusterClient, string clusterId, int pollIntervalSeconds = 15)
    {
        var retryPolicy = Policy.Handle<WebException>()
            .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.BadGateway)
            .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.InternalServerError)
            .Or<ClientApiException>(e => e.Message.Contains("\"error_code\":\"TEMPORARILY_UNAVAILABLE\""))
            .Or<TaskCanceledException>(e => !e.CancellationToken.IsCancellationRequested) // web request timeout
            .OrResult<ClusterInfo>(info => info.State is not (ClusterState.RUNNING or ClusterState.ERROR or ClusterState.TERMINATED))
            .WaitAndRetryForeverAsync(
                _ => TimeSpan.FromSeconds(pollIntervalSeconds),
                (delegateResult, _) =>
                {
                    if (delegateResult.Exception != null)
                    {
                        Console.WriteLine($"[{DateTime.UtcNow:s}] Failed to query cluster info - {delegateResult.Exception}");
                    }
                });

        await retryPolicy.ExecuteAsync(async () =>
        {
            var info = await clusterClient.Get(clusterId);
            Console.WriteLine($"[{DateTime.UtcNow:s}]Cluster:{clusterId}\tState:{info.State}\tMessage:{info.StateMessage}");

            return info;
        });
    }

    private static async Task TestClustersApi(DatabricksClient client)
    {
        Console.WriteLine("Listing node types (take 10)");
        var nodeTypes = await client.Clusters.ListNodeTypes();
        foreach (var nodeType in nodeTypes.Take(10))
        {
            Console.WriteLine($"\t{nodeType.NodeTypeId}\tMemory: {nodeType.MemoryMb} MB\tCores: {nodeType.NumCores}\tAvailable Quota: {nodeType.ClusterCloudProviderNodeInfo.AvailableCoreQuota}");
        }

        Console.WriteLine("Listing Databricks runtime versions");
        var sparkVersions = await client.Clusters.ListSparkVersions();
        foreach (var (key, name) in sparkVersions)
        {
            Console.WriteLine($"\t{key}\t\t{name}");
        }

        Console.WriteLine("Listing existing clusters");
        var clusters = await client.Clusters.List();
        foreach (var cluster in clusters)
        {
            Console.WriteLine($"\t{cluster.ClusterId}\t\t{cluster.ClusterName}");
        }

        Console.WriteLine("Creating standard cluster");

        var clusterConfig = ClusterAttributes
            .GetNewClusterConfiguration("SampleProgram cluster")
            .WithRuntimeVersion(RuntimeVersions.Runtime_10_4)
            .WithAutoTermination(30)
            .WithClusterLogConf("dbfs:/logs/")
            .WithNodeType(NodeTypes.Standard_D3_v2)
            .WithClusterMode(ClusterMode.SingleNode)
            .WithDockerImage("databricksruntime/standard:latest");

        var clusterId = await client.Clusters.Create(clusterConfig);

        var createdCluster = await client.Clusters.Get(clusterId);
        Console.WriteLine(JsonSerializer.Serialize(createdCluster, Options));
        await WaitForCluster(client.Clusters, clusterId);

        Console.WriteLine($"Editing cluster {clusterId}");
        createdCluster.CustomTags = new Dictionary<string, string> { { "TestingTagKey", "TestingTagValue" } };
        await client.Clusters.Edit(clusterId, clusterConfig);
        await WaitForCluster(client.Clusters, clusterId);

        Console.WriteLine("Deleting cluster {0}", clusterId);
        await client.Clusters.Delete(clusterId);

        Console.WriteLine("Creating Photon cluster");
        clusterConfig = ClusterAttributes
            .GetNewClusterConfiguration("SampleProgram cluster")
            .WithRuntimeVersion(RuntimeVersions.Runtime_10_4_PHOTON)
            .WithClusterMode(ClusterMode.Standard)
            .WithNumberOfWorkers(1)
            .WithNodeType(NodeTypes.Standard_E8s_v3);

        clusterId = await client.Clusters.Create(clusterConfig);
        createdCluster = await client.Clusters.Get(clusterId);
        Console.WriteLine(JsonSerializer.Serialize(createdCluster, Options));
        await WaitForCluster(client.Clusters, clusterId);

        Console.WriteLine("Deleting cluster {0}", clusterId);
        await client.Clusters.Delete(clusterId);

        Console.WriteLine("Creating HighConcurrency cluster");

        clusterConfig = ClusterAttributes
            .GetNewClusterConfiguration("SampleProgram cluster")
            .WithRuntimeVersion(RuntimeVersions.Runtime_10_4)
            .WithAutoScale(1, 3)
            .WithAutoTermination(30)
            .WithClusterLogConf("dbfs:/logs/")
            .WithNodeType(NodeTypes.Standard_D3_v2)
            .WithClusterMode(ClusterMode.HighConcurrency)
            .WithTableAccessControl(true);

        clusterId = await client.Clusters.Create(clusterConfig);

        createdCluster = await client.Clusters.Get(clusterId);
        Console.WriteLine(JsonSerializer.Serialize(createdCluster, Options));
        await WaitForCluster(client.Clusters, clusterId);

        Console.WriteLine($"Terminating cluster {clusterId}");
        await client.Clusters.Terminate(clusterId);
        await WaitForCluster(client.Clusters, clusterId, 5);

        Console.WriteLine($"Getting all events from cluster {clusterId}");

        EventsResponse eventsResponse = null;
        var events = new List<ClusterEvent>();
        do
        {
            var nextPage = eventsResponse?.NextPage;
            eventsResponse = await client.Clusters.Events(
                clusterId,
                nextPage?.StartTime,
                nextPage?.EndTime,
                nextPage?.Order,
                nextPage?.EventTypes,
                nextPage?.Offset,
                nextPage?.Limit
            );
            events.AddRange(eventsResponse.Events);

        } while (eventsResponse.HasNextPage);

        Console.WriteLine("{0} events retrieved from cluster {1}.", events.Count, clusterId);
        Console.WriteLine("Top 10 events: ");
        foreach (var e in events.Take(10))
        {
            Console.WriteLine("\t[{0:s}] {1}\t{2}", e.Timestamp, e.Type, e.Details.User);
        }

        Console.WriteLine("Deleting cluster {0}", clusterId);
        await client.Clusters.Delete(clusterId);
    }
}
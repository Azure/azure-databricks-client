using System;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestInstancePoolApi(DatabricksClient client)
    {
        Console.WriteLine("Creating Testing Instance Pool");
        var poolAttributes = new InstancePoolAttributes
        {
            PoolName = "TestInstancePool",
            PreloadedSparkVersions = new[] { RuntimeVersions.Runtime_10_4 },
            MinIdleInstances = 2,
            MaxCapacity = 100,
            IdleInstanceAutoTerminationMinutes = 15,
            NodeTypeId = NodeTypes.Standard_D3_v2,
            EnableElasticDisk = true,
            DiskSpec = new DiskSpec
                { DiskCount = 2, DiskSize = 64, DiskType = DiskType.FromAzureDisk(AzureDiskVolumeType.STANDARD_LRS) },
            PreloadedDockerImages = new[]
            {
                new DockerImage {Url = "databricksruntime/standard:latest"}
            },
            AzureAttributes = new InstancePoolAzureAttributes { Availability = AzureAvailability.SPOT_AZURE, SpotBidMaxPrice = -1 }
        };

        var poolId = await client.InstancePool.Create(poolAttributes).ConfigureAwait(false);

        Console.WriteLine("Listing pools");
        var pools = await client.InstancePool.List().ConfigureAwait(false);
        foreach (var pool in pools)
        {
            Console.WriteLine($"\t{pool.PoolId}\t{pool.PoolName}\t{pool.State}");
        }

        Console.WriteLine("Getting created pool by poolId");
        var targetPoolInfo = await client.InstancePool.Get(poolId).ConfigureAwait(false);

        Console.WriteLine("Editing pool");
        targetPoolInfo.MinIdleInstances = 3;
        await client.InstancePool.Edit(poolId, targetPoolInfo).ConfigureAwait(false);

        Console.WriteLine("Getting edited pool by poolId");
        targetPoolInfo = await client.InstancePool.Get(poolId).ConfigureAwait(false);
        Console.WriteLine($"MinIdleInstances: {targetPoolInfo.MinIdleInstances}");

        Console.WriteLine("Creating a sample cluster in the pool.");
        var clusterConfig = ClusterAttributes.GetNewClusterConfiguration("SampleProgram cluster")
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3)
            .WithAutoScale(3, 7)
            .WithAutoTermination(30)
            .WithClusterLogConf("dbfs:/logs/");
        clusterConfig.InstancePoolId = poolId;

        var clusterId = await client.Clusters.Create(clusterConfig);

        var createdCluster = await client.Clusters.Get(clusterId);

        Console.WriteLine($"Created cluster pool Id: {createdCluster.InstancePoolId}");

        Console.WriteLine("Deleting pool");
        await client.InstancePool.Delete(poolId);

        Console.WriteLine("Deleting cluster");
        await client.Clusters.Delete(clusterId);
    }
}
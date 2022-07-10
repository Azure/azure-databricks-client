using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public class ClustersApiClient : ApiClient, IClustersApi
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClustersApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public ClustersApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<string> Create(ClusterAttributes clusterAttributes, string idempotencyToken = default,
        CancellationToken cancellationToken = default)
    {
        var jsonObj = JsonSerializer.SerializeToNode(clusterAttributes, Options)!.AsObject();
        if (!string.IsNullOrEmpty(idempotencyToken))
        {
            jsonObj.Add("idempotency_token", idempotencyToken);
        }

        var clusterIdentifier = await HttpPost<JsonObject, JsonObject>(this.HttpClient,
                $"{ApiVersion}/clusters/create", jsonObj, cancellationToken)
            .ConfigureAwait(false);
        return clusterIdentifier["cluster_id"]!.GetValue<string>();
    }

    public async Task Start(string clusterId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/start", new {cluster_id = clusterId},
            cancellationToken).ConfigureAwait(false);
    }

    public async Task Edit(string clusterId, ClusterAttributes clusterConfig,
        CancellationToken cancellationToken = default)
    {
        var jsonObj = JsonSerializer.SerializeToNode(clusterConfig, Options)!.AsObject();
        jsonObj.Add("cluster_id", clusterId);
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/edit", jsonObj, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Restart(string clusterId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/restart", new {cluster_id = clusterId},
            cancellationToken).ConfigureAwait(false);
    }

    public async Task Resize(string clusterId, int numWorkers, CancellationToken cancellationToken = default)
    {
        var request = new {cluster_id = clusterId, num_workers = numWorkers};
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/resize", request, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Resize(string clusterId, AutoScale autoScale, CancellationToken cancellationToken = default)
    {
        var request = new {cluster_id = clusterId, autoscale = autoScale};
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/resize", request, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ClusterInfo> Get(string clusterId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/clusters/get?cluster_id={clusterId}";
        return await HttpGet<ClusterInfo>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task Terminate(string clusterId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/delete", new {cluster_id = clusterId},
            cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string clusterId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/permanent-delete", new {cluster_id = clusterId},
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ClusterInfo>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/clusters/list";
        var clusterList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken)
            .ConfigureAwait(false);

        clusterList.TryGetPropertyValue("clusters", out var clustersNode);

        return clustersNode
            .Map(node => node.Deserialize<IEnumerable<ClusterInfo>>(Options))
            .GetOrElse(Enumerable.Empty<ClusterInfo>);
    }

    public async Task Pin(string clusterId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/pin", new {cluster_id = clusterId},
            cancellationToken).ConfigureAwait(false);
    }

    public async Task Unpin(string clusterId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/clusters/unpin", new {cluster_id = clusterId},
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<NodeType>> ListNodeTypes(CancellationToken cancellationToken = default)
    {
        var result =
            await HttpGet<JsonObject>(this.HttpClient, $"{ApiVersion}/clusters/list-node-types", cancellationToken)
                .ConfigureAwait(false);
        return result["node_types"].Deserialize<IEnumerable<NodeType>>(Options);
    }

    public async Task<IDictionary<string, string>> ListSparkVersions(CancellationToken cancellationToken = default)
    {
        var result =
            await HttpGet<JsonObject>(this.HttpClient, $"{ApiVersion}/clusters/spark-versions", cancellationToken)
                .ConfigureAwait(false);

        return result["versions"]!.AsArray().ToDictionary(
            e => e["key"]!.GetValue<string>(),
            e => e["name"]!.GetValue<string>()
        );
    }

    public async Task<EventsResponse> Events(string clusterId, DateTimeOffset? startTime, DateTimeOffset? endTime,
        ListOrder? order,
        IEnumerable<ClusterEventType> eventTypes, long? offset, long? limit,
        CancellationToken cancellationToken = default)
    {
        var request = new EventsRequest
        {
            ClusterId = clusterId,
            EndTime = endTime,
            StartTime = startTime,
            Order = order,
            EventTypes = eventTypes,
            Limit = limit,
            Offset = offset
        };

        var response = await HttpPost<EventsRequest, EventsResponse>(this.HttpClient,
                $"{ApiVersion}/clusters/events", request, cancellationToken)
            .ConfigureAwait(false);

        return response;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClustersApiClient : ApiClient, IClustersApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClustersApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        public ClustersApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<string> Create(ClusterInfo clusterInfo, CancellationToken cancellationToken = default)
        {
            var clusterIdentifier = await HttpPost<ClusterInfo, dynamic>(this.HttpClient, "clusters/create", clusterInfo, cancellationToken)
                .ConfigureAwait(false);
            return clusterIdentifier.cluster_id.ToObject<string>();
        }

        public async Task Start(string clusterId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "clusters/start", new { cluster_id = clusterId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task Edit(string clusterId, ClusterInfo clusterConfig, CancellationToken cancellationToken = default)
        {
            clusterConfig.ClusterId = clusterId;
            await HttpPost(this.HttpClient, "clusters/edit", clusterConfig, cancellationToken).ConfigureAwait(false);
        }

        public async Task Restart(string clusterId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "clusters/restart", new { cluster_id = clusterId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task Resize(string clusterId, int numWorkers, CancellationToken cancellationToken = default)
        {
            var request = new {cluster_id = clusterId, num_workers = numWorkers};
            await HttpPost(this.HttpClient, "clusters/resize", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task Resize(string clusterId, AutoScale autoScale, CancellationToken cancellationToken = default)
        {
            var request = new {cluster_id = clusterId, autoscale = autoScale};
            await HttpPost(this.HttpClient, "clusters/resize", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ClusterInfo> Get(string clusterId, CancellationToken cancellationToken = default)
        {
            var requestUri = $"clusters/get?cluster_id={clusterId}";
            return await HttpGet<ClusterInfo>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }

        public async Task Terminate(string clusterId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "clusters/delete", new { cluster_id = clusterId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(string clusterId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "clusters/permanent-delete", new { cluster_id = clusterId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClusterInfo>> List(CancellationToken cancellationToken = default)
        {
            const string requestUri = "clusters/list";
            var clusterList = await HttpGet<dynamic>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
            return PropertyExists(clusterList, "clusters")
                ? clusterList.clusters.ToObject<IEnumerable<ClusterInfo>>()
                : Enumerable.Empty<ClusterInfo>();
        }

        public async Task Pin(string clusterId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "clusters/pin", new { cluster_id = clusterId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task Unpin(string clusterId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "clusters/unpin", new { cluster_id = clusterId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NodeType>> ListNodeTypes(CancellationToken cancellationToken = default)
        {
            var result = await HttpGet<dynamic>(this.HttpClient, "clusters/list-node-types", cancellationToken).ConfigureAwait(false);
            return result.node_types.ToObject<IEnumerable<NodeType>>();
        }

        public async Task<IDictionary<string, string>> ListSparkVersions(CancellationToken cancellationToken = default)
        {
            var result = await HttpGet<dynamic>(this.HttpClient, "clusters/spark-versions", cancellationToken).ConfigureAwait(false);
            return ((IEnumerable<dynamic>) result.versions).ToDictionary(
                sv => (string) sv.key.ToObject<string>(),
                sv => (string) sv.name.ToObject<string>()
            );
        }

        public async Task<EventsResponse> Events(string clusterId, DateTimeOffset? startTime, DateTimeOffset? endTime, ListOrder? order,
            IEnumerable<ClusterEventType> eventTypes, long? offset, long? limit, CancellationToken cancellationToken = default)
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

            var response = await HttpPost<EventsRequest, EventsResponse>(this.HttpClient, "clusters/events", request, cancellationToken)
                .ConfigureAwait(false);

            return response;
        }
    }
}

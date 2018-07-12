using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<string> Create(ClusterInfo clusterInfo)
        {
            var clusterIdentifier = await HttpPost<ClusterInfo, dynamic>(this.HttpClient, "clusters/create", clusterInfo)
                .ConfigureAwait(false);
            return clusterIdentifier.cluster_id.ToObject<string>();
        }

        public async Task Start(string clusterId)
        {
            await HttpPost(this.HttpClient, "clusters/start", new { cluster_id = clusterId }).ConfigureAwait(false);
        }

        public async Task Edit(string clusterId, ClusterInfo clusterConfig)
        {
            clusterConfig.ClusterId = clusterId;
            await HttpPost(this.HttpClient, "clusters/edit", clusterConfig).ConfigureAwait(false);
        }

        public async Task Restart(string clusterId)
        {
            await HttpPost(this.HttpClient, "clusters/restart", new { cluster_id = clusterId }).ConfigureAwait(false);
        }

        public async Task Resize(string clusterId, int numWorkers)
        {
            var request = new {cluster_id = clusterId, num_workers = numWorkers};
            await HttpPost(this.HttpClient, "clusters/resize", request).ConfigureAwait(false);
        }

        public async Task Resize(string clusterId, AutoScale autoScale)
        {
            var request = new {cluster_id = clusterId, autoscale = autoScale};
            await HttpPost(this.HttpClient, "clusters/resize", request).ConfigureAwait(false);
        }

        public async Task<ClusterInfo> Get(string clusterId)
        {
            var requestUri = $"clusters/get?cluster_id={clusterId}";
            return await HttpGet<ClusterInfo>(this.HttpClient, requestUri).ConfigureAwait(false);
        }

        public async Task Terminate(string clusterId)
        {
            await HttpPost(this.HttpClient, "clusters/delete", new { cluster_id = clusterId }).ConfigureAwait(false);
        }

        public async Task Delete(string clusterId)
        {
            await HttpPost(this.HttpClient, "clusters/permanent-delete", new { cluster_id = clusterId }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ClusterInfo>> List()
        {
            const string requestUri = "clusters/list";
            var clusterList = await HttpGet<dynamic>(this.HttpClient, requestUri).ConfigureAwait(false);
            return PropertyExists(clusterList, "clusters")
                ? clusterList.clusters.ToObject<IEnumerable<ClusterInfo>>()
                : Enumerable.Empty<ClusterInfo>();
        }

        public async Task Pin(string clusterId)
        {
            await HttpPost(this.HttpClient, "clusters/pin", new { cluster_id = clusterId }).ConfigureAwait(false);
        }

        public async Task Unpin(string clusterId)
        {
            await HttpPost(this.HttpClient, "clusters/unpin", new { cluster_id = clusterId }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<NodeType>> ListNodeTypes()
        {
            var result = await HttpGet<dynamic>(this.HttpClient, "clusters/list-node-types").ConfigureAwait(false);
            return result.node_types.ToObject<IEnumerable<NodeType>>();
        }

        public async Task<IDictionary<string, string>> ListSparkVersions()
        {
            var result = await HttpGet<dynamic>(this.HttpClient, "clusters/spark-versions").ConfigureAwait(false);
            return ((IEnumerable<dynamic>) result.versions).ToDictionary(
                sv => (string) sv.key.ToObject<string>(),
                sv => (string) sv.name.ToObject<string>()
            );
        }

        public async Task<EventsResponse> Events(string clusterId, DateTimeOffset? startTime, DateTimeOffset? endTime, ListOrder? order,
            IEnumerable<ClusterEventType> eventTypes, long? offset, long? limit)
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

            var response = await HttpPost<EventsRequest, EventsResponse>(this.HttpClient, "clusters/events", request)
                .ConfigureAwait(false);

            return response;
        }
    }
}

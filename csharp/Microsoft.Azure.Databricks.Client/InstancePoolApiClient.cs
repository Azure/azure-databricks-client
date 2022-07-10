using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class InstancePoolApiClient : ApiClient, IInstancePoolApi
    {
        /// <inheritdoc />
        public InstancePoolApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <inheritdoc />
        public async Task<string> Create(InstancePoolAttributes poolAttributes, CancellationToken cancellationToken = default)
        {
            var poolIdentifier =
                await HttpPost<InstancePoolAttributes, JsonObject>(
                    this.HttpClient,
                    $"{ApiVersion}/instance-pools/create",
                    poolAttributes, cancellationToken).ConfigureAwait(false);
            return poolIdentifier["instance_pool_id"].GetValue<string>();
        }

        /// <inheritdoc />
        public async Task Edit(string poolId, InstancePoolAttributes poolAttributes, CancellationToken cancellationToken = default)
        {
            var pool = new InstancePoolInfo
            {
                PoolId = poolId,
                PoolName = poolAttributes.PoolName,
                NodeTypeId = poolAttributes.NodeTypeId,
                MinIdleInstances = poolAttributes.MinIdleInstances,
                MaxCapacity = poolAttributes.MaxCapacity,
                IdleInstanceAutoTerminationMinutes = poolAttributes.IdleInstanceAutoTerminationMinutes
            };

            await HttpPost(this.HttpClient, $"{ApiVersion}/instance-pools/edit", pool, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task Delete(string poolId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, $"{ApiVersion}/instance-pools/delete", new {instance_pool_id = poolId}, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<InstancePoolInfo> Get(string poolId, CancellationToken cancellationToken = default)
        {
            var requestUri = $"{ApiVersion}/instance-pools/get?instance_pool_id={poolId}";
            return await HttpGet<InstancePoolInfo>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<InstancePoolInfo>> List(CancellationToken cancellationToken = default)
        {
            string requestUri = $"{ApiVersion}/instance-pools/list";
            var poolList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
            if (poolList.TryGetPropertyValue("instance_pools", out var instance_pools))
            {
                return instance_pools.Deserialize<IEnumerable<InstancePoolInfo>>();
            }
            else
            {
                return Enumerable.Empty<InstancePoolInfo>();
            }
        }
    }
}

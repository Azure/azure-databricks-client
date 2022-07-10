using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IInstancePoolApi: IDisposable
    {
        /// <summary>
        /// Create an instance pool.
        /// Use the returned instance_pool_id to query the status of the instance pool, which includes the number of instances currently allocated by the pool.
        /// If you provide the min_idle_instances parameter, instances are provisioned in the background and are ready to use once the idle_count in the InstancePoolStats equals the requested minimum.
        /// </summary>
        /// <returns>instance_pool_id</returns>
        Task<string> Create(InstancePoolAttributes poolAttributes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit an instance pool. This modifies the configuration of an existing instance pool.
        /// </summary>
        /// <remarks>
        /// - You can edit only the following fields: instance_pool_name, min_idle_instances, max_capacity, and idle_instance_autotermination_minutes.
        /// - You must supply an instance_pool_name.
        /// - You must supply a node_type_id and it must match the original node_type_id
        /// </remarks>
        Task Edit(string poolId, InstancePoolAttributes poolAttributes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an instance pool.
        /// This permanently deletes the instance pool.
        /// The idle instances in the pool are terminated asynchronously.
        /// New clusters cannot attach to the pool.
        /// Running clusters attached to the pool continue to run but cannot autoscale up.
        /// Terminated clusters attached to the pool will fail to start until they are edited to no longer use the pool.
        /// </summary>
        Task Delete(string poolId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the information for an instance pool given its identifier.
        /// </summary>
        Task<InstancePoolInfo> Get(string poolId, CancellationToken cancellationToken = default);

        /// <summary>
        /// List information for all instance pools.
        /// </summary>
        Task<IEnumerable<InstancePoolInfo>> List(CancellationToken cancellationToken = default);
    }
}

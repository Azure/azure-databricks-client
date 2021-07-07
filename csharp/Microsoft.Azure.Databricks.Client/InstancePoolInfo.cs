using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// The state of an instance pool. The current allowable state transitions are as follows:
    /// ACTIVE -> DELETE
    /// </summary>
    public enum InstancePoolState
    {
        /// <summary>
        /// Indicates an instance pool is active. Clusters can attach to it.
        /// </summary>
        ACTIVE,

        /// <summary>
        /// Indicates the instance pool has been deleted and is no longer accessible.
        /// </summary>
        DELETED
    }

    public class InstancePoolInfo : InstancePoolAttributes
    {
        /// <summary>
        /// The canonical unique identifier for the instance pool.
        /// </summary>
        [JsonProperty(PropertyName = "instance_pool_id")]
        public string PoolId { get; set; }

        /// <summary>
        /// Tags that are added by Azure Databricks regardless of any custom_tags, including
        ///     Vendor: Databricks
        ///     DatabricksInstancePoolCreatorId: create_user_id
        ///     DatabricksInstancePoolId: instance_pool_id
        /// </summary>
        [JsonProperty(PropertyName = "default_tags")]
        public Dictionary<string, string> DefaultTags { get; set; }

        /// <summary>
        /// Current state of the instance pool.
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public InstancePoolState State { get; set; }

        /// <summary>
        /// Statistics about the usage of the instance pool.
        /// </summary>
        [JsonProperty(PropertyName = "stats")]
        public InstancePoolStats Stats { get; set; }

        /// <summary>
        /// Status about failed pending instances in the pool.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public InstancePoolStatus Status { get; set; }
    }

    public class InstancePoolStatus
    {
        /// <summary>
        /// List of error messages for the failed pending instances.
        /// </summary>
        [JsonProperty(PropertyName = "pending_instance_errors")]
        public List<PendingInstanceError> PendingInstanceErrors { get; set; }
    }

    public class PendingInstanceError
    {
        /// <summary>
        /// ID of the failed instance.
        /// </summary>
        [JsonProperty(PropertyName = "instance_id")]
        public string InstanceId { get; set; }

        /// <summary>
        /// Message describing the cause of the failure.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
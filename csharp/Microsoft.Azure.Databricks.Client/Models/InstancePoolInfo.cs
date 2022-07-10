using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
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

    public record InstancePoolInfo : InstancePoolAttributes
    {
        /// <summary>
        /// The canonical unique identifier for the instance pool.
        /// </summary>
        [JsonPropertyName("instance_pool_id")]
        public string PoolId { get; set; }

        /// <summary>
        /// Tags that are added by Azure Databricks regardless of any custom_tags, including
        ///     Vendor: Databricks
        ///     DatabricksInstancePoolCreatorId: create_user_id
        ///     DatabricksInstancePoolId: instance_pool_id
        /// </summary>
        [JsonPropertyName("default_tags")]
        public Dictionary<string, string> DefaultTags { get; set; }

        /// <summary>
        /// Current state of the instance pool.
        /// </summary>
        [JsonPropertyName("state")]
        public InstancePoolState State { get; set; }

        /// <summary>
        /// Statistics about the usage of the instance pool.
        /// </summary>
        [JsonPropertyName("stats")]
        public InstancePoolStats Stats { get; set; }

        /// <summary>
        /// Status about failed pending instances in the pool.
        /// </summary>
        [JsonPropertyName("status")]
        public InstancePoolStatus Status { get; set; }
    }

    public record InstancePoolStatus
    {
        /// <summary>
        /// List of error messages for the failed pending instances.
        /// </summary>
        [JsonPropertyName("pending_instance_errors")]
        public List<PendingInstanceError> PendingInstanceErrors { get; set; }
    }

    public record PendingInstanceError
    {
        /// <summary>
        /// ID of the failed instance.
        /// </summary>
        [JsonPropertyName("instance_id")]
        public string InstanceId { get; set; }

        /// <summary>
        /// Message describing the cause of the failure.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
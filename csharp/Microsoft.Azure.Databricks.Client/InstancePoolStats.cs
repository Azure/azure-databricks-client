using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// Statistics about the usage of the instance pool.
    /// </summary>
    public class InstancePoolStats
    {
        /// <summary>
        /// Number of active instances that are in use by a cluster.
        /// </summary>
        [JsonProperty(PropertyName = "used_count")]
        public int UsedCount { get; set; }

        /// <summary>
        /// Number of active instances that are not in use by a cluster.
        /// </summary>
        [JsonProperty(PropertyName = "idle_count")]
        public int IdleCount { get; set; }

        /// <summary>
        /// Number of pending instances that are assigned to a cluster.
        /// </summary>
        [JsonProperty(PropertyName = "pending_used_count")]
        public int PendingUsedCount { get; set; }

        /// <summary>
        /// Number of pending instances that are not assigned to a cluster.
        /// </summary>
        [JsonProperty(PropertyName = "pending_idle_count")]
        public int PendingIdleCount { get; set; }

    }
}
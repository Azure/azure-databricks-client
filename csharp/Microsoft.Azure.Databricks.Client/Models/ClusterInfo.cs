using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// Describes all of the metadata about a single Spark cluster in Databricks.
    /// </summary>
    /// <seealso cref="T:Microsoft.Azure.Databricks.DatabricksClient.ClusterInstance" />
    public record ClusterInfo : ClusterAttributes
    {
        /// <summary>
        /// The canonical identifier for the cluster used by a run. This field is always available for runs on existing clusters. For runs on new clusters, it becomes available once the cluster is created. This value can be used to view logs by browsing to /#setting/sparkui/$cluster_id/driver-logs. The logs will continue to be available after the run completes.
        /// If this identifier is not yet available, the response won’t include this field.
        /// </summary>
        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The canonical identifier for the Spark context used by a run. This field will be filled in once the run begins execution. This value can be used to view the Spark UI by browsing to /#setting/sparkui/$cluster_id/$spark_context_id. The Spark UI will continue to be available after the run has completed.
        /// If this identifier is not yet available, the response won’t include this field.
        /// </summary>
        [JsonPropertyName("spark_context_id")]
        public long SparkContextId { get; set; }

        /// <summary>
        /// Creator user name. The field won’t be included in the response if the user has already been deleted.
        /// </summary>
        [JsonPropertyName("creator_user_name")]
        public string CreatorUserName { get; set; }

        /// <summary>
        /// Node on which the Spark driver resides. The driver node contains the Spark master and the Databricks application that manages the per-notebook Spark REPLs.
        /// </summary>
        [JsonPropertyName("driver")]
        public SparkNode Driver { get; set; }

        /// <summary>
        /// Nodes on which the Spark executors reside.
        /// </summary>
        [JsonPropertyName("executors")]
        public IEnumerable<SparkNode> Executors { get; set; }

        /// <summary>
        /// Port on which Spark JDBC server is listening, in the driver nod. No service will be listening on on this port in executor nodes.
        /// </summary>
        [JsonPropertyName("jdbc_port")]
        public int JdbcPort { get; set; }

        /// <summary>
        /// Current state of the cluster.
        /// </summary>
        [JsonPropertyName("state")]
        public ClusterState? State { get; set; }

        /// <summary>
        /// A message associated with the most recent state transition (e.g., the reason why the cluster entered a TERMINATED state).
        /// </summary>
        [JsonPropertyName("state_message")]
        public string StateMessage { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster creation request was received (when the cluster entered a PENDING state).
        /// </summary>
        [JsonPropertyName("start_time")]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster was terminated, if applicable.
        /// </summary>
        [JsonPropertyName("terminated_time")]
        public DateTimeOffset? TerminatedTime { get; set; }

        /// <summary>
        /// Time when the cluster driver last lost its state (due to a restart or driver failure).
        /// </summary>
        [JsonPropertyName("last_state_loss_time")]
        public DateTimeOffset? LastStateLossTime { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster was last active. A cluster is active if there is at least one command that has not finished on the cluster. This field is available after the cluster has reached a RUNNING state. Updates to this field are made as best-effort attempts. Certain versions of Spark do not support reporting of cluster activity. Refer to Automatic termination for details.
        /// </summary>
        [JsonPropertyName("last_activity_time")]
        public DateTimeOffset? LastActivityTime { get; set; }

        /// <summary>
        /// Total amount of cluster memory, in megabytes
        /// </summary>
        [JsonPropertyName("cluster_memory_mb")]
        public long ClusterMemoryMb { get; set; }

        /// <summary>
        /// Number of CPU cores available for this cluster. Note that this can be fractional, e.g. 7.5 cores, since certain node types are configured to share cores between Spark nodes on the same instance.
        /// </summary>
        [JsonPropertyName("cluster_cores")]
        public float ClusterCores { get; set; }

        /// <summary>
        /// Tags that are added by Databricks regardless of any custom_tags, including:
        ///     Vendor: Databricks
        ///     Creator: username_of_creator
        ///     ClusterName: name_of_cluster
        ///     ClusterId: id_of_cluster
        ///     Name: Databricks internal use
        /// </summary>
        [JsonPropertyName("default_tags")]
        public Dictionary<string, string> DefaultTags { get; set; }

        /// <summary>
        /// Cluster log delivery status.
        /// </summary>
        [JsonPropertyName("cluster_log_status")]
        public LogSyncStatus ClusterLogSyncStatus { get; set; }

        /// <summary>
        /// Information about why the cluster was terminated. This field only appears when the cluster is in a TERMINATING or TERMINATED state.
        /// </summary>
        [JsonPropertyName("termination_reason")]
        public TerminationReason TerminationReason { get; set; }

        [JsonPropertyName("pinned_by_user_name")]
        public string PinnedByUserName { get; set; }

        [JsonPropertyName("init_scripts_safe_mode")]
        public bool InitScriptsSafeMode { get; set; }

        /// <summary>
        /// Determines whether the cluster was created by a user through the UI, by the Databricks Jobs Scheduler, or through an API request.
        /// </summary>
        [JsonPropertyName("cluster_source")]
        public ClusterSource? ClusterSource { get; set; }
    }
}
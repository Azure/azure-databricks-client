using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// Identifiers for the cluster and Spark context used by a run. These two values together identify an execution context across all time.
    /// </summary>
    public record ClusterInstance
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
        public string SparkContextId { get; set; }
    }
}
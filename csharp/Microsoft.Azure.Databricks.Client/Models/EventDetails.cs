using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record EventDetails
    {
        /// <summary>
        /// The current number of nodes in the cluster.
        /// </summary>
        [JsonPropertyName("current_num_workers")]
        public int CurrentNumWorkers { get; set; }

        /// <summary>
        /// The targeted number of nodes in the cluster.
        /// </summary>
        [JsonPropertyName("target_num_workers")]
        public int TargetNumWorkers { get; set; }

        /// <summary>
        /// The cluster attributes before a cluster was edited.
        /// </summary>
        [JsonPropertyName("previous_attributes")]
        public ClusterAttributes PreviousAttributes { get; set; }

        /// <summary>
        /// For created clusters, the attributes of the cluster.
        /// For edited clusters, the new attributes of the cluster.
        /// </summary>
        [JsonPropertyName("attributes")]
        public ClusterAttributes Attributes { get; set; }

        /// <summary>
        /// The size of the cluster before an edit or resize.
        /// </summary>
        [JsonPropertyName("previous_cluster_size")]
        public ClusterSize PreviousClusterSize { get; set; }

        /// <summary>
        /// The actual cluster size that was set in the cluster creation or edit.
        /// </summary>
        [JsonPropertyName("cluster_size")]
        public ClusterSize ClusterSize { get; set; }

        /// <summary>
        /// The cause of a change in target size.
        /// </summary>
        [JsonPropertyName("cause")]
        public ResizeCause Cause { get; set; }

        /// <summary>
        /// A termination reason:
        ///     On a TERMINATED event, the reason for the termination.
        ///     On a RESIZE_COMPLETE event, indicates the reason that we failed to acquire some nodes.
        /// </summary>
        [JsonPropertyName("reason")]
        public TerminationReason Reason { get; set; }

        /// <summary>
        /// The user that caused the event to occur. (Empty if it was done by the control plane.)
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; }
    }
}
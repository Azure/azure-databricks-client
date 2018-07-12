using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class EventDetails
    {
        /// <summary>
        /// The current number of nodes in the cluster.
        /// </summary>
        [JsonProperty(PropertyName = "current_num_workers")]
        public int CurrentNumWorkers { get; set; }

        /// <summary>
        /// The targeted number of nodes in the cluster.
        /// </summary>
        [JsonProperty(PropertyName = "target_num_workers")]
        public int TargetNumWorkers { get; set; }

        /// <summary>
        /// The cluster attributes before a cluster was edited.
        /// </summary>
        [JsonProperty(PropertyName = "previous_attributes")]
        public ClusterAttributes PreviousAttributes { get; set; }

        /// <summary>
        /// For created clusters, the attributes of the cluster.
        /// For edited clusters, the new attributes of the cluster.
        /// </summary>
        [JsonProperty(PropertyName = "attributes")]
        public ClusterAttributes Attributes { get; set; }

        /// <summary>
        /// The size of the cluster before an edit or resize.
        /// </summary>
        [JsonProperty(PropertyName = "previous_cluster_size")]
        public ClusterSize PreviousClusterSize { get; set; }

        /// <summary>
        /// The actual cluster size that was set in the cluster creation or edit.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_size")]
        public ClusterSize ClusterSize { get; set; }

        /// <summary>
        /// The cause of a change in target size.
        /// </summary>
        [JsonProperty(PropertyName = "cause")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResizeCause Cause { get; set; }

        /// <summary>
        /// A termination reason:
        ///     On a TERMINATED event, the reason for the termination.
        ///     On a RESIZE_COMPLETE event, indicates the reason that we failed to acquire some nodes.
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public TerminationReason Reason { get; set; }

        /// <summary>
        /// The user that caused the event to occur. (Empty if it was done by the control plane.)
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public string User { get; set; }
    }
}
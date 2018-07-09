using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class NodeType
    {
        /// <summary>
        /// Unique identifier for this node type. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "node_type_id")]
        public string NodeTypeId { get; set; }

        /// <summary>
        /// Memory (in MB) available for this node type. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "memory_mb")]
        public int MemoryMb { get; set; }

        /// <summary>
        /// Number of CPU cores available for this node type. Note that this can be fractional, e.g., 2.5 cores, if the the number of cores on a machine instance is not divisible by the number of Spark nodes on that machine. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "num_cores")]
        public float NumCores { get; set; }

        /// <summary>
        /// A string description associated with this node type, e.g., “i3.xlarge”. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// An identifier for the type of hardware that this node runs on, e.g., “r3.2xlarge” in AWS. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "instance_type_id")]
        public string InstanceTypeId { get; set; }

        /// <summary>
        /// Whether the node type is deprecated. Non-deprecated node types offer greater performance.
        /// </summary>
        [JsonProperty(PropertyName = "is_deprecated")]
        public bool IsDeprecated { get; set; }

        [JsonProperty(PropertyName = "node_info")]
        public NodeInfo NodeInfo { get; set; }
    }
}
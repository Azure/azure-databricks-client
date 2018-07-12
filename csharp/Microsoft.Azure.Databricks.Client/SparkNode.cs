using System;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// Describes a specific Spark driver or executor.
    /// </summary>
    public class SparkNode
    {
        /// <summary>
        /// Private IP address (typically a 10.x.x.x address) of the Spark node. Note that this is different from the private IP address of the host instance.
        /// </summary>
        [JsonProperty(PropertyName = "private_ip")]
        public string PrivateIp { get; set; }

        /// <summary>
        /// Public DNS address of this node. This address can be used to access the Spark JDBC server on the driver node.
        /// </summary>
        [JsonProperty(PropertyName = "public_dns")]
        public string PublicDns { get; set; }

        /// <summary>
        /// Globally unique identifier for this node.
        /// </summary>
        [JsonProperty(PropertyName = "node_id")]
        public string NodeId { get; set; }

        /// <summary>
        /// Globally unique identifier for the host instance from the cloud provider.
        /// </summary>
        [JsonProperty(PropertyName = "instance_id")]
        public string InstanceId { get; set; }

        /// <summary>
        /// The timestamp when the Spark node is launched.
        /// </summary>
        [JsonProperty(PropertyName = "start_timestamp")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset StartTimestamp { get; set; }

        /// <summary>
        /// The private IP address of the host instance.
        /// </summary>
        [JsonProperty(PropertyName = "host_private_ip")]
        public string HostPrivateIp { get; set; }
    }
}
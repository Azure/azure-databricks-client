using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// Describes the initial set of disks to attach to each instance.
    /// For example, if there are 3 instances and each instance is configured to start with 2 disks, 100 GiB each, then Databricks creates a total of 6 disks, 100 GiB each, for these instances
    /// </summary>
    public record DiskSpec
    {
        /// <summary>
        /// The type of disks to attach.
        /// </summary>
        [JsonPropertyName("disk_type")]
        public DiskType DiskType { get; set; }

        /// <summary>
        /// The number of disks to attach to each instance:
        /// - This feature is only enabled for supported node types.
        /// - Users can choose up to the limit of the disks supported by the node type.
        /// - For node types with no local disk, at least one disk needs to be specified.
        /// </summary>
        [JsonPropertyName("disk_count")]
        public int DiskCount { get; set; }

        /// <summary>
        /// The size of each disk (in GiB) to attach. Values must fall into the supported range for a particular instance type:
        /// Azure:
        /// - Premium LRS (SSD): 1 - 1023 GiB
        /// - Standard LRS (HDD): 1- 1023 GiB
        /// AWS:
        /// - General Purpose SSD: 100 - 4096 GiB
        /// - Throughput Optimized HDD: 500 - 4096 GiB
        /// </summary>
        [JsonPropertyName("disk_size")]
        public int DiskSize { get; set; }
    }
}
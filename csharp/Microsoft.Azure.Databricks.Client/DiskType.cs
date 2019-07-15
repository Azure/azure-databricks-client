using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public enum AzureDiskVolumeType
    {
        /// <summary>
        /// Premium storage tier, backed by SSDs.
        /// </summary>
        PREMIUM_LRS,

        /// <summary>
        /// Standard storage tier, backed by HDDs.
        /// </summary>
        STANDARD_LRS
    }

    public enum EbsVolumeType
    {
        /// <summary>
        /// Provision extra storage using AWS gp2 EBS volumes.
        /// </summary>
        GENERAL_PURPOSE_SSD,

        /// <summary>
        /// Provision extra storage using AWS st1 volumes.
        /// </summary>
        THROUGHPUT_OPTIMIZED_HDD
    }

    public class DiskType
    {
        /// <summary>
        /// The EBS volume type to use.
        /// </summary>
        [JsonProperty(PropertyName = "ebs_volume_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EbsVolumeType? EbsVolumeType { get; set; }

        /// <summary>
        /// The type of Azure Disk to use.
        /// </summary>
        [JsonProperty(PropertyName = "azure_disk_volume_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AzureDiskVolumeType? AzureDiskVolumeType { get; set; }

        public static DiskType FromAzureDisk(AzureDiskVolumeType volumeType) => new DiskType {AzureDiskVolumeType = volumeType};

        public static DiskType FromEbsDisk(EbsVolumeType volumeType) => new DiskType {EbsVolumeType = volumeType};
    }
}
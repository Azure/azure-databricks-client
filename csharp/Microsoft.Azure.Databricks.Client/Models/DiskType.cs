using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
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

    public record DiskType
    {
        /// <summary>
        /// The EBS volume type to use.
        /// </summary>
        [JsonPropertyName("ebs_volume_type")]
        public EbsVolumeType? EbsVolumeType { get; set; }

        /// <summary>
        /// The type of Azure Disk to use.
        /// </summary>
        [JsonPropertyName("azure_disk_volume_type")]
        public AzureDiskVolumeType? AzureDiskVolumeType { get; set; }

        public static DiskType FromAzureDisk(AzureDiskVolumeType volumeType) => new() { AzureDiskVolumeType = volumeType };

        public static DiskType FromEbsDisk(EbsVolumeType volumeType) => new() { EbsVolumeType = volumeType };
    }
}
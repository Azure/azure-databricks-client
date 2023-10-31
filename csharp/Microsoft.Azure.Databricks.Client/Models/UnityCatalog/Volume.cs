using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Volume : VolumeAttributes
{
    /// <summary>
    /// The identifier of the user who owns the volume
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// The three-level (fully qualified) name of the volume
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    /// <summary>
    /// The unique identifier of the volume
    /// </summary>
    [JsonPropertyName("volume_id")]
    public string VolumeId { get; set; }

    /// <summary>
    /// The unique identifier of the metastore
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// The identifier of the user who created the volume
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// The identifier of the user who updated the volume last time
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }
}

public enum VolumeType
{
    EXTERNAL,
    MANAGED
}

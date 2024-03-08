using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record VolumeAttributes
{
    /// <summary>
    /// The name of the catalog where the schema and the volume are
    /// </summary>
    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    /// <summary>
    /// The name of the schema where the volume is
    /// </summary>
    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    /// <summary>
    /// The name of the volume
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Enum: "EXTERNAL" "MANAGED"
    /// </summary>
    [JsonPropertyName("volume_type")]
    public VolumeType? VolumeType { get; set; }

    /// <summary>
    /// The storage location on the cloud
    /// </summary>
    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }

    /// <summary>
    /// The comment attached to the volume
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}

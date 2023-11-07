using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record SchemaAttributes
{
    /// <summary>
    /// Name of the schema, relative to the parent catalog.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Name of the parent catalog.
    /// </summary>
    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// A map of key-value properties attached to the securable.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, string> Properties { get; set; }

    /// <summary>
    /// Storage root URL for managed tables within the schema.
    /// </summary>
    [JsonPropertyName("storage_root")]
    public string StorageRoot { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Schema
{
    /// <summary>
    /// Name of the schema, relative to the parent catalog.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Name of the parent catalog.
    /// </summary>
    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; init; }

    /// <summary>
    /// Username of the current owner of the schema.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; init; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; init; }

    /// <summary>
    /// A map of key-value properties attached to the securable.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, string> Properties { get; init; }

    /// <summary>
    /// Storage root URL for managed tables within the schema.
    /// </summary>
    [JsonPropertyName("storage_root")]
    public string StorageRoot { get; init; }

    /// <summary>
    /// Unique identifier of the parent metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; init; }

    /// <summary>
    /// Full name of the schema, in the form of catalog_name.schema_name.
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; init; }

    /// <summary>
    /// Storage location for managed tables within the schema.
    /// </summary>
    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; init; }

    /// <summary>
    /// Time at which this schema was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    /// Username of the schema creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; init; }

    /// <summary>
    /// Time at which this schema was last modified, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    /// Username of the user who last modified the schema.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; init; }

    /// <summary>
    /// The type of the parent catalog.
    /// </summary>
    [JsonPropertyName("catalog_type")]
    public string CatalogType { get; init; }
}

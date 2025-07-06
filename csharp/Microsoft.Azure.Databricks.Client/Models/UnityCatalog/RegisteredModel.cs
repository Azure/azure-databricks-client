using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record RegisteredModel
{
    /// <summary>
    /// List of aliases associated with the registered model
    /// </summary>
    [JsonPropertyName("aliases")]
    public IEnumerable<RegisteredModelAlias> Aliases { get; set; }

    /// <summary>
    /// Indicates whether the principal is limited to retrieving metadata for the associated object through the BROWSE privilege when include_browse is enabled in the request.
    /// </summary>
    [JsonPropertyName("browse_only")]
    public bool BrowseOnly { get; set; }

    /// <summary>
    /// The name of the registered model
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The name of the catalog where the schema and the registered model reside
    /// </summary>
    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    /// <summary>
    /// The name of the schema where the registered model resides
    /// </summary>
    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    /// <summary>
    /// The three-level (fully qualified) name of the registered model
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    /// <summary>
    /// The identifier of the user who owns the registered model
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// The unique identifier of the metastore
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// Creation timestamp of the registered model in milliseconds since the Unix epoch
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// The identifier of the user who created the registered model
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Last-update timestamp of the registered model in milliseconds since the Unix epoch
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// The identifier of the user who updated the registered model last time
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// The storage location on the cloud under which model version data files are stored
    /// </summary>
    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }

    [JsonPropertyName("securable_type")]
    public string SecurableType { get; set; }

    [JsonPropertyName("securable_kind")]
    public string SecurableKind { get; set; }

    /// <summary>
    /// The comment attached to the registered model
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}

public record RegisteredModelAlias
{
    [JsonPropertyName("alias_name")]
    public string AliasName { get; set; }

    [JsonPropertyName("version_num")]
    public int VersionNum { get; set; }
}

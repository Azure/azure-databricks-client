using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record RegisteredModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }

    [JsonPropertyName("securable_type")]
    public string SecurableType { get; set; }

    [JsonPropertyName("securable_kind")]
    public string SecurableKind { get; set; }

    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}

public record RegisteredModelAlias
{
    [JsonPropertyName("alias_name")]
    public string? AliasName { get; set; }

    [JsonPropertyName("version_num")]
    public int VersionNum { get; set; }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
public record ModelVersion
{
    [JsonPropertyName("model_name")]
    public string ModelName { get; set; }

    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("run_id")]
    public string RunId { get; set; }

    [JsonPropertyName("run_workspace_id")]
    public long RunWorkspaceId { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    [JsonPropertyName("created_at")]
    public long CreatedAt { get; set; }

    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    [JsonPropertyName("updated_at")]
    public long UpdatedAt { get; set; }

    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }

    [JsonPropertyName("aliases")]
    public IEnumerable<Alias>? Aliases { get; set; }
}

public record Alias
{
    [JsonPropertyName("alias_name")]
    public string AliasName { get; set; }

    [JsonPropertyName("version_num")]
    public int VersionNum { get; set; }
}

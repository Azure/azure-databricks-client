using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record LineageObjectAttributes
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    [JsonPropertyName("table_type")]
    public string TableType { get; set; }
}

public record TableInfo : LineageObjectAttributes
{
}

public record ColumnInfo : LineageObjectAttributes
{
    [JsonPropertyName("table_name")]
    public string TableName { get; set; }
}

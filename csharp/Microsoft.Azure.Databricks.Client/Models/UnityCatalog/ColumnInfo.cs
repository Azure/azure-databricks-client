using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record ColumnInfo : LineageObjectAttributes
{
    [JsonPropertyName("table_name")]
    public string TableName { get; set; }
}

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record TableSummary
{
    /// <summary>
    /// Full name of table, in form of catalog_name.schema_name.table_name
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    /// <summary>
    /// Enum: "MANAGED" "EXTERNAL" "VIEW" "MATERIALIZED_VIEW" "STREAMING_TABLE"
    /// </summary>
    [JsonPropertyName("table_type")]
    public TableType? TableType { get; set; }
}

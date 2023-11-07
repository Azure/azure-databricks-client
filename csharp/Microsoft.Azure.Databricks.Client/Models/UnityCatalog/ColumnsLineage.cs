using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record ColumnsLineage
{
    [JsonPropertyName("upstream_cols")]
    public IEnumerable<ColumnInfo> UpstreamColumns { get; set; }

    [JsonPropertyName("downstream_cols")]
    public IEnumerable<ColumnInfo> DownstreamColumns { get; private set; }
}
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record TablesLineage
{
    [JsonPropertyName("upstreams")]
    public IEnumerable<ObjectsLineageStream> Upstreams { get; set; }

    [JsonPropertyName("downstreams")]
    public IEnumerable<ObjectsLineageStream> Downstreams { get; set; }
}

public record ObjectsLineageStream
{
    [JsonPropertyName("tableInfo")]
    public TableInfo TableInfo { get; set; }
}
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

    [JsonPropertyName("pipelineInfos")]
    public IEnumerable<PipelineInfo> PipelineInfos { get; set; }
}

public record PipelineInfo
{
    [JsonPropertyName("pipeline_id")]
    public string PipelineId { get; set; }

    [JsonPropertyName("update_id")]
    public string UpdateId { get; set; }
}
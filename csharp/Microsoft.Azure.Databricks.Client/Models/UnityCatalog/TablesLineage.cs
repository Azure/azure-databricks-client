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

    [JsonPropertyName("jobInfos")]
    public IEnumerable<JobInfo> JobInfos { get; set; }
}

public record PipelineInfo
{
    [JsonPropertyName("pipeline_id")]
    public string PipelineId { get; set; }

    [JsonPropertyName("update_id")]
    public string UpdateId { get; set; }
}

public record JobInfo
{
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }

    [JsonPropertyName("job_id")]
    public long JobId { get; set; }
}

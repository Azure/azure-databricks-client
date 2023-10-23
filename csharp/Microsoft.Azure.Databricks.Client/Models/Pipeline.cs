using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record PipelineLatestUpdate
{
    /// <summary>
    /// The ID of the update.
    /// </summary>
    [JsonPropertyName("update_id")]
    public string UpdateId { get; set; }


    /// <summary>
    /// The update state.
    /// </summary>
    [JsonPropertyName("state")]
    public PipelineUpdateState? State { get; set; }

    /// <summary>
    /// The time when this update was created.
    /// </summary>
    [JsonPropertyName("creation_time")]
    public string CreationTime { get; set; }
}

public record PipelineFilters
{
    /// <summary>
    /// Paths to include.
    /// </summary>
    [JsonPropertyName("include")]
    public IEnumerable<string> Include { get; set; }

    /// <summary>
    /// Paths to exclude.
    /// </summary>
    [JsonPropertyName("exclude")]
    public IEnumerable<string> Exclude { get; set; }
}

public record Pipeline
{
    /// <summary>
    /// The ID of the pipeline.
    /// </summary>
    [JsonPropertyName("pipeline_id")]
    public string PipelineId { get; set; }

    /// <summary>
    /// The pipeline specification. This field is not returned when called by ListPipelines.
    /// </summary>
    [JsonPropertyName("spec")]
    public PipelineSpecification Specification { get; set; }

    /// <summary>
    /// The pipeline state.
    /// </summary>
    [JsonPropertyName("state")]
    public PipelineState? State { get; set; }

    /// <summary>
    /// An optional message detailing the cause of the pipeline state.
    /// </summary>
    [JsonPropertyName("cause")]
    public string Cause { get; set; }

    /// <summary>
    /// The ID of the cluster that the pipeline is running on.
    /// </summary>
    [JsonPropertyName("cluster_id")]
    public string ClusterId { get; set; }

    /// <summary>
    /// A human friendly identifier for the pipeline, taken from the spec.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The health of a pipeline.
    /// </summary>
    [JsonPropertyName("health")]
    public PipelineHealth? Health { get; set; }

    /// <summary>
    /// The username of the pipeline creator.
    /// </summary>
    [JsonPropertyName("creator_user_name")]
    public string CreatorUserName { get; set; }

    /// <summary>
    /// Status of the latest updates for the pipeline. Ordered with the newest update first.
    /// </summary>
    [JsonPropertyName("latest_updates")]
    public IEnumerable<PipelineLatestUpdate> LatestUpdates { get; set; }

    /// <summary>
    /// The last time the pipeline settings were modified or created.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTimeOffset? LastModified { get; set; }

    /// <summary>
    /// Username of the user that the pipeline will run on behalf of.
    /// </summary>
    [JsonPropertyName("run_as_user_name")]
    public string RunAsUserName { get; set; }
}

public enum PipelineHealth
{
    HEALTHY,
    UNHEALTHY
}

/// <summary>
/// States in which pipeline can be
/// </summary>
public enum PipelineState
{
    DEPLOYING,
    STARTING,
    RUNNING,
    STOPPING,
    DELETED,
    RECOVERING,
    FAILED,
    RESETTING,
    IDLE
}
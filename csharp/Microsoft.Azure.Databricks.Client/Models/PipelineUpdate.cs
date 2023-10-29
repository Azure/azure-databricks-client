using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record PipelineUpdate
{
    /// <summary>
    /// The ID of the pipeline.
    /// </summary>
    [JsonPropertyName("pipeline_id")]
    public string PipelineId { get; set; }

    /// <summary>
    /// The ID of this update.
    /// </summary>
    [JsonPropertyName("update_id")]
    public string UpdateId { get; set; }

    /// <summary>
    /// The pipeline configuration with system defaults applied where unspecified by the user. Not returned by ListUpdates.
    /// </summary>
    [JsonPropertyName("config")]
    public PipelineSpecification PipelineConfig { get; set; }

    /// <summary>
    /// What triggered this update.
    /// </summary>
    [JsonPropertyName("cause")]
    public PipelineUpdateCause? Cause { get; set; }

    /// <summary>
    /// The update state.
    /// </summary>
    [JsonPropertyName("state")]
    public PipelineUpdateState? State { get; set; }

    /// <summary>
    /// The ID of the cluster that the update is running on.
    /// </summary>
    [JsonPropertyName("cluster_id")]
    public string ClusterId { get; set; }

    /// <summary>
    /// The time when this update was created.
    /// </summary>
    [JsonPropertyName("creation_time")]
    public DateTimeOffset? CreationTime { get; set; }

    /// <summary>
    /// If true, this update will reset all tables before running.
    /// </summary>
    [JsonPropertyName("full_refresh")]
    public bool FullRefresh { get; set; }

    /// <summary>
    /// A list of tables to update without fullRefresh. 
    /// If both refresh_selection and full_refresh_selection are empty, this is a full graph update. 
    /// Full Refresh on a table means that the states of the table will be reset before the refresh.
    /// </summary>
    [JsonPropertyName("refresh_selection")]
    public IEnumerable<string> RefreshSelection { get; set; }

    /// <summary>
    /// A list of tables to update with fullRefresh. 
    /// If both refresh_selection and full_refresh_selection are empty, this is a full graph update. 
    /// Full Refresh on a table means that the states of the table will be reset before the refresh.
    /// </summary>
    [JsonPropertyName("full_refresh_selection")]
    public IEnumerable<string> FullRefreshSelection { get; set; }
}

public enum PipelineUpdateCause
{
    API_CALL,
    RETRY_ON_FAILURE,
    SERVICE_UPGRADE,
    SCHEMA_CHANGE,
    JOB_TASK,
    USER_ACTION
}

public enum PipelineUpdateState
{
    QUEUED,
    CREATED,
    WAITING_FOR_RESOURCES,
    INITIALIZING,
    RESETTING,
    SETTING_UP_TABLES,
    RUNNING,
    STOPPING,
    COMPLETED,
    FAILED,
    CANCELED
}

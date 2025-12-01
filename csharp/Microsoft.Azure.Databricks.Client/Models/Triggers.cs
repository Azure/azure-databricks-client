// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// Continuous trigger settings for a job.
/// When you run your job with the continuous trigger, Databricks Jobs ensures there is always one active run of the job.
/// </summary>
public record ContinuousTrigger
{
    /// <summary>
    /// Indicate whether the continuous execution of the job is paused or not.
    /// </summary>
    [JsonPropertyName("pause_status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PauseStatus? PauseStatus { get; set; }
}

/// <summary>
/// File arrival trigger settings for a job.
/// Indicates a job that is triggered by file arrival.
/// </summary>
public record FileArrivalTrigger
{
    /// <summary>
    /// URL to be monitored for file arrivals.
    /// The path must point to the root or a subpath of the Unity Catalog external location or volume.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// If set, the trigger starts a run only after the specified amount of time passed since the last time the trigger fired.
    /// The minimum allowed value is 60 seconds.
    /// </summary>
    [JsonPropertyName("min_time_between_triggers_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinTimeBetweenTriggersSeconds { get; set; }

    /// <summary>
    /// If set, the trigger starts a run only after no file activity has occurred for the specified amount of time.
    /// This makes it possible to wait for a batch of incoming files to arrive before triggering a run.
    /// The minimum allowed value is 60 seconds.
    /// </summary>
    [JsonPropertyName("wait_after_last_change_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? WaitAfterLastChangeSeconds { get; set; }
}

/// <summary>
/// The condition that determines whether the trigger fires.
/// </summary>
public enum TableUpdateTriggerCondition
{
    /// <summary>
    /// When all tables are updated.
    /// </summary>
    ALL_UPDATED,

    /// <summary>
    /// When any table is updated.
    /// </summary>
    ANY_UPDATED
}

/// <summary>
/// Table update trigger settings for a job.
/// Indicates a job that is triggered by a table update.
/// </summary>
public record TableUpdateTrigger
{
    /// <summary>
    /// A list of Unity Catalog table names to monitor for changes.
    /// Maximum of 10 tables. Each table name must be a valid fully qualified Unity Catalog table name.
    /// </summary>
    [JsonPropertyName("table_names")]
    public List<string> TableNames { get; set; }

    /// <summary>
    /// The condition that determines whether the trigger fires.
    /// </summary>
    [JsonPropertyName("condition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TableUpdateTriggerCondition? Condition { get; set; }

    /// <summary>
    /// If set, the trigger starts a run only after the specified amount of time has passed since the last time the trigger fired.
    /// The minimum allowed value is 60 seconds.
    /// </summary>
    [JsonPropertyName("min_time_between_triggers_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MinTimeBetweenTriggersSeconds { get; set; }

    /// <summary>
    /// If set, the trigger starts a run only after no table update has occurred for the specified amount of time.
    /// This makes it possible to wait for a batch of updates before triggering a run.
    /// The minimum allowed value is 60 seconds.
    /// </summary>
    [JsonPropertyName("wait_after_last_change_seconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? WaitAfterLastChangeSeconds { get; set; }
}

/// <summary>
/// Trigger settings that define when a job should run.
/// </summary>
public record TriggerSettings
{
    /// <summary>
    /// File arrival trigger settings.
    /// </summary>
    [JsonPropertyName("file_arrival")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FileArrivalTrigger FileArrival { get; set; }

    /// <summary>
    /// Table update trigger settings.
    /// </summary>
    [JsonPropertyName("table_update")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TableUpdateTrigger TableUpdate { get; set; }

    /// <summary>
    /// Indicate whether this trigger is paused or not.
    /// </summary>
    [JsonPropertyName("pause_status")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PauseStatus? PauseStatus { get; set; }
}

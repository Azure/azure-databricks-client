// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record JobNotifications<T>
{
    /// <summary>
    /// A list of notifications to be notified when a run begins. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
    /// </summary>
    [JsonPropertyName("on_start")]
    public IEnumerable<T> OnStart { get; set; }

    /// <summary>
    /// A list of notifications to be notified when a run successfully completes. A run is considered to have completed successfully if it ends with a TERMINATED life_cycle_state and a SUCCESSFUL result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
    /// </summary>
    [JsonPropertyName("on_success")]
    public IEnumerable<T> OnSuccess { get; set; }

    /// <summary>
    /// A list of notifications to be notified when a run unsuccessfully completes. A run is considered to have completed unsuccessfully if it ends with an INTERNAL_ERROR life_cycle_state or a SKIPPED, FAILED, or TIMED_OUT result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
    /// </summary>
    [JsonPropertyName("on_failure")]
    public IEnumerable<T> OnFailure { get; set; }

    /// <summary>
    /// An optional list of notifications to call when the duration of a run exceeds the threshold specified for the RUN_DURATION_SECONDS metric in the health field.
    /// A maximum of 3 destinations can be specified for the on_duration_warning_threshold_exceeded property.
    /// </summary>
    [JsonPropertyName("on_duration_warning_threshold_exceeded")]
    public IEnumerable<T> OnDurationWarningThresholdExceeded { get; set; }

    /// <summary>
    /// An optional list of notifications to call when any streaming backlog thresholds are exceeded for any stream.
    /// Streaming backlog thresholds can be set in the health field using the following metrics: STREAMING_BACKLOG_BYTES, STREAMING_BACKLOG_RECORDS, STREAMING_BACKLOG_SECONDS, or STREAMING_BACKLOG_FILES.
    /// Alerting is based on the 10-minute average of these metrics. If the issue persists, notifications are resent every 30 minutes.
    /// A maximum of 3 destinations can be specified for the on_streaming_backlog_exceeded property.
    /// </summary>
    [JsonPropertyName("on_streaming_backlog_exceeded")]
    public IEnumerable<T> OnStreamingBacklogExceeded { get; set; }
}

public record JobEmailNotifications: JobNotifications<string>
{
    /// <summary>
    /// If true, do not send email to recipients specified in on_failure if the run is skipped.
    /// </summary>
    [Obsolete("This property is deprecated. Please use NotificationSettings.NoAlertForSkippedRuns property.")]
    [JsonPropertyName("no_alert_for_skipped_runs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool NoAlertForSkippedRuns { get; set; }
}

public record JobWebhookNotifications: JobNotifications<JobWebhookSetting>
{
}

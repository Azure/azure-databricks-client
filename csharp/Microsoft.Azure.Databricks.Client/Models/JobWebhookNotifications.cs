// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record JobWebhookNotifications
{
    /// <summary>
    /// A list of email addresses to be notified when a run begins. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
    /// </summary>
    [JsonPropertyName("on_start")]
    public IEnumerable<JobWebhookSetting> OnStart { get; set; }

    /// <summary>
    /// A list of email addresses to be notified when a run successfully completes. A run is considered to have completed successfully if it ends with a TERMINATED life_cycle_state and a SUCCESSFUL result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
    /// </summary>
    [JsonPropertyName("on_success")]
    public IEnumerable<JobWebhookSetting> OnSuccess { get; set; }

    /// <summary>
    /// A list of email addresses to be notified when a run unsuccessfully completes. A run is considered to have completed unsuccessfully if it ends with an INTERNAL_ERROR life_cycle_state or a SKIPPED, FAILED, or TIMED_OUT result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
    /// </summary>
    [JsonPropertyName("on_failure")]
    public IEnumerable<JobWebhookSetting> OnFailure { get; set; }
}
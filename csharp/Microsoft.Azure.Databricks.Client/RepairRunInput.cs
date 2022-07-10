using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client;

public record RepairRunInput
{
    /// <summary>
    /// The job run ID of the run to repair. The run must not be in
    /// progress.
    /// </summary>
    /// <example>455644833</example>
    [JsonPropertyName("run_id")]
    public long RunId { get; set; }

    /// <summary>
    /// The task keys of the task runs to repair.
    /// </summary>
    [JsonPropertyName("rerun_tasks")]
    public IEnumerable<string> RerunTasks { get; set; }

    /// <summary>
    /// The ID of the latest repair. This parameter is not required when
    /// repairing a run for the first time, but must be provided on
    /// subsequent requests to repair the same run.
    /// </summary>
    [JsonPropertyName("latest_repair_id")]
    public long LatestRepairId { get; set; }
}
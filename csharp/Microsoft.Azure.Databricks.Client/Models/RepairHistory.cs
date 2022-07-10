using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models;

public enum RepairHistoryItemType
{
    ORIGINAL,
    REPAIR
}

public record RepairHistory
{
    [JsonPropertyName("repair_history")]
    public IEnumerable<RepairHistoryItem> Items { get; set; }
}

public record RepairHistoryItem
{
    /// <summary>
    /// The repair history item type. Indicates whether a run is the
    /// original run or a repair run.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public RepairHistoryItemType @Type { get; set; }

    /// <summary>
    /// The start time of the (repaired) run.
    /// </summary>
    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// The end time of the (repaired) run.
    /// </summary>
    [JsonPropertyName("end_time")]
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// The result and lifecycle states of the (repaired) run.
    /// </summary>
    [JsonPropertyName("state")]
    public RunState State { get; set; }

    /// <summary>
    /// The ID of the repair. Only returned for the items that represent a
    /// repair in `repair_history`.
    /// </summary>
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    /// <summary>
    /// The run IDs of the task runs that ran as part of this repair history
    /// item.
    /// </summary>
    [JsonPropertyName("task_run_ids")]
    public long[] TaskRunIds { get; set; }
}


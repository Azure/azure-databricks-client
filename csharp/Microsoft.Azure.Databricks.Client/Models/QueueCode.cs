namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// Provides the reason for queuing the run.
/// </summary>
public enum QueueCode
{
    /// <summary>
    /// The run was queued due to reaching the workspace limit of active task runs.
    /// </summary>
    ACTIVE_RUNS_LIMIT_REACHED,

    /// <summary>
    /// The run was queued due to reaching the per-job limit of concurrent job runs.
    /// </summary>
    MAX_CONCURRENT_RUNS_REACHED,

    /// <summary>
    /// The run was queued due to reaching the workspace limit of active run job tasks.
    /// </summary>
    ACTIVE_RUN_JOB_TASKS_LIMIT_REACHED
}
// ReSharper disable InconsistentNaming
namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// The result state of the run.
    ///     If life_cycle_state = TERMINATED: if the run had a task, the result is guaranteed to be available, and it indicates the result of the task.
    ///     If life_cycle_state = PENDING, RUNNING, or SKIPPED, the result state is not available.
    ///     If life_cycle_state = TERMINATING or lifecyclestate = INTERNAL_ERROR: the result state is available if the run had a task and managed to start it.
    /// Once available, the result state will never change.
    /// </summary>
    public enum RunResultState
    {
        /// <summary>
        /// The task completed successfully.
        /// </summary>
        SUCCESS,

        /// <summary>
        /// The task completed with an error.
        /// </summary>
        FAILED,

        /// <summary>
        /// The run was stopped after reaching the timeout.
        /// </summary>
        TIMEDOUT,

        /// <summary>
        /// The run was canceled at user request.
        /// </summary>
        CANCELED
    }
}
// ReSharper disable InconsistentNaming
namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// These are the type of triggers that can fire a run.
    /// </summary>
    public enum TriggerType
    {
        /// <summary>
        /// These are schedules that periodically trigger runs, such as a cron scheduler.
        /// </summary>
        PERIODIC,

        /// <summary>
        /// These are one time triggers that only fire a single run. This means the user triggered a single run on demand through the UI or the API.
        /// </summary>
        ONE_TIME,

        /// <summary>
        /// This indicates a run that is triggered as a retry of a previously failed run. This occurs when the user requests to re-run the job in case of failures.
        /// </summary>
        RETRY
    }
}
namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// The type of the run.
    /// </summary>
    public enum RunType
    {
        /// <summary>
        /// Normal job run. A run created with Run now.
        /// </summary>
        JOB_RUN,

        /// <summary>
        /// Workflow run. A run created with dbutils.notebook.run.
        /// </summary>
        WORKFLOW_RUN,

        /// <summary>
        /// Submit run. A run created with Run now.
        /// </summary>
        SUBMIT_RUN
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IJobsApi : IDisposable
    {
        /// <summary>
        /// Creates a new job with the provided settings.
        /// </summary>
        Task<long> Create(JobSettings jobSettings, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all jobs.
        /// </summary>
        Task<IEnumerable<Job>> List(CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the job and sends an email to the addresses specified in JobSettings.email_notifications. No action will occur if the job has already been removed. After the job is removed, neither its details or its run history will be visible via the Jobs UI or API. The job is guaranteed to be removed upon completion of this request. However, runs that were active before the receipt of this request may still be active. They will be terminated asynchronously.
        /// </summary>
        Task Delete(long jobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves information about a single job.
        /// </summary>
        Task<Job> Get(long jobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Overwrites the settings of a job with the provided settings.
        /// </summary>
        /// <param name="jobId">
        /// The canonical identifier of the job to reset. This field is required.
        /// </param>
        /// <param name="newSettings">
        /// The new settings of the job. These new settings will replace the old settings entirely.
        /// Changes to the following fields will not be applied to active runs: JobSettings.cluster_spec or JobSettings.task.
        /// Changes to the following fields will be applied to active runs as well as future runs: JobSettings.timeout_second, JobSettings.email_notifications, or JobSettings.retry_policy.This field is required.
        /// </param>
        Task Reset(long jobId, JobSettings newSettings, CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs the job now, and returns the run_id of the triggered run.
        /// </summary>
        Task<RunIdentifier> RunNow(long jobId, RunParameters runParams, CancellationToken cancellationToken = default);

        /// <summary>
        /// Submit a one-time run with the provided settings. This endpoint doesn’t require a Databricks job to be created. You can directly submit your workload. Runs submitted via this endpoint don’t show up in the UI. Once the run is submitted, you can use the jobs/runs/get API to check the run state.
        /// </summary>
        Task<long> RunSubmit(RunOnceSettings settings, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists runs from most recently started to least.
        /// </summary>
        /// <param name="jobId">The job for which to list runs. If omitted, the Jobs service will list runs from all jobs.</param>
        /// <param name="offset">The offset of the first run to return, relative to the most recent run.</param>
        /// <param name="limit">The number of runs to return. This value should be greater than 0 and less than 1000. The default value is 20. If a request specifies a limit of 0, the service will instead use the maximum limit.</param>
        /// <param name="activeOnly">
        /// if true, only active runs will be included in the results; otherwise, lists both active and completed runs.
        /// Note: This field cannot be true when completed_only is true.
        /// </param>
        /// <param name="completedOnly">
        /// if true, only completed runs will be included in the results; otherwise, lists both active and completed runs.
        /// Note: This field cannot be true when active_only is true.
        /// </param>
        /// <param name="runType">The type of runs to return. For a description of run types, see Run.</param>
        Task<RunList> RunsList(long? jobId = null, int offset = 0, int limit = 20, bool activeOnly = false,
            bool completedOnly = false, /*RunType? runType = null, */ CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the metadata of a run.
        /// </summary>
        /// <param name="runId">The canonical identifier of the run for which to retrieve the metadata. This field is required.</param>
        Task<Run> RunsGet(long runId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels a run. The run is canceled asynchronously, so when this request completes, the run may still be running. The run will be terminated shortly. If the run is already in a terminal life_cycle_state, this method is a no-op.
        /// </summary>
        /// <param name="runId">The canonical identifier of the run for which to cancel. This field is required.</param>
        Task RunsCancel(long runId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a non-active run. Returns an error if the run is active.
        /// </summary>
        /// <param name="runId">The canonical identifier of the run for which to delete. This field is required.</param>
        Task RunsDelete(long runId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves the export of a job run task.
        /// </summary>
        /// <remarks>
        /// Only notebook runs can be exported in HTML format at the moment. Exporting other runs of other types will fail.
        /// </remarks>
        /// <param name="runId">The canonical identifier for the run. This field is required.</param>
        /// <param name="viewsToExport">Which views to export (CODE, DASHBOARDS, or ALL). Defaults to CODE.</param>
        Task<IEnumerable<ViewItem>> RunsExport(long runId, ViewsToExport viewsToExport = ViewsToExport.CODE, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the output of a run. When a notebook task returns value through the Notebook Workflow Exit call, you can use this endpoint to retrieve that value. Note that Databricks restricts this API to return the first 5 MB of the output. For returning a larger result, you can store job results in a cloud storage service.
        /// Runs are automatically removed after 60 days.If you to want to reference them beyond 60 days, you should save old run results before they expire.To export using the UI, see Export job run results.To export using the Job API, see Runs Export.
        /// </summary>
        /// <param name="runId">The canonical identifier for the run. This field is required.</param>
        Task<(string, string, Run)> RunsGetOutput(long runId, CancellationToken cancellationToken = default);
    }
}
using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public interface IJobsApi : IDisposable
{
    /// <summary>
    /// Creates a new job with the provided settings.
    /// </summary>
    Task<long> Create(JobSettings jobSettings, IEnumerable<AccessControlRequest> accessControlList = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all jobs.
    /// </summary>
    /// <param name="limit">The number of jobs to return. This value must be greater than 0 and less or equal to 25. The default value is 20.</param>
    /// <param name="offset">The offset of the first job to return, relative to the most recently created job.</param>
    /// <param name="expandTasks">Whether to include task and cluster details in the response.</param>
    Task<JobList> List(int limit = 20, int offset = 0, bool expandTasks = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the job and sends an email to the addresses specified in JobSettings.email_notifications. No action will occur if the job has already been removed. After the job is removed, neither its details or its run history will be visible via the Jobs UI or API. The job is guaranteed to be removed upon completion of this request. However, runs that were active before the receipt of this request may still be active. They will be terminated asynchronously.
    /// </summary>
    Task Delete(long jobId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves information about a single job.
    /// </summary>
    Task<Job> Get(long jobId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Overwrites all settings for a job.
    /// </summary>
    /// <param name="jobId">
    /// The canonical identifier of the job to reset. This field is required.
    /// </param>
    /// <param name="newSettings">
    /// The new settings of the job. These settings completely replace the old settings.
    /// Changes to the field `JobSettings.timeout_seconds` are applied to active runs. Changes to other fields are applied to future runs only.
    /// </param>
    Task Reset(long jobId, JobSettings newSettings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add, change, or remove specific settings of an existing job. Use the Reset endpoint to overwrite all job settings.
    /// </summary>
    /// <param name="jobId">The canonical identifier of the job to update. This field is required.</param>
    /// <param name="newSettings">The new settings for the job. Any top-level fields specified in new_settings are completely replaced. Partially updating nested fields is not supported. Changes to the field JobSettings.timeout_seconds are applied to active runs.Changes to other fields are applied to future runs only.</param>
    /// <param name="fieldsToRemove">Remove top-level fields in the job settings. Removing nested fields is not supported. This field is optional.</param>
    Task Update(long jobId, JobSettings newSettings, string[] fieldsToRemove = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs the job now, and returns the run_id of the triggered run.
    /// </summary>
    Task<long> RunNow(long jobId, RunParameters runParams = default, string idempotencyToken = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Submit a one-time run. This endpoint allows you to submit a workload directly without creating a job. Runs submitted using this endpoint don't display in the UI. Use the `jobs/runs/get` API to check the run state after the job is submitted.
    /// </summary>
    Task<long> RunSubmit(RunSubmitSettings settings, IEnumerable<AccessControlRequest> accessControlList = default,
        string idempotencyToken = default,
        CancellationToken cancellationToken = default);

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
    /// <param name="expandTasks">Whether to include task and cluster details in the response.</param>
    /// <param name="startTimeFrom">
    /// Show runs that started _at or after_ this value. Can be combined with _start_time_to_
    /// to filter by a time range.
    /// </param>
    /// <param name="startTimeTo">
    /// Show runs that started _at or before_ this value. The value must be
    /// a UTC timestamp in milliseconds. Can be combined with
    /// _start_time_from_ to filter by a time range.
    /// </param>
    Task<RunList> RunsList(long? jobId = default, int offset = 0, int limit = 25, bool activeOnly = default,
        bool completedOnly = default,
        RunType? runType = default, bool expandTasks = default, DateTimeOffset? startTimeFrom = default,
        DateTimeOffset? startTimeTo = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the metadata of a run.
    /// </summary>
    /// <param name="runId">The canonical identifier of the run for which to retrieve the metadata. This field is required.</param>
    /// <param name="includeHistory">Whether to include the repair history in the response.</param>
    Task<(Run, RepairHistory)> RunsGet(long runId, bool includeHistory = default,
        CancellationToken cancellationToken = default);

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
    Task<IEnumerable<ViewItem>> RunsExport(long runId, ViewsToExport viewsToExport = ViewsToExport.CODE,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve the output of a run. When a notebook task returns value through the Notebook Workflow Exit call, you can use this endpoint to retrieve that value. Note that Databricks restricts this API to return the first 5 MB of the output. For returning a larger result, you can store job results in a cloud storage service.
    /// Runs are automatically removed after 60 days.If you to want to reference them beyond 60 days, you should save old run results before they expire.To export using the UI, see Export job run results.To export using the Job API, see Runs Export.
    /// </summary>
    /// <param name="runId">The canonical identifier for the run. This field is required.</param>
    Task<RunOutput> RunsGetOutput(long runId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Re-run one or more tasks. Tasks are re-run as part of the original job
    /// run, use the current job and task settings, and can be viewed in the
    /// history for the original job run.
    /// </summary>
    /// <returns>The ID of the repair.</returns>
    Task<long> RunsRepair(RepairRunInput repairRunInput, RunParameters repairRunParameters, CancellationToken cancellationToken = default);
}
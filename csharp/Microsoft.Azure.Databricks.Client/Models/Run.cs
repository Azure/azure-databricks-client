using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record RunOutput
{
    /// <summary>
    /// The output of a notebook task, if available. A notebook
    /// task that terminates (either successfully or with a
    /// failure) without calling `dbutils.notebook.exit()` is
    /// considered to have an empty output. This field is set but
    /// its result value is empty. Azure Databricks restricts this
    /// API to return the first 5 MB of the output. To return a
    /// larger result, use the
    /// [ClusterLogConf](https://docs.microsoft.com/azure/databricks/dev-tools/api/latest/clusters#clusterlogconf)
    /// field to configure log storage for the job cluster.
    /// </summary>
    [JsonPropertyName("notebook_output")]
    public NotebookOutput NotebookOutput { get; set; }

    /// <summary>
    /// The output from tasks that write to cluster logs such as
    /// [SparkJarTask](https://docs.microsoft.com/azure/databricks/dev-tools/api/latest/jobs#/components/schemas/SparkJarTask)
    /// or
    /// [SparkPythonTask](https://docs.microsoft.com/azure/databricks/dev-tools/api/latest/jobs#/components/schemas/SparkPythonTask.
    /// Azure Databricks restricts this API to return the last 5
    /// MB of these logs. To return a larger result, use the
    /// [ClusterLogConf](https://docs.microsoft.com/azure/databricks/dev-tools/api/latest/clusters#clusterlogconf)
    /// field to configure log storage for the job cluster.
    /// </summary>
    [JsonPropertyName("logs")]
    public string Logs { get; set; }

    /// <summary>
    /// Whether the logs are truncated.
    /// </summary>
    [JsonPropertyName("logs_truncated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool LogsTruncated { get; set; }

    /// <summary>
    /// An error message indicating why a task failed or why
    /// output is not available. The message is unstructured, and
    /// its exact format is subject to change.
    /// </summary>
    /// <example>'ZeroDivisionError: integer division or modulo by zero'</example>
    [JsonPropertyName("error")]
    public string Error { get; set; }

    /// <summary>
    /// If there was an error executing the run, this field
    /// contains any available stack traces.
    /// </summary>
    /// <example>
    /// ---------------------------------------------------------------------------
    /// 
    /// Exception                                 Traceback (most recent call last)
    ///       1 numerator = 42
    ///       2 denominator = 0
    /// ----&gt; 3 return numerator / denominator
    /// 
    /// 
    /// ZeroDivisionError: integer division or modulo by zero
    /// </example>
    [JsonPropertyName("error_trace")]
    public string ErrorTrace { get; set; }

    /// <summary>
    /// All details of the run except for its output.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Run Metadata { get; set; }
}

public record NotebookOutput
{
    /// <summary>
    /// The value passed to
    /// [dbutils.notebook.exit()](https://docs.microsoft.com/azure/databricks/notebooks/notebook-workflows#notebook-workflows-exit).
    /// Azure Databricks restricts this API to return the first 5 MB of the
    /// value. For a larger result, your job can store the results in a
    /// cloud storage service. This field is absent if
    /// `dbutils.notebook.exit()` was never called.
    /// </summary>
    /// <example>An arbitrary string passed by calling dbutils.notebook.exit(...)</example>
    [JsonPropertyName("result")]
    public string Result { get; set; }

    /// <summary>
    /// Whether or not the result was truncated.
    /// </summary>
    [JsonPropertyName("truncated")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool Truncated { get; set; }
}

public record RunTask : BaseTask
{
    /// <summary>
    /// The ID of the task run.
    /// </summary>
    [JsonPropertyName("run_id")]
    public long RunId { get; set; }

    /// <summary>
    /// The result and lifecycle states of the run.
    /// </summary>
    [JsonPropertyName("state")]
    public RunState State { get; set; }

    /// <summary>
    /// The time at which this run was started in epoch milliseconds (milliseconds since 1/1/1970 UTC). Note that this may not be the time when the job task starts executing, for example, if the job is scheduled to run on a new cluster, this is the time the cluster creation call is issued.
    /// </summary>
    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// The time at which this run ended in epoch milliseconds (milliseconds since 1/1/1970 UTC).
    /// </summary>
    [JsonPropertyName("end_time")]
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// The time it took to set up the cluster in milliseconds. For runs that run on new clusters this is the cluster creation time, for runs that run on existing clusters this time should be very short.
    /// </summary>
    [JsonPropertyName("setup_duration")]
    public long SetupDuration { get; set; }

    /// <summary>
    /// The time in milliseconds it took to execute the commands in the jar or notebook until they completed, failed, timed out, were cancelled, or encountered an unexpected error.
    /// </summary>
    [JsonPropertyName("execution_duration")]
    public long ExecutionDuration { get; set; }

    /// <summary>
    /// The time in milliseconds it took to terminate the cluster and clean up any intermediary results, etc. Note that the total duration of the run is the sum of the setup_duration, the execution_duration and the cleanup_duration.
    /// </summary>
    [JsonPropertyName("cleanup_duration")]
    public long CleanupDuration { get; set; }

    /// <summary>
    /// The sequence number of this run attempt for a triggered job run. The
    /// initial attempt of a run has an attempt_number of 0. If the initial
    /// run attempt fails, and the job has a retry policy (`max_retries` &gt;
    /// 0), subsequent runs are created with an `original_attempt_run_id` of
    /// the original attempt's ID and an incrementing `attempt_number`. Runs
    /// are retried only until they succeed, and the maximum
    /// `attempt_number` is the same as the `max_retries` value for the job.
    /// </summary>
    [JsonPropertyName("attempt_number")]
    public int AttemptNumber { get; set; }

    /// <summary>
    /// The cluster used for this run. If the run is specified to use a new cluster, this field will be set once the Jobs service has requested a cluster for the run.
    /// </summary>
    [JsonPropertyName("cluster_instance")]
    public ClusterInstance ClusterInstance { get; set; }

    /// <summary>
    /// An optional specification for a remote repository containing the notebooks used by this job's notebook tasks.
    /// </summary>
    [JsonPropertyName("git_source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public GitSource GitSource { get; set; }
}

/// <summary>
/// All the information about a run except for its output. The output can be retrieved separately with the getRunOutput method.
/// </summary>
public record Run : RunIdentifier
{
    /// <summary>
    /// The creator user name. This field won’t be included in the response if the user has already been deleted.
    /// </summary>
    [JsonPropertyName("creator_user_name")]
    public string CreatorUserName { get; set; }

    ///<summary>
    /// The canonical identifier of the job that contains this run.
    /// </summary>
    [JsonPropertyName("job_id")]
    public long JobId { get; set; }

    /// <summary>
    /// If this run is a retry of a prior run attempt, this field contains the run_id of the original attempt; otherwise, it is the same as the run_id.
    /// </summary>
    [JsonPropertyName("original_attempt_run_id")]
    public long OriginalAttemptRunId { get; set; }

    /// <summary>
    /// The result and lifecycle states of the run.
    /// </summary>
    [JsonPropertyName("state")]
    public RunState State { get; set; }

    /// <summary>
    /// The cron schedule that triggered this run if it was triggered by the periodic scheduler.
    /// </summary>
    [JsonPropertyName("schedule")]
    public CronSchedule Schedule { get; set; }

    /// <summary>
    /// The list of tasks performed by the run. Each task has its own
    /// `run_id` which you can use to call `JobsGetOutput` to retrieve the
    /// run results.
    /// </summary>
    [JsonPropertyName("tasks")]
    public IEnumerable<RunTask> Tasks { get; set; }

    /// <summary>
    /// A list of job cluster specifications that can be shared and reused
    /// by tasks of this job. Libraries cannot be declared in a shared job
    /// cluster. You must declare dependent libraries in task settings.
    /// </summary>
    [JsonPropertyName("job_clusters")]
    public IEnumerable<JobCluster> JobClusters { get; set; }

    /// <summary>
    /// A snapshot of the job’s cluster specification when this run was created.
    /// </summary>
    [JsonPropertyName("cluster_spec")]
    public ClusterSpec ClusterSpec { get; set; }

    /// <summary>
    /// The cluster used for this run. If the run is specified to use a new cluster, this field will be set once the Jobs service has requested a cluster for the run.
    /// </summary>
    [JsonPropertyName("cluster_instance")]
    public ClusterInstance ClusterInstance { get; set; }

    /// <summary>
    /// An optional specification for a remote repository containing the notebooks used by this job's notebook tasks.
    /// </summary>
    [JsonPropertyName("git_source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public GitSource GitSource { get; set; }    

    /// <summary>
    /// The parameters used for this run.
    /// </summary>
    [JsonPropertyName("overriding_parameters")]
    public RunParameters OverridingParameters { get; set; }

    /// <summary>
    /// The time at which this run was started in epoch milliseconds (milliseconds since 1/1/1970 UTC). Note that this may not be the time when the job task starts executing, for example, if the job is scheduled to run on a new cluster, this is the time the cluster creation call is issued.
    /// </summary>
    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    /// <summary>
    /// The time at which this run ended in epoch milliseconds (milliseconds since 1/1/1970 UTC).
    /// </summary>
    [JsonPropertyName("end_time")]
    public DateTimeOffset? EndTime { get; set; }

    /// <summary>
    /// The time it took to set up the cluster in milliseconds. For runs that run on new clusters this is the cluster creation time, for runs that run on existing clusters this time should be very short.
    /// </summary>
    [JsonPropertyName("setup_duration")]
    public long SetupDuration { get; set; }

    /// <summary>
    /// The time in milliseconds it took to execute the commands in the jar or notebook until they completed, failed, timed out, were cancelled, or encountered an unexpected error.
    /// </summary>
    [JsonPropertyName("execution_duration")]
    public long ExecutionDuration { get; set; }

    /// <summary>
    /// The time in milliseconds it took to terminate the cluster and clean up any intermediary results, etc. Note that the total duration of the run is the sum of the setup_duration, the execution_duration and the cleanup_duration.
    /// </summary>
    [JsonPropertyName("cleanup_duration")]
    public long CleanupDuration { get; set; }

    /// <summary>
    /// The type of trigger that fired this run, e.g., a periodic schedule or a one time run.
    /// </summary>
    [JsonPropertyName("trigger")]
    public TriggerType? Trigger { get; set; }

    /// <summary>
    /// An optional name for the run. The maximum allowed length is 4096
    /// bytes in UTF-8 encoding.
    /// </summary>
    [JsonPropertyName("run_name")]
    public string RunName { get; set; }

    /// <summary>
    /// The URL to the detail page of the run.
    /// </summary>
    /// <example>
    /// https://my-workspace.cloud.databricks.com/#job/11223344/run/123
    /// </example>
    [JsonPropertyName("run_page_url")]
    public string RunPageUrl { get; set; }

    [JsonPropertyName("run_type")]
    public RunType RunType { get; set; }

    /// <summary>
    /// The sequence number of this run attempt for a triggered job run. The
    /// initial attempt of a run has an attempt_number of 0\. If the initial
    /// run attempt fails, and the job has a retry policy (`max_retries` \&gt;
    /// 0), subsequent runs are created with an `original_attempt_run_id` of
    /// the original attempt’s ID and an incrementing `attempt_number`. Runs
    /// are retried only until they succeed, and the maximum
    /// `attempt_number` is the same as the `max_retries` value for the job.
    /// </summary>
    [JsonPropertyName("attempt_number")]
    public int AttemptNumber { get; set; }

    /// <summary>
    /// Indication if the run has been completed.
    /// </summary>
    [JsonIgnore]
    public bool IsCompleted => State?.ResultState != null;
}
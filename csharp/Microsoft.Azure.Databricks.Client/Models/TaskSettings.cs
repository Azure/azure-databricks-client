using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record HasTaskKey
{
    /// <summary>
    /// A unique name for the task. This field is used to refer to this task from other tasks. This field is required and must be unique within its parent job.
    /// On Update or Reset, this field is used to reference the tasks to be updated or reset. The maximum length is 100 characters.
    /// </summary>
    [JsonPropertyName("task_key")]
    public string TaskKey { get; set; }
}

public abstract record BaseTask : HasTaskKey
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("depends_on")]
    public IEnumerable<HasTaskKey> DependsOn { get; set; }

    [JsonPropertyName("existing_cluster_id")]
    public string ExistingClusterId { get; set; }

    [JsonPropertyName("new_cluster")] 
    public ClusterAttributes NewCluster { get; set; }

    /// <summary>
    /// If set, indicates that this task must run a notebook. This field may not be specified in conjunction with spark_jar_task.
    /// </summary>
    [JsonPropertyName("notebook_task")]
    public NotebookTask NotebookTask { get; set; }

    /// <summary>
    /// If set, indicates that this task must run a JAR.
    /// </summary>
    [JsonPropertyName("spark_jar_task")]
    public SparkJarTask SparkJarTask { get; set; }

    /// <summary>
    /// If set, indicates that this task must run a Python file.
    /// </summary>
    [JsonPropertyName("spark_python_task")]
    public SparkPythonTask SparkPythonTask { get; set; }

    /// <summary>
    /// If set, indicates that this task must be launched by the spark submit script.
    /// </summary>
    [JsonPropertyName("spark_submit_task")]
    public SparkSubmitTask SparkSubmitTask { get; set; }

    /// <summary>
    /// If set, indicates that this task must execute a Pipeline.
    /// </summary>
    [JsonPropertyName("pipeline_task")]
    public PipelineTask PipelineTask { get; set; }

    /// <summary>
    /// If set, indicates that this job must execute a PythonWheel.
    /// </summary>
    [JsonPropertyName("python_wheel_task")]
    public PythonWheelTask PythonWheelTask { get; set; }

    /// <summary>
    /// An optional list of libraries to be installed on the cluster that executes the task. The default value is an empty list.
    /// </summary>
    [JsonPropertyName("libraries")]
    public List<Library> Libraries { get; set; }

    public BaseTask WithExistingClusterId(string clusterId)
    {
        this.ExistingClusterId = clusterId;
        return this;
    }

    public BaseTask WithNewCluster(ClusterAttributes cluster)
    {
        this.NewCluster = cluster;
        return this;
    }

    public BaseTask AttachLibrary(Library library)
    {
        this.Libraries ??= new List<Library>();
        this.Libraries.Add(library);
        return this;
    }

    public BaseTask WithDescription(string description)
    {
        this.Description = description;
        return this;
    }
}

public abstract record TaskSettings : BaseTask
{
    /// <summary>
    /// An optional timeout applied to each run of this job task. The default behavior is to have no timeout.
    /// </summary>
    [JsonPropertyName("timeout_seconds")]
    public int? TimeoutSeconds { get; set; }
}

public record JobTaskSettings : TaskSettings
{
    [JsonPropertyName("job_cluster_key")]
    public string JobClusterKey { get; set; }

    /// <summary>
    /// An optional set of email addresses that will be notified when runs of this job begin or complete as well as when this job is deleted. The default behavior is to not send any emails.
    /// </summary>
    [JsonPropertyName("email_notifications")]
    public JobEmailNotifications EmailNotifications { get; set; }

    /// <summary>
    /// An optional maximum number of times to retry an unsuccessful run. A run is considered to be unsuccessful if it completes with a FAILED result_state or INTERNAL_ERROR life_cycle_state. The value -1 means to retry indefinitely and the value 0 means to never retry. The default behavior is to never retry.
    /// </summary>
    [JsonPropertyName("max_retries")]
    public int? MaxRetries { get; set; }

    /// <summary>
    /// An optional minimal interval in milliseconds between attempts. The default behavior is that unsuccessful runs are immediately retried.
    /// </summary>
    [JsonPropertyName("min_retry_interval_millis")]
    public int? MinRetryIntervalMilliSeconds { get; set; }

    /// <summary>
    /// An optional policy to specify whether to retry a job when it times out. The default behavior is to not retry on timeout.
    /// </summary>
    [JsonPropertyName("retry_on_timeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool? RetryOnTimeout { get; set; }

    public JobTaskSettings WithRetry(int? maxRetries = default, int? minRetryIntervalMilliSeconds = default,
        bool? retryOnTimeout = default)
    {
        this.MaxRetries = maxRetries;
        this.MinRetryIntervalMilliSeconds = minRetryIntervalMilliSeconds;
        this.RetryOnTimeout = retryOnTimeout;
        return this;
    }

    public JobTaskSettings WithJobClusterKey(string jobClusterKey)
    {
        this.JobClusterKey = jobClusterKey;
        return this;
    }

    public JobTaskSettings WithEmailNotifications(IEnumerable<string> onStart = default,
        IEnumerable<string> onSuccess = default, IEnumerable<string> onFailure = default,
        bool noAlertForSkippedRuns = default)
    {
        this.EmailNotifications = new JobEmailNotifications
        {
            OnStart = onStart,
            OnSuccess = onSuccess,
            OnFailure = onFailure,
            NoAlertForSkippedRuns = noAlertForSkippedRuns
        };

        return this;
    }
}

public record RunSubmitTaskSettings : TaskSettings;

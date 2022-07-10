using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public abstract record JobRunBaseSettings<TTaskSetting> : IJsonOnDeserialized
    where TTaskSetting : TaskSettings, new()
{
    /// <summary>
    /// A list of task specifications to be executed by this job.
    /// </summary>
    [JsonPropertyName("tasks")]
    public List<TTaskSetting> Tasks { get; set; } = new();

    public void OnDeserialized()
    {
        var taskMap = this.Tasks.ToDictionary(
            task => task.TaskKey,
            task => task
        );

        foreach (var task in this.Tasks.Where(task => task.DependsOn != null))
        {
            task.DependsOn =
                from dep in task.DependsOn.GetOrElse(Enumerable.Empty<HasTaskKey>)
                select taskMap[dep.TaskKey];
        }
    }

    /// <summary>
    /// An optional timeout applied to each run of this job. The default behavior is to have no timeout.
    /// </summary>
    [JsonPropertyName("timeout_seconds")]
    public int TimeoutSeconds { get; set; }

    /// <summary>
    /// An optional specification for a remote repository containing the notebooks used by this job's notebook tasks.
    /// </summary>
    [JsonPropertyName("git_source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public GitSource GitSource { get; set; }

    public TTaskSetting AddTask(string taskKey, SparkJarTask task,
        IEnumerable<HasTaskKey> dependsOn = default, int? timeoutSeconds = default)
    {
        var taskSetting = new TTaskSetting {
            TaskKey = taskKey,
            SparkJarTask = task,
            DependsOn = dependsOn,
            TimeoutSeconds = timeoutSeconds
        };

        this.Tasks.Add(taskSetting);
        return taskSetting;
    }

    public TTaskSetting AddTask(string taskKey, SparkPythonTask task,
        IEnumerable<HasTaskKey> dependsOn = default, int? timeoutSeconds = default)
    {
        var taskSetting = new TTaskSetting { 
            TaskKey = taskKey, 
            SparkPythonTask = task,
            DependsOn = dependsOn,
            TimeoutSeconds = timeoutSeconds
        };

        this.Tasks.Add(taskSetting);
        return taskSetting;
    }

    public TTaskSetting AddTask(string taskKey, NotebookTask task,
        IEnumerable<HasTaskKey> dependsOn = default, int? timeoutSeconds = default)
    {
        var taskSetting = new TTaskSetting { 
            TaskKey = taskKey, 
            NotebookTask = task,
            DependsOn = dependsOn,
            TimeoutSeconds = timeoutSeconds
        };
        this.Tasks.Add(taskSetting);
        return taskSetting;
    }
    public TTaskSetting AddTask(string taskKey, SparkSubmitTask task,
        IEnumerable<HasTaskKey> dependsOn = default, int? timeoutSeconds = default)
    {
        var taskSetting = new TTaskSetting {
            TaskKey = taskKey,
            SparkSubmitTask = task,
            DependsOn = dependsOn,
            TimeoutSeconds = timeoutSeconds
        };

        this.Tasks.Add(taskSetting);
        return taskSetting;
    }

    public TTaskSetting AddTask(string taskKey, PipelineTask task,
        IEnumerable<HasTaskKey> dependsOn = default, int? timeoutSeconds = default)
    {
        var taskSetting = new TTaskSetting {
            TaskKey = taskKey,
            PipelineTask = task,
            DependsOn = dependsOn,
            TimeoutSeconds = timeoutSeconds
        };
        this.Tasks.Add(taskSetting);
        return taskSetting;
    }

    public TTaskSetting AddTask(string taskKey, PythonWheelTask task,
        IEnumerable<HasTaskKey> dependsOn = default, int? timeoutSeconds = default)
    {
        var taskSetting = new TTaskSetting
        {
            TaskKey = taskKey,
            PythonWheelTask = task,
            DependsOn = dependsOn,
            TimeoutSeconds = timeoutSeconds
        };

        this.Tasks.Add(taskSetting);
        return taskSetting;
    }
}

public record JobCluster
{
    /// <summary>
    /// A unique name for the job cluster. This field is required and must be unique within the job.
    /// `JobTaskSettings` may refer to this field to determine which cluster to launch for the task execution.
    /// </summary>
    [JsonPropertyName("job_cluster_key")]
    public string JobClusterKey { get; set; }

    [JsonPropertyName("new_cluster")]
    public ClusterAttributes NewCluster { get; set; }
}

public enum JobFormat
{
    SINGLE_TASK,
    MULTI_TASK
}

/// <summary>
/// Settings for a job. These settings can be updated using the resetJob method.
/// </summary>
public record JobSettings: JobRunBaseSettings<JobTaskSettings>
{
    /// <summary>
    /// Adds a cron schedule to a job
    /// </summary>
    /// <param name="cronSchedule"></param>
    /// <returns></returns>
    public JobSettings WithSchedule(CronSchedule cronSchedule)
    {
        Schedule = cronSchedule;
        return this;
    }

    /// <summary>
    /// An optional name for the job. The default value is Untitled.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// A map of tags associated with the job. These are forwarded to the cluster as cluster tags for jobs clusters, and are subject to the same limitations as cluster tags. A maximum of 25 tags can be added to the job.
    /// </summary>
    [JsonPropertyName("tags")]
    public Dictionary<string, string> Tags { get; set; }

    /// <summary>
    /// A list of job cluster specifications that can be shared and reused by tasks of this job. Libraries cannot be declared in a shared job cluster. You must declare dependent libraries in task settings.
    /// </summary>
    [JsonPropertyName("job_clusters")]
    public IEnumerable<JobCluster> JobClusters { get; set; }

    /// <summary>
    /// An optional set of email addresses that will be notified when runs of this job begin or complete as well as when this job is deleted. The default behavior is to not send any emails.
    /// </summary>
    [JsonPropertyName("email_notifications")]
    public JobEmailNotifications EmailNotifications { get; set; }

    /// <summary>
    /// An optional periodic schedule for this job. The default behavior is that the job only runs when triggered by clicking "Run Now" in the Jobs UI or sending an API request to `runNow`.
    /// </summary>
    [JsonPropertyName("schedule")]
    public CronSchedule Schedule { get; set; }

    /// <summary>
    /// An optional maximum allowed number of concurrent runs of the job.
    /// Set this value if you want to be able to execute multiple runs of the same job concurrently. This is useful for example if you trigger your job on a frequent schedule and want to allow consecutive runs to overlap with each other, or if you want to trigger multiple runs which differ by their input parameters.
    /// This setting affects only new runs. For example, suppose the job’s concurrency is 4 and there are 4 concurrent active runs. Then setting the concurrency to 3 won’t kill any of the active runs. However, from then on, new runs will be skipped unless there are fewer than 3 active runs.
    /// This value cannot exceed 1000. Setting this value to 0 will cause all new runs to be skipped. The default behavior is to allow only 1 concurrent run.
    /// </summary>
    [JsonPropertyName("max_concurrent_runs")]
    public int? MaxConcurrentRuns { get; set; }

    /// <summary>
    /// Used to tell what is the format of the job. This field is ignored in Create/Update/Reset calls. When using the Jobs API 2.1 this value is always set to `"MULTI_TASK"`.
    /// </summary>
    [JsonPropertyName("format")]
    public JobFormat Format { get; set; }
}

/// <summary>
/// Settings for a one-time run.
/// </summary>
public record RunSubmitSettings : JobRunBaseSettings<RunSubmitTaskSettings>
{
    /// <summary>
    /// An optional name for the run. The default value is Untitled.
    /// </summary>
    [JsonPropertyName("run_name")]
    public string RunName { get; set; }
}
using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public class Job
{
    /// <summary>
    /// The canonical identifier for this job.
    /// </summary>
    [JsonPropertyName("job_id")]
    public long JobId { get; set; }

    /// <summary>
    /// The creator user name. This field won't be included in the response if the user has already been deleted.
    /// </summary>
    /// <example>user.name@databricks.com</example>
    [JsonPropertyName("creator_user_name")]
    public string CreatorUserName { get; set; }

    /// <summary>
    /// The user name that the job runs as. `run_as_user_name` is
    /// based on the current job settings, and is set to the
    /// creator of the job if job access control is disabled, or
    /// the `is_owner` permission if job access control is
    /// enabled.
    /// </summary>
    /// <example>user.name@databricks.com</example>
    [JsonPropertyName("run_as_user_name")]
    public string RunAsUserName { get; set; }

    /// <summary>
    /// Settings for this job and all of its runs. These settings can be updated using the
    /// [Reset](https://docs.microsoft.com/azure/databricks/dev-tools/api/latest/jobs#operation/JobsReset)
    /// or
    /// [Update](https://docs.microsoft.com/azure/databricks/dev-tools/api/latest/jobs#operation/JobsUpdate)
    /// endpoints.
    /// </summary>
    [JsonPropertyName("settings")]
    public JobSettings Settings { get; set; }

    /// <summary>
    /// The time at which this job was created.
    /// </summary>
    [JsonPropertyName("created_time")]
    public DateTimeOffset? CreatedTime { get; set; }
}
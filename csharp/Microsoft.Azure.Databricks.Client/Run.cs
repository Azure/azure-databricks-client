using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// All the information about a run except for its output. The output can be retrieved separately with the getRunOutput method.
    /// </summary>
    public class Run : RunIdentifier
    {
        /// <summary>
        /// The creator user name. This field won’t be included in the response if the user has already been deleted.
        /// </summary>
        [JsonProperty(PropertyName = "creator_user_name")]
        public string CreatorUserName { get; set; }

        ///<summary>
        /// The canonical identifier of the job that contains this run.
        /// </summary>
        [JsonProperty(PropertyName = "job_id")]
        public long JobId { get; set; }

        /// <summary>
        /// If this run is a retry of a prior run attempt, this field contains the run_id of the original attempt; otherwise, it is the same as the run_id.
        /// </summary>
        [JsonProperty(PropertyName = "original_attempt_run_id")]
        public string OriginalAttemptRunId { get; set; }

        /// <summary>
        /// The result and lifecycle states of the run.
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public RunState State { get; set; }

        /// <summary>
        /// The cron schedule that triggered this run if it was triggered by the periodic scheduler.
        /// </summary>
        [JsonProperty(PropertyName = "schedule")]
        public CronSchedule Schedule { get; set; }

        /// <summary>
        /// The task performed by the run, if any.
        /// </summary>
        [JsonProperty(PropertyName = "task")]
        public JobTask Task { get; set; }

        /// <summary>
        /// A snapshot of the job’s cluster specification when this run was created.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_spec")]
        public ClusterSpec ClusterSpec { get; set; }

        /// <summary>
        /// The cluster used for this run. If the run is specified to use a new cluster, this field will be set once the Jobs service has requested a cluster for the run.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_instance")]
        public ClusterInstance ClusterInstance { get; set; }

        /// <summary>
        /// The parameters used for this run.
        /// </summary>
        [JsonProperty(PropertyName = "overriding_parameters")]
        public RunParameters OverridingParameters { get; set; }

        /// <summary>
        /// The time at which this run was started in epoch milliseconds (milliseconds since 1/1/1970 UTC). Note that this may not be the time when the job task starts executing, for example, if the job is scheduled to run on a new cluster, this is the time the cluster creation call is issued.
        /// </summary>
        [JsonProperty(PropertyName = "start_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? StartTime { get; set; }
        
        /// <summary>
        /// The time at which this run ended in epoch milliseconds (milliseconds since 1/1/1970 UTC).
        /// </summary>
        [JsonProperty(PropertyName = "end_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// The time it took to set up the cluster in milliseconds. For runs that run on new clusters this is the cluster creation time, for runs that run on existing clusters this time should be very short.
        /// </summary>
        [JsonProperty(PropertyName = "setup_duration")]
        public long SetupDuration { get; set; }

        /// <summary>
        /// The time in milliseconds it took to execute the commands in the jar or notebook until they completed, failed, timed out, were cancelled, or encountered an unexpected error.
        /// </summary>
        [JsonProperty(PropertyName = "execution_duration")]
        public long ExecutionDuration { get; set; }

        /// <summary>
        /// The time in milliseconds it took to terminate the cluster and clean up any intermediary results, etc. Note that the total duration of the run is the sum of the setup_duration, the execution_duration and the cleanup_duration.
        /// </summary>
        [JsonProperty(PropertyName = "cleanup_duration")]
        public long CleanupDuration { get; set; }

        /// <summary>
        /// The type of trigger that fired this run, e.g., a periodic schedule or a one time run.
        /// </summary>
        [JsonProperty(PropertyName = "trigger")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TriggerType? Trigger { get; set; }

        /// <summary>
        /// The URL to the detail page of the run.
        /// </summary>
        [JsonProperty(PropertyName = "run_page_url")]
        public string RunPageUrl { get; set; }

        [JsonProperty(PropertyName = "run_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunType RunType { get; set; }

        /// <summary>
        /// Indication if the run has been completed.
        /// </summary>
        [JsonIgnore]
        public bool IsCompleted => State?.ResultState != null;
    }
}

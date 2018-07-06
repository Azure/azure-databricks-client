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
        [JsonProperty(PropertyName = "creator_user_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CreatorUserName { get; set; }

        /// <summary>
        /// If this run is a retry of a prior run attempt, this field contains the run_id of the original attempt; otherwise, it is the same as the run_id.
        /// </summary>
        [JsonProperty(PropertyName = "original_attempt_run_id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string OriginalAttemptRunId { get; set; }

        /// <summary>
        /// The result and lifecycle states of the run.
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public RunState State { get; set; }

        /// <summary>
        /// The cron schedule that triggered this run if it was triggered by the periodic scheduler.
        /// </summary>
        [JsonProperty(PropertyName = "schedule", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CronSchedule Schedule { get; set; }

        /// <summary>
        /// The task performed by the run, if any.
        /// </summary>
        [JsonProperty(PropertyName = "task", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public JobTask Task { get; set; }

        /// <summary>
        /// A snapshot of the job’s cluster specification when this run was created.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_spec")]
        public ClusterSpec ClusterSpec { get; set; }

        /// <summary>
        /// The cluster used for this run. If the run is specified to use a new cluster, this field will be set once the Jobs service has requested a cluster for the run.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_instance", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ClusterInstance ClusterInstance { get; set; }

        /// <summary>
        /// The parameters used for this run.
        /// </summary>
        [JsonProperty(PropertyName = "overriding_parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public RunParameters OverridingParameters { get; set; }

        /// <summary>
        /// The time at which this run was started in epoch milliseconds (milliseconds since 1/1/1970 UTC). Note that this may not be the time when the job task starts executing, for example, if the job is scheduled to run on a new cluster, this is the time the cluster creation call is issued.
        /// </summary>
        [JsonProperty(PropertyName = "start_time", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// The time it took to set up the cluster in milliseconds. For runs that run on new clusters this is the cluster creation time, for runs that run on existing clusters this time should be very short.
        /// </summary>
        [JsonProperty(PropertyName = "setup_duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long SetupDuration { get; set; }

        /// <summary>
        /// The time in milliseconds it took to execute the commands in the jar or notebook until they completed, failed, timed out, were cancelled, or encountered an unexpected error.
        /// </summary>
        [JsonProperty(PropertyName = "execution_duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long ExecutionDuration { get; set; }

        /// <summary>
        /// The time in milliseconds it took to terminate the cluster and clean up any intermediary results, etc. Note that the total duration of the run is the sum of the setup_duration, the execution_duration and the cleanup_duration.
        /// </summary>
        [JsonProperty(PropertyName = "cleanup_duration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long CleanupDuration { get; set; }

        /// <summary>
        /// The type of trigger that fired this run, e.g., a periodic schedule or a one time run.
        /// </summary>
        [JsonProperty(PropertyName = "trigger", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public TriggerType? Trigger { get; set; }

        /// <summary>
        /// The URL to the detail page of the run.
        /// </summary>
        [JsonProperty(PropertyName = "run_page_url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RunPageUrl { get; set; }
    }
}

using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class CronSchedule
    {
        /// <summary>
        /// A cron expression using quartz syntax that describes the schedule for a job. See Quartz for details. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "quartz_cron_expression")]
        public string QuartzCronExpression { get; set; }

        /// <summary>
        /// A Java timezone id. The schedule for a job will be resolved with respect to this timezone. See Java TimeZone for details. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "timezone_id")]
        public string TimezoneId { get; set; }

        /// <summary>
        /// Indicate whether this schedule is paused or not.
        /// </summary>
        [DefaultValue(PauseStatus.UNPAUSED)]
        [JsonProperty(PropertyName = "pause_status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PauseStatus PauseStatus { get; set; }
    }

    /// <summary>
    /// The paused status for a cron schedule
    /// </summary>
    public enum PauseStatus
    {
        /// <summary>
        /// Set when the cron schedule is paused
        /// </summary>
        PAUSED,
        
        /// <summary>
        /// Set when the cron schedule is not paused
        /// </summary>
        UNPAUSED
    }
}
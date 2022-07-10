using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record CronSchedule
    {
        /// <summary>
        /// A cron expression using quartz syntax that describes the schedule for a job. See Quartz for details. This field is required.
        /// </summary>
        [JsonPropertyName("quartz_cron_expression")]
        public string QuartzCronExpression { get; set; }

        /// <summary>
        /// A Java timezone id. The schedule for a job will be resolved with respect to this timezone. See Java TimeZone for details. This field is required.
        /// </summary>
        [JsonPropertyName("timezone_id")]
        public string TimezoneId { get; set; }

        /// <summary>
        /// Indicate whether this schedule is paused or not.
        /// </summary>
        [DefaultValue(PauseStatus.UNPAUSED)]
        [JsonPropertyName("pause_status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
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
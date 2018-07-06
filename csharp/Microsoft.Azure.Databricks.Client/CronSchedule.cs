using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class CronSchedule
    {
        /// <summary>
        /// A cron expression using quartz syntax that describes the schedule for a job. See Quartz for details. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "quartz_cron_expression", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string QuartzCronExpression { get; set; }

        /// <summary>
        /// A Java timezone id. The schedule for a job will be resolved with respect to this timezone. See Java TimeZone for details. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "timezone_id")]
        public string TimezoneId { get; set; }
    }
}
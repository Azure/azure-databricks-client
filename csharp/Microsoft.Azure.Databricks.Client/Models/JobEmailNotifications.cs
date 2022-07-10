using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record JobEmailNotifications
    {
        /// <summary>
        /// A list of email addresses to be notified when a run begins. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
        /// </summary>
        [JsonPropertyName("on_start")]
        public IEnumerable<string> OnStart { get; set; }

        /// <summary>
        /// A list of email addresses to be notified when a run successfully completes. A run is considered to have completed successfully if it ends with a TERMINATED life_cycle_state and a SUCCESSFUL result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
        /// </summary>
        [JsonPropertyName("on_success")]
        public IEnumerable<string> OnSuccess { get; set; }

        /// <summary>
        /// A list of email addresses to be notified when a run unsuccessfully completes. A run is considered to have completed unsuccessfully if it ends with an INTERNAL_ERROR life_cycle_state or a SKIPPED, FAILED, or TIMED_OUT result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
        /// </summary>
        [JsonPropertyName("on_failure")]
        public IEnumerable<string> OnFailure { get; set; }

        /// <summary>
        /// If true, do not send email to recipients specified in on_failure if the run is skipped.
        /// </summary>
        [JsonPropertyName("no_alert_for_skipped_runs")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public bool NoAlertForSkippedRuns { get; set; }
    }
}
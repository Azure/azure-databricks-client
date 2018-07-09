using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class JobEmailNotifications
    {
        /// <summary>
        /// A list of email addresses to be notified when a run begins. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
        /// </summary>
        [JsonProperty(PropertyName = "on_start")]
        public IEnumerable<string> OnStart { get; set; }

        /// <summary>
        /// A list of email addresses to be notified when a run successfully completes. A run is considered to have completed successfully if it ends with a TERMINATED life_cycle_state and a SUCCESSFUL result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
        /// </summary>
        [JsonProperty(PropertyName = "on_success")]
        public IEnumerable<string> OnSuccess { get; set; }

        /// <summary>
        /// A list of email addresses to be notified when a run unsuccessfully completes. A run is considered to have completed unsuccessfully if it ends with an INTERNAL_ERROR life_cycle_state or a SKIPPED, FAILED, or TIMED_OUT result_state. If not specified upon job creation or reset, the list will be empty, i.e., no address will be notified.
        /// </summary>
        [JsonProperty(PropertyName = "on_failure")]
        public IEnumerable<string> OnFailure { get; set; }
    }
}
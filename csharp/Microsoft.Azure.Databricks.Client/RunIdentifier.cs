using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunIdentifier
    {
        /// <summary>
        /// The globally unique id of the newly triggered run.
        /// </summary>
        [JsonProperty(PropertyName = "run_id")]
        public long RunId { get; set; }

        /// <summary>
        /// The sequence number of this run among all runs of the job.
        /// </summary>
        [JsonProperty(PropertyName = "number_in_job")]
        public long NumberInJob { get; set; }
    }
}
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunNowSettings : RunParameters
    {
        /// <summary>
        /// The canonical identifier for this job.
        /// </summary>
        [JsonProperty(PropertyName = "job_id")]
        public long JobId { get; set; }
    }
}
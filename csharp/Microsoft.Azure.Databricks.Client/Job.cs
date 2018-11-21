using System;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class Job
    {
        /// <summary>
        /// The canonical identifier for this job.
        /// </summary>
        [JsonProperty(PropertyName = "job_id")]
        public long JobId { get; set; }

        /// <summary>
        /// The creator user name. This field won’t be included in the response if the user has already been deleted.
        /// </summary>
        [JsonProperty(PropertyName = "creator_user_name")]
        public string CreatorUserName { get; set; }

        /// <summary>
        /// Settings for this job and all of its runs. These settings can be updated using the resetJob method.
        /// </summary>
        [JsonProperty(PropertyName = "settings")]
        public JobSettings Settings { get; set; }

        /// <summary>
        /// The time at which this job was created in epoch milliseconds (milliseconds since 1/1/1970 UTC).
        /// </summary>
        [JsonProperty(PropertyName = "created_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset CreatedTime { get; set; }
    }
}

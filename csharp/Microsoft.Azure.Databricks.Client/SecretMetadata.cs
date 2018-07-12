using System;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class SecretMetadata
    {
        /// <summary>
        /// A unique name to identify the secret.
        /// </summary>
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        /// <summary>
        /// The last updated timestamp (in milliseconds) for the secret.
        /// </summary>
        [JsonProperty(PropertyName = "last_updated_timestamp")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset LastUpdatedTimestamp { get; set; }
    }
}
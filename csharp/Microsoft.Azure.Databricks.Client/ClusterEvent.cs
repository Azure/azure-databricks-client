using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClusterEvent
    {
        /// <summary>
        /// Canonical identifier for the cluster. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The timestamp when the event occurred, stored as the number of milliseconds since the unix epoch. Assigned by the Timeline service.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClusterEventType Type { get; set; }

        /// <summary>
        /// This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "details")]
        public EventDetails Details { get; set; }
    }
}
using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record ClusterEvent
    {
        /// <summary>
        /// Canonical identifier for the cluster. This field is required.
        /// </summary>
        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The timestamp when the event occurred, stored as the number of milliseconds since the unix epoch. Assigned by the Timeline service.
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// This field is required.
        /// </summary>
        [JsonPropertyName("type")]
        public ClusterEventType Type { get; set; }

        /// <summary>
        /// This field is required.
        /// </summary>
        [JsonPropertyName("details")]
        public EventDetails Details { get; set; }
    }
}
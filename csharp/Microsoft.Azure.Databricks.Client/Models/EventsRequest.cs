using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record EventsRequest
    {
        /// <summary>
        /// The ID of the cluster to retrieve events about. This field is required.
        /// </summary>
        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The start time in epoch milliseconds. If empty, returns events starting from the beginning of time.
        /// </summary>
        [JsonPropertyName("start_time")]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// The end time in epoch milliseconds. If empty, returns events up to the current time.
        /// </summary>
        [JsonPropertyName("end_time")]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// The order to list events in; either ASC or DESC. Defaults to DESC.
        /// </summary>
        [JsonPropertyName("order")]
        public ListOrder? Order { get; set; }

        /// <summary>
        /// An optional set of event types to filter on. If empty, all event types are returned.
        /// </summary>
        [JsonPropertyName("event_types")]
        public IEnumerable<ClusterEventType> EventTypes { get; set; }

        /// <summary>
        /// The offset in the result set. Defaults to 0 (no offset). When an offset is specified and the results are requested in descending order, the end_time field is required.
        /// </summary>
        [JsonPropertyName("offset")]
        public long? Offset { get; set; }

        /// <summary>
        /// The maximum number of events to include in a page of events. Defaults to 50, and maximum allowed value is 500.
        /// </summary>
        [JsonPropertyName("limit")]
        public long? Limit { get; set; }
    }
}
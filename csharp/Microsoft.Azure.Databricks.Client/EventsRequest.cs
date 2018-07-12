using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class EventsRequest
    {
        /// <summary>
        /// The ID of the cluster to retrieve events about. This field is required.
        /// </summary>
        [JsonProperty(PropertyName= "cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The start time in epoch milliseconds. If empty, returns events starting from the beginning of time.
        /// </summary>
        [JsonProperty(PropertyName = "start_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// The end time in epoch milliseconds. If empty, returns events up to the current time.
        /// </summary>
        [JsonProperty(PropertyName = "end_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// The order to list events in; either ASC or DESC. Defaults to DESC.
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ListOrder? Order { get; set; }

        /// <summary>
        /// An optional set of event types to filter on. If empty, all event types are returned.
        /// </summary>
        [JsonProperty(PropertyName = "event_types", ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<ClusterEventType> EventTypes { get; set; }

        /// <summary>
        /// The offset in the result set. Defaults to 0 (no offset). When an offset is specified and the results are requested in descending order, the end_time field is required.
        /// </summary>
        [JsonProperty(PropertyName = "offset")]
        public long? Offset { get; set; }

        /// <summary>
        /// The maximum number of events to include in a page of events. Defaults to 50, and maximum allowed value is 500.
        /// </summary>
        [JsonProperty(PropertyName = "limit")]
        public long? Limit { get; set; }
    }
}
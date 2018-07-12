using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class EventsResponse
    {
        /// <summary>
        /// This list of matching events.
        /// </summary>
        [JsonProperty(PropertyName = "events")]
        public IEnumerable<ClusterEvent> Events { get; set; }

        /// <summary>
        /// The parameters required to retrieve the next page of events. Omitted if there are no more events to read.
        /// </summary>
        [JsonProperty(PropertyName = "next_page")]
        public EventsRequest NextPage { get; set; }

        /// <summary>
        /// Whether the response has next page of events.
        /// </summary>
        [JsonIgnore]
        public bool HasNextPage => NextPage != null;

        /// <summary>
        /// The total number of events filtered by the start_time, end_time, and event_types.
        /// </summary>
        [JsonProperty(PropertyName = "total_count")]
        public long TotalCount { get; set; }
    }
}
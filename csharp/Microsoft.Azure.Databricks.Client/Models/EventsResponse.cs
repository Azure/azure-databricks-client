using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record EventsResponse
    {
        /// <summary>
        /// This list of matching events.
        /// </summary>
        [JsonPropertyName("events")]
        public IEnumerable<ClusterEvent> Events { get; set; }

        /// <summary>
        /// The parameters required to retrieve the next page of events. Omitted if there are no more events to read.
        /// </summary>
        [JsonPropertyName("next_page")]
        public EventsRequest NextPage { get; set; }

        /// <summary>
        /// Whether the response has next page of events.
        /// </summary>
        [JsonIgnore]
        public bool HasNextPage => NextPage != null;

        /// <summary>
        /// The total number of events filtered by the start_time, end_time, and event_types.
        /// </summary>
        [JsonPropertyName("total_count")]
        public long TotalCount { get; set; }
    }
}
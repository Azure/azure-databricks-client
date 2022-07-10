using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record RunList
    {
        /// <summary>
        /// A list of runs, from most recently started to least.
        /// </summary>
        [JsonPropertyName("runs")]
        public IEnumerable<Run> Runs { get; set; }

        /// <summary>
        /// If true, additional runs matching the provided filter are available for listing.
        /// </summary>
        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }
    }
}
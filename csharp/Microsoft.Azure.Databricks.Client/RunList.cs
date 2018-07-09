using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunList
    {
        /// <summary>
        /// A list of runs, from most recently started to least.
        /// </summary>
        [JsonProperty(PropertyName = "runs")]
        public IEnumerable<Run> Runs { get; set; }

        /// <summary>
        /// If true, additional runs matching the provided filter are available for listing.
        /// </summary>
        [JsonProperty(PropertyName = "has_more")]
        public bool HasMore { get; set; }
    }
}
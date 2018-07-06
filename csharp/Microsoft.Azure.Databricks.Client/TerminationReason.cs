using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Databricks.Client
{
    public class TerminationReason
    {
        /// <summary>
        /// Status code indicating why the cluster was terminated.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TerminationCode Code { get; set; }

        /// <summary>
        /// List of parameters that provide additional information about why the cluster was terminated.
        /// </summary>
        [JsonProperty(PropertyName = "parameters")]
        public JObject Parameters { get; set; }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Microsoft.Azure.Databricks.Client
{
    public class TerminationReason
    {
        [JsonProperty(PropertyName = "code")]
        public string TerminationCodeText { get; set; }

        /// <summary>
        /// Status code indicating why the cluster was terminated.
        /// </summary>
        [JsonIgnore]
        public TerminationCode Code
        {
            get
            {
                if (Enum.TryParse(TerminationCodeText, out TerminationCode code))
                    return code;
                else return TerminationCode.OTHER;
            }
        }

        /// <summary>
        /// List of parameters that provide additional information about why the cluster was terminated.
        /// </summary>
        [JsonProperty(PropertyName = "parameters")]
        public JObject Parameters { get; set; }
    }
}
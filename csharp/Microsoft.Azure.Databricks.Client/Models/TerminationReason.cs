using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record TerminationReason
    {
        [JsonPropertyName("code")]
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
        [JsonPropertyName("parameters")]
        public Dictionary<string, string> Parameters { get; set; }
    }
}
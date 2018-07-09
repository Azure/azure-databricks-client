using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// A data structure that describes the public metadata of an access token.
    /// </summary>
    public class PublicTokenInfo
    {
        /// <summary>
        /// The ID of this token
        /// </summary>
        [JsonProperty(PropertyName = "token_id")]
        public string TokenId { get; set; }

        /// <summary>
        /// Server time (in epoch milliseconds) when the token was created.
        /// </summary>
        [JsonProperty(PropertyName = "creation_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Server time (in epoch milliseconds) when the token will expire, or -1 if not applicable.
        /// </summary>
        [DefaultValue(-1)]
        [JsonProperty(PropertyName = "expiry_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? ExpiryTime { get; set; }

        /// <summary>
        /// Comment the token was created with, if applicable.
        /// </summary>
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
    }
}

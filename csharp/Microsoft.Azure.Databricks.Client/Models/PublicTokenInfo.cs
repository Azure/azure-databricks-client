using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// A data structure that describes the public metadata of an access token.
    /// </summary>
    public record PublicTokenInfo
    {
        /// <summary>
        /// The ID of this token
        /// </summary>
        [JsonPropertyName("token_id")]
        public string TokenId { get; set; }

        /// <summary>
        /// Server time (in epoch milliseconds) when the token was created.
        /// </summary>
        [JsonPropertyName("creation_time")]
        public DateTimeOffset? CreationTime { get; set; }

        /// <summary>
        /// Server time (in epoch milliseconds) when the token will expire, or -1 if not applicable.
        /// </summary>
        [DefaultValue(-1)]
        [JsonPropertyName("expiry_time")]
        public DateTimeOffset? ExpiryTime { get; set; }

        /// <summary>
        /// Comment the token was created with, if applicable.
        /// </summary>
        [JsonPropertyName("comment")]
        public string Comment { get; set; }
    }
}

using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record SecretMetadata
    {
        /// <summary>
        /// A unique name to identify the secret.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// The last updated timestamp (in milliseconds) for the secret.
        /// </summary>
        [JsonPropertyName("last_updated_timestamp")]
        public DateTimeOffset? LastUpdatedTimestamp { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// The log delivery status
    /// </summary>
    public record LogSyncStatus
    {
        /// <summary>
        /// The timestamp of last attempt. If the last attempt fails, last_exception will contain the exception in the last attempt.
        /// </summary>
        [JsonPropertyName("last_attempted")]
        public long LastAttempted { get; set; }

        /// <summary>
        /// The exception thrown in the last attempt, it would be null (omitted in the response) if there is no exception in last attempted.
        /// </summary>
        [JsonPropertyName("last_exception")]
        public string LastException { get; set; }
    }
}
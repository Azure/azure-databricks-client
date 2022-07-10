using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record FileHandle
    {
        /// <summary>
        /// The handle on an open stream. This field is required.
        /// </summary>
        [JsonPropertyName("handle")]
        public long Handle { get; set; }
    }
}
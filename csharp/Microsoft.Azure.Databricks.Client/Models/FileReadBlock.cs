using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record FileReadBlock
    {
        /// <summary>
        /// The number of bytes read (could be less than length if we hit end of file). This refers to number of bytes read in unencoded version (response data is base64-encoded).
        /// </summary>
        [JsonPropertyName("bytes_read")]
        public long BytesRead { get; set; }

        /// <summary>
        /// The base64-encoded contents of the file read.
        /// </summary>
        [JsonPropertyName("data")]
        public byte[] Data { get; set; }
    }
}
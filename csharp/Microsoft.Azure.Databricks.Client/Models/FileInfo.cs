using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record FileInfo
    {
        /// <summary>
        /// The path of the file or directory.
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; }

        /// <summary>
        /// True if the path is a directory.
        /// </summary>
        [JsonPropertyName("is_dir")]
        public bool IsDirectory { get; set; }

        /// <summary>
        /// The length of the file in bytes or zero if the path is a directory.
        /// </summary>
        [JsonPropertyName("file_size")]
        public long FileSize { get; set; }
    }
}

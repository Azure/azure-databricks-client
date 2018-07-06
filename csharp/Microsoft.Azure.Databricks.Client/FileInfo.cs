using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class FileInfo
    {
        /// <summary>
        /// The path of the file or directory.
        /// </summary>
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// True if the path is a directory.
        /// </summary>
        [JsonProperty(PropertyName = "is_dir", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDirectory { get; set; }

        /// <summary>
        /// The length of the file in bytes or zero if the path is a directory.
        /// </summary>
        [JsonProperty(PropertyName = "file_size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long FileSize { get; set; }
    }
}

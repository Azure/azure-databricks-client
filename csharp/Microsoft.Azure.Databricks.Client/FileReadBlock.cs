using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class FileReadBlock
    {
        /// <summary>
        /// The number of bytes read (could be less than length if we hit end of file). This refers to number of bytes read in unencoded version (response data is base64-encoded).
        /// </summary>
        [JsonProperty(PropertyName = "bytes_read")]
        public long BytesRead { get; set; }

        /// <summary>
        /// The base64-encoded contents of the file read.
        /// </summary>
        [JsonProperty(PropertyName = "data", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public byte[] Data { get; set; }
    }
}
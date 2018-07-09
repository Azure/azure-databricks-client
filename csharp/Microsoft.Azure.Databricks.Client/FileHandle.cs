using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class FileHandle
    {
        public FileHandle()
        {
        }

        public FileHandle(long handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// The handle on an open stream. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "handle")]
        public long Handle { get; set; }
    }
}
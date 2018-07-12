using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class DbfsStorageInfo
    {
        /// <summary>
        /// DBFS destination, e.g. dbfs:/my/path
        /// </summary>
        [JsonProperty(PropertyName = "destination")]
        public string Destination { get; set; }
    }
}
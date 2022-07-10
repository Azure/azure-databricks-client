using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record DbfsStorageInfo
    {
        /// <summary>
        /// DBFS destination, e.g. dbfs:/my/path
        /// </summary>
        [JsonPropertyName("destination")]
        public string Destination { get; set; }
    }
}
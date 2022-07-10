using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record InitScriptInfo
    {
        /// <summary>
        /// DBFS location of init script. destination must be provided.
        /// </summary>
        [JsonPropertyName("dbfs")]
        public DbfsStorageInfo Dbfs { get; set; }
    }
}
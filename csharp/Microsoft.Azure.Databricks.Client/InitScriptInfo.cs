using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class InitScriptInfo
    {
        /// <summary>
        /// DBFS location of init script. destination must be provided.
        /// </summary>
        [JsonProperty(PropertyName = "dbfs")]
        public DbfsStorageInfo Dbfs { get; set; }
    }
}
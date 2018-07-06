using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClusterLogConf
    {
        /// <summary>
        /// For dbfs, destination must be provided. For example, { "dbfs" : { "destination" : "dbfs:/home/cluster_log" } }
        /// </summary>
        [JsonProperty(PropertyName = "dbfs")]
        public DbfsStorageInfo Dbfs { get; set; }
    }
}
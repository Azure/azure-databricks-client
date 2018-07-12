using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class NodeInfo
    {
        [JsonProperty(PropertyName = "available_core_quota")]
        public int AvailableCoreQuota { get; set; }
    }
}
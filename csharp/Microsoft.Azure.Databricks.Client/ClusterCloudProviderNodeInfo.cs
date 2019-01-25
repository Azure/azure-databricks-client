using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClusterCloudProviderNodeInfo
    {
        /// <summary>
        /// Available CPU core quota.
        /// </summary>
        [JsonProperty(PropertyName = "available_core_quota")]
        public int AvailableCoreQuota { get; set; }

        /// <summary>
        /// Total CPU core quota.
        /// </summary>
        [JsonProperty(PropertyName = "total_core_quota")]
        public int TotalCoreQuota { get; set; }

        /// <summary>
        /// Status as reported by the cloud provider.
        /// </summary>
        [JsonProperty(PropertyName = "status", ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<ClusterCloudProviderNodeStatus> Status { get; set; }
    }
}
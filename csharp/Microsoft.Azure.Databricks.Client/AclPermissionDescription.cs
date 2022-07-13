using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class AclPermissionDescription
    {
        [JsonProperty("permission_level")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PermissionLevel PermissionLevel { get; set; }

        public string Description { get; set; }
    }
}
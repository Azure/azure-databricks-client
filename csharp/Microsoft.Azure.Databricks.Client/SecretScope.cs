using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An organizational resource for storing secrets. Secret scopes can be different types, and ACLs can be applied to control permissions for all secrets within a scope.
    /// </summary>
    public class SecretScope
    {
        /// <summary>
        /// A unique name to identify the secret scope.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of secret scope backend.
        /// </summary>
        [JsonProperty(PropertyName = "backend_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ScopeBackendType BackendType { get; set; }
    }
}
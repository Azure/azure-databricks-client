using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An organizational resource for storing secrets. Secret scopes can be different types, and ACLs can be applied to control permissions for all secrets within a scope.
    /// </summary>
    public abstract class SecretScope
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
        public abstract ScopeBackendType BackendType { get; set; }
    }

    public class DatabricksSecretScope : SecretScope
    {
        public override ScopeBackendType BackendType
        {
            get => ScopeBackendType.DATABRICKS;
            set { }
        }
    }

    public class AzureKeyVaultSecretScope : SecretScope
    {
        public override ScopeBackendType BackendType
        {
            get => ScopeBackendType.AZURE_KEYVAULT;
            set { }
        }

        [JsonProperty(PropertyName = "keyvault_metadata")]
        public KeyVaultMetadata KeyVaultMetadata { get; set; }

    }

    public class KeyVaultMetadata
    {
        [JsonProperty(PropertyName = "dns_name")]
        public string DnsName { get; set; }

        [JsonProperty(PropertyName = "resource_id")]
        public string ResourceId { get; set; }
    }
}
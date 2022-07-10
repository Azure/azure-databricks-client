using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// An organizational resource for storing secrets. Secret scopes can be different types, and ACLs can be applied to control permissions for all secrets within a scope.
    /// </summary>
    public abstract record SecretScope
    {
        /// <summary>
        /// A unique name to identify the secret scope.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of secret scope backend.
        /// </summary>
        [JsonPropertyName("backend_type")]
        public abstract ScopeBackendType BackendType { get; set; }
    }

    public record DatabricksSecretScope : SecretScope
    {
        public override ScopeBackendType BackendType
        {
            get => ScopeBackendType.DATABRICKS;
            set { }
        }
    }

    public record AzureKeyVaultSecretScope : SecretScope
    {
        public override ScopeBackendType BackendType
        {
            get => ScopeBackendType.AZURE_KEYVAULT;
            set { }
        }

        [JsonPropertyName("keyvault_metadata")]
        public KeyVaultMetadata KeyVaultMetadata { get; set; }

    }

    public record KeyVaultMetadata
    {
        [JsonPropertyName("dns_name")]
        public string DnsName { get; set; }

        [JsonPropertyName("resource_id")]
        public string ResourceId { get; set; }
    }
}
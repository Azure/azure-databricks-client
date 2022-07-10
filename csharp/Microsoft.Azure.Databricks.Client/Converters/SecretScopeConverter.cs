using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters
{
    public class SecretScopeConverter : JsonConverter<SecretScope>
    {
        public override bool HandleNull => true;

        public override SecretScope Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var scope = JsonDocument.ParseValue(ref reader).RootElement;
            var backendType = scope.GetProperty("backend_type");
            return backendType.Deserialize<ScopeBackendType>() switch
            {
                ScopeBackendType.DATABRICKS => scope.Deserialize<DatabricksSecretScope>(),
                ScopeBackendType.AZURE_KEYVAULT => scope.Deserialize<AzureKeyVaultSecretScope>(),
                _ => throw new NotSupportedException("SecretScope backend type not recognized: " + backendType),
            };
        }

        public override void Write(Utf8JsonWriter writer, SecretScope value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
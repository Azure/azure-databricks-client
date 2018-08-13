using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Databricks.Client
{
    public class SecretScopeConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanWrite => false;


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var scope = JObject.Load(reader);

            var backendType = scope["backend_type"].ToObject<ScopeBackendType>();
            switch (backendType)
            {
                case ScopeBackendType.DATABRICKS:
                    return scope.ToObject<DatabricksSecretScope>();
                case ScopeBackendType.AZURE_KEYVAULT:
                    return scope.ToObject<AzureKeyVaultSecretScope>();
                default:
                    throw new NotSupportedException("SecretScope backend type not recognized: " + backendType);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(SecretScope).GetTypeInfo().IsAssignableFrom(objectType);
        }
    }
}
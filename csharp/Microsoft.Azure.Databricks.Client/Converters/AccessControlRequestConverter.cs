using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters
{
    public class AccessControlRequestConverter : JsonConverter<AccessControlRequest>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(AccessControlRequest).IsAssignableFrom(typeToConvert);
        }

        private static readonly JsonSerializerOptions EnumOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            Converters = {new JsonStringEnumConverter()}
        };

        public override bool HandleNull => true;

        public override AccessControlRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var acr = JsonDocument.ParseValue(ref reader).RootElement;

            if (acr.TryGetProperty("user_name", out _))
            {
                return acr.Deserialize<AccessControlRequestForUser>(EnumOptions);
            }

            if (acr.TryGetProperty("group_name", out _))
            {
                return acr.Deserialize<AccessControlRequestForGroup>(EnumOptions);
            }

            if (acr.TryGetProperty("service_principal_name", out _))
            {
                return acr.Deserialize<AccessControlRequestForServicePrincipal>(EnumOptions);
            }

            throw new NotSupportedException("AccessControlRequest not recognized");
        }

        public override void Write(Utf8JsonWriter writer, AccessControlRequest value, JsonSerializerOptions options)
        {
            var node = value switch
            {
                AccessControlRequestForUser user => JsonSerializer.SerializeToNode(user, typeof(AccessControlRequestForUser), EnumOptions),
                AccessControlRequestForGroup group => JsonSerializer.SerializeToNode(group, typeof(AccessControlRequestForGroup), EnumOptions),
                AccessControlRequestForServicePrincipal sp => JsonSerializer.SerializeToNode(sp, typeof(AccessControlRequestForServicePrincipal), EnumOptions),
                _ => throw new NotImplementedException($"JsonConverter not implemented for type {value.GetType()}")
            };

            node!.WriteTo(writer);
        }
    }
}

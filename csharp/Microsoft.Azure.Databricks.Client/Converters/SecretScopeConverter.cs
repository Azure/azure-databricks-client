// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters;

public class SecretScopeConverter : JsonConverter<SecretScope>
{
    public override bool HandleNull => true;

    public override SecretScope Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var scope = JsonNode.Parse(ref reader)!.AsObject();
        return scope["backend_type"]!.Deserialize<ScopeBackendType>(options) switch
        {
            ScopeBackendType.DATABRICKS => scope.Deserialize<DatabricksSecretScope>(options),
            ScopeBackendType.AZURE_KEYVAULT => scope.Deserialize<AzureKeyVaultSecretScope>(options),
            _ => throw new NotSupportedException("SecretScope backend type not recognized: " + scope["backend_type"])
        };
    }

    public override void Write(Utf8JsonWriter writer, SecretScope value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
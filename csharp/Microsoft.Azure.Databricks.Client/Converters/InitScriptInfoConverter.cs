// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client.Converters;

public class InitScriptInfoConverter : JsonConverter<InitScriptInfo>
{
    public override InitScriptInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var info = JsonNode.Parse(ref reader)!.AsObject();
        if (info.TryGetPropertyValue("workspace", out var workspace))
        {
            return new InitScriptInfo
            {
                StorageDestination = workspace.Deserialize<WorkspaceStorageInfo>()
            };
        }

        if (info.TryGetPropertyValue("dbfs", out var dbfs))
        {
            return new InitScriptInfo
            {
                StorageDestination = dbfs.Deserialize<DbfsStorageInfo>()
            };
        }

        if (info.TryGetPropertyValue("abfss", out var abfss))
        {
            return new InitScriptInfo
            {
                StorageDestination = abfss.Deserialize<AbfssStorageInfo>()
            };
        }


        if (info.TryGetPropertyValue("volumes", out var volumes))
        {
            return new InitScriptInfo
            {
                StorageDestination = volumes.Deserialize<VolumesStorageInfo>()
            };
        }

        throw new NotSupportedException($"Storage destination not recognized: {info}");
    }

    public override void Write(Utf8JsonWriter writer, InitScriptInfo value, JsonSerializerOptions options)
    {
        var node = value.StorageDestination switch
        {
            DbfsStorageInfo dbfs => new JsonObject
            {
                ["dbfs"] = JsonSerializer.SerializeToNode(dbfs)
            },
            AbfssStorageInfo abfss => new JsonObject
            {
                ["abfss"] = JsonSerializer.SerializeToNode(abfss)
            },
            WorkspaceStorageInfo workspace => new JsonObject
            {
                ["workspace"] = JsonSerializer.SerializeToNode(workspace)
            },
            VolumesStorageInfo volumes => new JsonObject
            {
                ["volumes"] = JsonSerializer.SerializeToNode(volumes)
            },
            _ => throw new NotImplementedException($"JsonConverter not implemented for type {value.GetType()}")
        };

        node!.WriteTo(writer);
    }
}
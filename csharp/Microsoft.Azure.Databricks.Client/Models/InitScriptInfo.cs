// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Microsoft.Azure.Databricks.Client.Converters;

namespace Microsoft.Azure.Databricks.Client.Models;

[JsonConverter(typeof(InitScriptInfoConverter))]
public record InitScriptInfo
{
    [JsonIgnore]
    public StorageInfo StorageDestination { get; set; }
}

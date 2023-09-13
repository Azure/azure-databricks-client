// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record InitScriptStorageInfo
{
    /// <summary>
    /// Init script destination. Depends on the kind of init script source specified.
    /// See https://docs.databricks.com/api/azure/workspace/clusters/get
    /// </summary>
    [JsonPropertyName("destination")]
    public string Destination { get; set; }
}
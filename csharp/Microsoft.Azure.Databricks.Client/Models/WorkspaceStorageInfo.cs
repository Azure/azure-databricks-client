// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record WorkspaceStorageInfo
{
    /// <summary>
    /// Workspace destination, e.g. /my/path
    /// </summary>
    [JsonPropertyName("destination")]
    public string Destination { get; set; }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public abstract record StorageInfo
{
    /// <summary>
    /// DBFS destination, e.g. dbfs:/my/path
    /// </summary>
    [JsonPropertyName("destination")]
    public string Destination { get; set; }
}

public record DbfsStorageInfo : StorageInfo { }

public record WorkspaceStorageInfo : StorageInfo { }

public record VolumesStorageInfo : StorageInfo { }
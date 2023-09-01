// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The information of the object in workspace. It will be returned by list and get-status.
/// </summary>
public record ObjectInfo
{
    /// <summary>
    /// The type of the object. It could be NOTEBOOK, DIRECTORY or LIBRARY.
    /// </summary>
    [JsonPropertyName("object_type")]
    public ObjectType ObjectType { get; set; }

    /// <summary>
    /// The absolute path of the object.
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>
    /// The language of the object. This value is set only if the object type is NOTEBOOK.
    /// </summary>
    [JsonPropertyName("language")]
    public Language? Language { get; set; }

    /// <summary>
    /// Only applicable to files. The creation UTC timestamp.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Only applicable to files, the last modified UTC timestamp.
    /// </summary>
    [JsonPropertyName("modified_at")]
    public DateTimeOffset? ModifiedAt { get; set; }

    /// <summary>
    /// Unique identifier for a NOTEBOOK or DIRECTORY.
    /// </summary>
    [JsonPropertyName("object_id")]
    public long ObjectId { get; set; }

    /// <summary>
    /// Only applicable to files. The file size in bytes can be returned.
    /// </summary>
    [JsonPropertyName("size")]
    public long? Size { get; set; }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record GroupMembership : PrincipalName
{
    /// <summary>
    /// Name of the parent group to which the new member will be added.
    /// </summary>
    [JsonPropertyName("parent_name")]
    public string ParentName { get; set; }
}
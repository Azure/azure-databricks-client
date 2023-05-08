// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record PolicyFamily
{
    /// <summary>
    /// ID of the policy family
    /// </summary>
    [JsonPropertyName("policy_family_id")]
    public string Id { get; set; }

    /// <summary>
    /// Name of the policy family.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Human-readable description of the purpose of the policy family.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Policy definition JSON document expressed in [Databricks Policy Definition Language](https://docs.microsoft.com/en-us/azure/databricks/administration-guide/clusters/policies#policy-def).
    /// The JSON document must be passed as a string and cannot be embedded in the requests.
    /// </summary>
    [JsonPropertyName("definition")]
    public string Definition { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }
}
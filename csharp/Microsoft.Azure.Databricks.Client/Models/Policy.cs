// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public enum PolicySortColumn
{
    /// <summary>
    /// Sort result list by policy creation type.
    /// </summary>
    POLICY_CREATION_TIME,

    /// <summary>
    /// Sort result list by policy name.
    /// </summary>
    POLICY_NAME
}

public record Policy
{
    /// <summary>
    /// Canonical unique identifier for the cluster policy.
    /// </summary>
    [JsonPropertyName("policy_id")]
    public string PolicyId { get; set; }

    /// <summary>
    /// Cluster policy name. This must be unique. Length must be between 1 and 100 characters.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Policy definition JSON document expressed in Databricks Policy Definition Language. You must pass the JSON document as a string; it cannot be simply embedded in the requests.
    /// </summary>
    [JsonPropertyName("definition")]
    public string Definition { get; set; }

    /// <summary>
    /// Creation time. The timestamp (in millisecond) when this cluster policy was created.
    /// </summary>
    [JsonPropertyName("created_at_timestamp")]
    public DateTimeOffset? CreatedAtTimestamp { get; set; }
}
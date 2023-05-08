// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Reflection.Metadata;
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
    /// ID of the policy family.
    /// </summary>
    [JsonPropertyName("policy_family_id")]
    public string PolicyFamilyId { get; set; }

    /// <summary>
    /// Policy definition JSON document expressed in [Databricks Policy Definition Language](https://docs.microsoft.com/en-us/azure/databricks/administration-guide/clusters/policies#policy-def).
    /// The JSON document must be passed as a string and cannot be embedded in the requests.
    /// You can use this to customize the policy definition inherited from the policy family. Policy rules specified here are merged into the inherited policy definition.
    /// </summary>
    [JsonPropertyName("policy_family_definition_overrides")]
    public string PolicyFamilyDefinitionOverrides { get; set; }

    /// <summary>
    /// If true, policy is a default policy created and managed by Databricks. Default policies cannot be deleted, and their policy families cannot be changed.
    /// </summary>
    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }

    /// <summary>
    /// Creation time. The timestamp (in millisecond) when this cluster policy was created.
    /// </summary>
    [JsonPropertyName("created_at_timestamp")]
    public DateTimeOffset? CreatedAtTimestamp { get; set; }

    /// <summary>
    /// Max number of clusters per user that can be active using this policy. If not present, there is no max limit.
    /// </summary>
    [JsonPropertyName("max_clusters_per_user")]
    public long? MaxClusterPerUser { get; set; }
}
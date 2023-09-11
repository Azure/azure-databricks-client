// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record Repo
{
    /// <summary>
    /// ID of the repo object in the workspace.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// URL of the Git repository to be linked.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// Git provider
    /// </summary>
    [JsonPropertyName("provider")]
    public RepoProvider Provider { get; set; }

    /// <summary>
    /// Desired path for the repo in the workspace. Must be in the format /Repos/{folder}/{repo-name}.
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>
    /// Branch that the local version of the repo is checked out to.
    /// </summary>
    [JsonPropertyName("branch")]
    public string Branch { get; set; }

    /// <summary>
    /// SHA-1 hash representing the commit ID of the current HEAD of the repo.
    /// </summary>
    [JsonPropertyName("head_commit_id")]
    public string HeadCommitId { get; set; }

    [JsonPropertyName("sparse_checkout")]
    public RepoSparseCheckout SparseCheckout { get; set; }
}

public record RepoSparseCheckout
{
    /// <summary>
    /// List of patterns to include for sparse checkout.
    /// </summary>
    [JsonPropertyName("patterns")]
    public List<string> Patterns { get; set; }
}

/// <summary>
/// The available Git providers are gitHub, bitbucketCloud, gitLab, azureDevOpsServices, gitHubEnterprise, bitbucketServer, gitLabEnterpriseEdition and awsCodeCommit.
/// </summary>
public enum RepoProvider
{
    gitHub,
    bitbucketCloud,
    gitLab,
    azureDevOpsServices,
    gitHubEnterprise,
    bitbucketServer,
    gitLabEnterpriseEdition,
    awsCodeCommit
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public interface IReposApi : IDisposable
{
    /// <summary>
    /// Returns repos that the calling user has Manage permissions on. Results are paginated with each page containing twenty repos.
    /// </summary>
    /// <param name="pathPrefix">Filters repos that have paths starting with the given path prefix.</param>
    /// <param name="pageToken">Token used to get the next page of results. If not specified, returns the first page of results as well as a next page token if there are more results.</param>
    Task<(IEnumerable<Repo>, string)> List(string pathPrefix = default, string pageToken = default, CancellationToken cancellationToken = default);


    /// <summary>
    /// Creates a repo in the workspace and links it to the remote Git repo specified. Note that repos created programmatically must be linked to a remote Git repo, unlike repos created in the browser.
    /// </summary>
    /// <param name="url">URL of the Git repository to be linked.</param>
    /// <param name="provider">Git provider</param>
    /// <param name="path">Desired path for the repo in the workspace. Must be in the format /Repos/{folder}/{repo-name}.</param>
    /// <param name="sparseCheckout">If specified, the repo will be created with sparse checkout enabled. You cannot enable/disable sparse checkout after the repo is created.</param>
    Task<Repo> Create(string url, RepoProvider provider, string path, RepoSparseCheckout sparseCheckout = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the repo with the given repo ID.
    /// </summary>
    /// <param name="repoId">The ID for the corresponding repo to access.</param>
    Task<Repo> Get(long repoId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the repo to a different branch or tag, or updates the repo to the latest commit on the same branch.
    /// </summary>
    /// <param name="repoId">The ID for the corresponding repo to access.</param>
    /// <param name="branch">Branch that the local version of the repo is checked out to.</param>
    /// <param name="tag">Tag that the local version of the repo is checked out to. Updating the repo to a tag puts the repo in a detached HEAD state. Before committing new changes, you must update the repo to a branch instead of the detached HEAD.</param>
    /// <param name="sparseCheckout">If specified, update the sparse checkout settings. The update will fail if sparse checkout is not enabled for the repo.</param>
    Task Update(long repoId, string branch = default, string tag = default, RepoSparseCheckout sparseCheckout = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified repo.
    /// </summary>
    /// <param name="repoId">The ID for the corresponding repo to access.</param>
    Task Delete(long repoId, CancellationToken cancellationToken = default);
}

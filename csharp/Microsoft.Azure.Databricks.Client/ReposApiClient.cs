// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public class ReposApiClient : ApiClient, IReposApi
{
    private readonly string _apiBaseUrl;

    public ReposApiClient(HttpClient httpClient) : base(httpClient)
    {
        _apiBaseUrl = $"{ApiVersion}/repos";
    }

    public async Task<Repo> Create(string url, RepoProvider provider, string path, RepoSparseCheckout sparseCheckout = null, CancellationToken cancellationToken = default)
    {
        return await HttpPost<dynamic, Repo>(this.HttpClient, _apiBaseUrl, new { url, provider = (RepoProvider?)provider, path, sparse_checkout = sparseCheckout }, cancellationToken)
                .ConfigureAwait(false);
    }

    public async Task Delete(long repoId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_apiBaseUrl}/{repoId}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Repo> Get(long repoId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_apiBaseUrl}/{repoId}";
        return await HttpGet<Repo>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<(IEnumerable<Repo>, string)> List(string pathPrefix = null, string pageToken = null, CancellationToken cancellationToken = default)
    {
        var requestUri = this._apiBaseUrl + "?";
        if (pathPrefix != null)
        {
            requestUri += $"&path_prefix={pathPrefix}";
        }

        if (pageToken != null)
        {
            requestUri += $"&next_page_token={pageToken}";
        }

        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        response.TryGetPropertyValue("repos", out var reposNode);
        response.TryGetPropertyValue("next_page_token", out var nextPageTokenNode);

        var repos = reposNode?.Deserialize<IEnumerable<Repo>>(Options) ?? Enumerable.Empty<Repo>();
        var nextPageToken = nextPageTokenNode?.GetValue<string>() ?? string.Empty;

        return (repos, nextPageToken);
    }

    public async Task Update(long repoId, string branch = null, string tag = null, RepoSparseCheckout sparseCheckout = null, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_apiBaseUrl}/{repoId}";

        await HttpPatch(this.HttpClient, requestUri, new { branch, tag, sparse_checkout = sparseCheckout }, cancellationToken)
            .ConfigureAwait(false);
    }
}

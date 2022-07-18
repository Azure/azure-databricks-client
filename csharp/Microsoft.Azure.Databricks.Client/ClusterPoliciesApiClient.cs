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

public class ClusterPoliciesApiClient : ApiClient, IClusterPoliciesApi
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClusterPoliciesApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public ClusterPoliciesApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<Policy> Get(string policyId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/policies/clusters/get?policy_id={policyId}";
        return await HttpGet<Policy>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Policy>> List(ListOrder sortOrder = ListOrder.DESC, PolicySortColumn sortBy = PolicySortColumn.POLICY_CREATION_TIME,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/policies/clusters/list?sort_order={sortOrder}&sort_column={sortBy}";

        var policiesList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken)
            .ConfigureAwait(false);

        policiesList.TryGetPropertyValue("policies", out var policiesNode);

        return policiesNode?.Deserialize<IEnumerable<Policy>>(Options) ?? Enumerable.Empty<Policy>();
    }

    public async Task<string> Create(string name, string definition, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/policies/clusters/create";
        var clusterIdentifier =
            await HttpPost<dynamic, JsonObject>(this.HttpClient, requestUri, new { name, definition }, cancellationToken)
                .ConfigureAwait(false);
        return clusterIdentifier["policy_id"]!.GetValue<string>();
    }

    public async Task Edit(string policyId, string name, string definition,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/policies/clusters/edit";
        await HttpPost(this.HttpClient, requestUri, new { policy_id = policyId, name, definition }, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Delete(string policyId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/policies/clusters/delete";
        await HttpPost(this.HttpClient, requestUri, new { policy_id = policyId }, cancellationToken)
            .ConfigureAwait(false);
    }
}
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

public class TokenApiClient : ApiClient, ITokenApi
{
    public TokenApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<(string, PublicTokenInfo)> Create(long? lifetimeSeconds, string comment,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(new { lifetime_seconds = lifetimeSeconds, comment }, Options)!
            .AsObject();

        var result = await HttpPost<JsonObject, JsonObject>(
            this.HttpClient,
            $"{ApiVersion}/token/create",
            request,
            cancellationToken
        ).ConfigureAwait(false);

        return (
            result["token_value"]!.GetValue<string>(),
            result["token_info"]!.AsObject().Deserialize<PublicTokenInfo>(Options)
        );
    }

    public async Task<IEnumerable<PublicTokenInfo>> List(CancellationToken cancellationToken = default)
    {
        var result = await HttpGet<JsonObject>(
            this.HttpClient,
            $"{ApiVersion}/token/list",
            cancellationToken
        ).ConfigureAwait(false);
        return from token in result["token_infos"]!.AsArray()
               select token.Deserialize<PublicTokenInfo>(Options);
    }

    public async Task Revoke(string tokenId, CancellationToken cancellationToken = default)
    {
        var request = new { token_id = tokenId };
        await HttpPost(this.HttpClient, $"{ApiVersion}/token/delete", request, cancellationToken).ConfigureAwait(false);
    }
}
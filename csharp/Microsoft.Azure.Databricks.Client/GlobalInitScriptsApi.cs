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

public class GlobalInitScriptsApi : ApiClient, IGlobalInitScriptsApi
{
    public GlobalInitScriptsApi(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<GlobalInitScript>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/global-init-scripts";
        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken)
            .ConfigureAwait(false);
        return response.TryGetPropertyValue("scripts", out var scriptsNode)
            ? scriptsNode.Deserialize<IEnumerable<GlobalInitScript>>(Options)
            : Enumerable.Empty<GlobalInitScript>();
    }

    public async Task<GlobalInitScript> Get(string scriptId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/global-init-scripts/{scriptId}";

        return await HttpGet<GlobalInitScript>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> Create(string name, string script, bool enabled = false, int? position = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/global-init-scripts";
        var request = new GlobalInitScript { Enabled = enabled, Name = name, Position = position, Script = script };
        var idNode =
            await HttpPost<GlobalInitScript, JsonObject>(this.HttpClient, requestUri, request, cancellationToken)
                .ConfigureAwait(false);
        return idNode["script_id"]!.GetValue<string>();
    }

    public async Task Delete(string scriptId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/global-init-scripts/{scriptId}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task Update(string scriptId, string name = null, string script = null, bool? enabled = default, int? position = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/global-init-scripts/{scriptId}";
        var request = new GlobalInitScript { Enabled = enabled, Name = name, Position = position, Script = script };
        await HttpPatch(this.HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }
}
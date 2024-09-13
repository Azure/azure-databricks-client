using Azure;
using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class RegisteredModelsApiClient : ApiClient, IRegisteredModelsApi
{
    public RegisteredModelsApiClient(HttpClient httpClient) : base(httpClient) { }


    public async Task<(IEnumerable<RegisteredModel>, string)> List(
        string catalog_name = default,
        string schema_name = default,
        int max_results = 0,
        string pageToken = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/models?");

        if (!string.IsNullOrEmpty(catalog_name))
        {
            requestUriSb.Append($"&catalog_name={Uri.EscapeDataString(catalog_name)}");
        }

        if (!string.IsNullOrEmpty(schema_name))
        {
            requestUriSb.Append($"&schema_name={Uri.EscapeDataString(schema_name)}");
        }

        if (max_results > 0)
        {
            requestUriSb.Append($"&max_results={max_results}");
        }

        if (pageToken != null)
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var response = await HttpGet<JsonObject>(HttpClient, requestUriSb.ToString(), cancellationToken).ConfigureAwait(false);
        response.TryGetPropertyValue("registered_models", out var registeredModels);
        response.TryGetPropertyValue("next_page_token", out var nextPageTokenNode);
        var models = registeredModels?.Deserialize<IEnumerable<RegisteredModel>>(Options) ?? Enumerable.Empty<RegisteredModel>();
        var nextPageToken = nextPageTokenNode?.GetValue<string>() ?? string.Empty;
        return (models, nextPageToken);
    }

    public global::Azure.AsyncPageable<RegisteredModel> ListPageable(
        string catalog_name = default,
        string schema_name = default,
        int max_results = 0,
        CancellationToken cancellationToken = default)
    {
        return new AsyncPageable<RegisteredModel>(async (pageToken) =>
        {
            var (models, nextPageToken) = await List(catalog_name, schema_name, max_results, pageToken, cancellationToken).ConfigureAwait(false);
            return (models.ToList(), !string.IsNullOrEmpty(nextPageToken), nextPageToken);
        });
    }

    public async Task<RegisteredModel> Get(string full_name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/models/{full_name}";
        return await HttpGet<RegisteredModel>(HttpClient, requestUri, cancellationToken);
    }

    public async Task<RegisteredModelAlias> SetAlias(
        string full_name,
        string alias,
        int version_num,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/models/{full_name}/aliases/{alias}";
        var request = new { version_num };
        return await HttpPut<dynamic, RegisteredModelAlias>(HttpClient, requestUri, request, cancellationToken);
    }
}

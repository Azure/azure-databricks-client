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


    public async Task<IEnumerable<RegisteredModel>> ListRegisteredModels(
    string catalog_name = null,
    string schema_name = null,
    int max_results = 0,
    CancellationToken cancellationToken = default)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrEmpty(catalog_name))
        {
            queryParameters.Add($"catalog_name={Uri.EscapeDataString(catalog_name)}");
        }
        if (!string.IsNullOrEmpty(schema_name))
        {
            queryParameters.Add($"schema_name={Uri.EscapeDataString(schema_name)}");
        }
        if (max_results > 0)
        {
            queryParameters.Add($"max_results={max_results}");
        }

        var queryString = queryParameters.Count > 0 ? "?" + string.Join("&", queryParameters) : string.Empty;
        var requestUri = $"{BaseUnityCatalogUri}/models{queryString}";


        var registeredModelsList = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        if (registeredModelsList.TryGetPropertyValue("registered_models", out var registeredModels))
        {
            return registeredModels?.Deserialize<IEnumerable<RegisteredModel>>(Options) ?? Enumerable.Empty<RegisteredModel>();
        }

        return Enumerable.Empty<RegisteredModel>();
    }

    public async Task<RegisteredModel> GetRegisteredModel(string full_name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/models/{full_name}";
        return await HttpGet<RegisteredModel>(HttpClient, requestUri, cancellationToken);
    }

    public async Task<RegisteredModelAlias> SetRegisteredModelAlias(
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

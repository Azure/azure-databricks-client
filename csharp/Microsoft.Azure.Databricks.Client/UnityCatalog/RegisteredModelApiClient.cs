using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
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
        string catalog_name = default,
        string schema_name = default,
        int max_results = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/models");

        if (catalog_name != null)
        {
            requestUriSb.Append($"&catalog_name={catalog_name}");
        }
        if (schema_name != null)
        {
            requestUriSb.Append($"&schema_name={schema_name}");
        }
        if (max_results > 0)
        {
            requestUriSb.Append($"&max_results={max_results}");
        }

        var requestUri = requestUriSb.ToString();
        var registeredModelsList = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        registeredModelsList.TryGetPropertyValue("registered_models", out var registeredModels);
        return registeredModels?.Deserialize<IEnumerable<RegisteredModel>>(Options) ?? Enumerable.Empty<RegisteredModel>();
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

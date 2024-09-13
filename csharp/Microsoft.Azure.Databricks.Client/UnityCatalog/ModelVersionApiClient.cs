using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog
{
    public class ModelVersionApiClient : ApiClient, IModelVersionApi
    {
        public ModelVersionApiClient(HttpClient httpClient) : base(httpClient) { }

        public async Task<(IEnumerable<ModelVersion>, string)> List(
              string full_name,
              int max_results = 0,
              string pageToken = default,
              CancellationToken cancellationToken = default)
        {
            var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/models/{full_name}/versions?");
            if (max_results > 0)
            {
                requestUriSb.Append($"&max_results={max_results}");
            }

            if (pageToken != null)
            {
                requestUriSb.Append($"&page_token={pageToken}");
            }

            var requestUri = requestUriSb.ToString();
            var response = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
            response.TryGetPropertyValue("model_versions", out var modelVersions);
            response.TryGetPropertyValue("next_page_token", out var nextPageTokenNode);

            var versions = modelVersions?.Deserialize<IEnumerable<ModelVersion>>(Options) ?? Enumerable.Empty<ModelVersion>();
            var nextPageToken = nextPageTokenNode?.GetValue<string>() ?? string.Empty;

            return (versions, nextPageToken);
        }

        public global::Azure.AsyncPageable<ModelVersion> ListPageable(string full_name, int max_results = 0, CancellationToken cancellationToken = default)
        {
            return new AsyncPageable<ModelVersion>(async (pageToken) =>
            {
                var (versions, nextPageToken) = await List(full_name, max_results, pageToken, cancellationToken).ConfigureAwait(false);
                return (versions.ToList(), !string.IsNullOrEmpty(nextPageToken), nextPageToken);
            });
        }

        public async Task<ModelVersion> Get(string full_name, int version, CancellationToken cancellationToken = default)
        {
            var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/models/{full_name}/versions/{version}");
            var requestUri = requestUriSb.ToString();
            return await HttpGet<ModelVersion>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ModelVersion> GetByAlias(string full_name, string alias, CancellationToken cancellationToken = default)
        {
            var requestUri = $"{BaseUnityCatalogUri}/models/{full_name}/aliases/{alias}";
            return await HttpGet<ModelVersion>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }
    }
}

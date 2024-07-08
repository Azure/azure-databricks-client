using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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


        public async Task<IEnumerable<ModelVersion>> ListModelVersions(
           string full_name,
           int max_results = default,
           CancellationToken cancellationToken = default)
        {
            var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/models/{full_name}/versions");
            if (max_results > 0)
            {
                requestUriSb.Append($"?max_results={max_results}");
            }

            var requestUri = requestUriSb.ToString();
            var modelVersionsJson = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
            modelVersionsJson.TryGetPropertyValue("model_versions", out var modelVersions);
            return modelVersions?.Deserialize<IEnumerable<ModelVersion>>(Options) ?? Enumerable.Empty<ModelVersion>();
        }


        public async Task<ModelVersion> GetModelVersion(
           string full_name,
           int version,
           string name = default,
           string catalog_name = default,
           string schema_name = default,
           string metastore_id = default,
           CancellationToken cancellationToken = default)
        {
            var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/models/{full_name}/versions/{version}");
            if (name != null)
            {
                requestUriSb.Append($"?name={name}");
            }
            if (name != null)
            {
                requestUriSb.Append($"?catalog_name={catalog_name}");
            }
            if (name != null)
            {
                requestUriSb.Append($"?schema_name={schema_name}");
            }
            if (name != null)
            {
                requestUriSb.Append($"?metastore_id={metastore_id}");
            }

            var requestUri = requestUriSb.ToString();
            return await HttpGet<ModelVersion>(HttpClient, requestUri, cancellationToken);
        }

        public async Task<ModelVersion> GetModelVersionByAlias(
            string full_name,
            string alias,
            CancellationToken cancellationToken = default)
        {
            var requestUri = $"{BaseUnityCatalogUri}/models/{full_name}/aliases/{alias}";
            return await HttpGet<ModelVersion>(HttpClient, requestUri, cancellationToken);
            // var response = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
            //return response;
            //return new ModelVersion();

        }
    }
}

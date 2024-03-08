using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class SchemasApiClient : ApiClient, ISchemasApi
{
    public SchemasApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<Schema>> List(
        string catalogName,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas?catalog_name={catalogName}";
        var schemasList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        schemasList.TryGetPropertyValue("schemas", out var schemas);

        return schemas?.Deserialize<IEnumerable<Schema>>(Options) ?? Enumerable.Empty<Schema>();
    }

    public async Task<Schema> Create(
        SchemaAttributes attributes,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas";

        return await HttpPost<SchemaAttributes, Schema>(this.HttpClient, requestUri, attributes, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Schema> Get(string schemaFullName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas/{schemaFullName}";
        return await HttpGet<Schema>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Schema> Update(
        string schemaFullName,
        string name = default,
        string owner = default,
        string comment = default,
        Dictionary<string, string> properties = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas/{schemaFullName}";
        var request = new { name, owner, comment, properties };
        return await HttpPatch<dynamic, Schema>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string schemaFullName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas/{schemaFullName}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

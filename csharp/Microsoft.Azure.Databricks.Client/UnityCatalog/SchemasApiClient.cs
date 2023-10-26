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

    public async Task<IEnumerable<Schema>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas";
        var schemasList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        schemasList.TryGetPropertyValue("schemas", out var schemas);

        return schemas?.Deserialize<IEnumerable<Schema>>(Options) ?? Enumerable.Empty<Schema>();
    }

    public async Task<Schema> Create(
        string name,
        string catalogName,
        string comment = default,
        Dictionary<string, string> properties = default,
        string storageRoot = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas";
        var requestDict = new Dictionary<string, string>()
        {
            { "name", name },
            { "catalog_name", catalogName }
        };

        if (comment != null)
        {
            requestDict["comment"] = comment;
        }

        if (storageRoot != null)
        {
            requestDict["storage_root"] = storageRoot;
        }


        var request = JsonSerializer.SerializeToNode(requestDict, Options).AsObject();
        
        if (properties != null)
        {
            request.Add("properties", JsonSerializer.SerializeToNode(properties, Options));
        }

        return await HttpPost<JsonObject, Schema>
                (this.HttpClient, requestUri, request, cancellationToken)
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

        var requestDict = new Dictionary<string, string>();
        
        if (name != null)
        {
            requestDict["name"] = name;
        }

        if (owner != null)
        {
            requestDict["owner"] = owner;
        }

        if (comment != null)
        {
            requestDict["comment"] = comment;
        }

        var request = JsonSerializer.SerializeToNode(requestDict, Options).AsObject();

        if (properties != null)
        {
            request.Add("properties", JsonSerializer.SerializeToNode(properties, Options));
        }

        return await HttpPatch<JsonObject, Schema>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string schemaFullName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/schemas/{schemaFullName}";

        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

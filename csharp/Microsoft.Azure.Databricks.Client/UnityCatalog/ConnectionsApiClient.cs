using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class ConnectionsApiClient : ApiClient, IConnectionsApi
{
    public ConnectionsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<Connection>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections";
        var connectionsList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        connectionsList.TryGetPropertyValue("connections", out var connections);
        return connections?.Deserialize<IEnumerable<Connection>>(Options) ?? Enumerable.Empty<Connection>();
    }

    public async Task<Connection> Create(
        ConnectionAttributes connectionAttributes,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections";
        return await HttpPost<ConnectionAttributes, Connection>(HttpClient, requestUri, connectionAttributes, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Connection> Get(string name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections/{name}";
        return await HttpGet<Connection>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Connection> Update(
        string connectionName,
        string name,
        Dictionary<string, string> options = default,
        string owner = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections/{connectionName}";
        var request = new { name, options, owner };
        return await HttpPatch<dynamic, Connection>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string connectionName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections/{connectionName}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

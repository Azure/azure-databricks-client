using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        string name,
        ConnectionType connectionType,
        Dictionary<string, string> options,
        bool? readOnly = default,
        string comment = default,
        Dictionary<string, string> properties = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections";

        var request = new Dictionary<string, string>()
        {
            {"name", name },
            {"connection_type", connectionType.ToString() }
        };

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        requestJson.Add("options", JsonSerializer.SerializeToNode(options));

        if (readOnly  != null )
        {
            requestJson.Add("read_only", readOnly);
        }

        if (comment != null )
        {
            requestJson.Add("comment", comment);
        }

        if (properties != null )
        {
            requestJson.Add("properties", JsonSerializer.SerializeToNode(properties));
        }

        return await HttpPost<JsonObject, Connection>(HttpClient, requestUri, requestJson, cancellationToken).ConfigureAwait(false);
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

        var request = new Dictionary<string, string>()
        {
            {"name", name }
        };

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();
        requestJson.Add("options", JsonSerializer.SerializeToNode(options));
        
        if (owner != null)
        {
            requestJson.Add("owner", owner);
        }

        return await HttpPatch<JsonObject, Connection>(HttpClient, requestUri, requestJson, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string connectionName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/connections/{connectionName}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

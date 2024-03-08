using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class SystemSchemasApiClient : ApiClient, ISystemSchemas
{
    public SystemSchemasApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<SystemSchema>> List(string metastoreId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}/systemschemas";
        var systemSchemasList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        systemSchemasList.TryGetPropertyValue("schemas", out var schemas);
        return schemas?.Deserialize<IEnumerable<SystemSchema>>(Options) ?? Enumerable.Empty<SystemSchema>();
    }

    public async Task Enable(string metastoreId, SystemSchemaName schemaName,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}/systemschemas/{schemaName}";
        await HttpPut(this.HttpClient, requestUri, new { }, cancellationToken);
    }

    public async Task Disable(string metastoreId, SystemSchemaName schemaName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}/systemschemas/{schemaName}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken);
    }
}

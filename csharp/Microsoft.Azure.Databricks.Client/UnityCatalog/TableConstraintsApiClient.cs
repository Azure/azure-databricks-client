using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class TableConstraintsApiClient : ApiClient
{
    public TableConstraintsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<ConstraintRecord> Create(TableConstraintAttributes constraintAttributes, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/constraints";
        var request = JsonSerializer.SerializeToNode(constraintAttributes, Options).AsObject();
        return await HttpPost<JsonObject, ConstraintRecord>(this.HttpClient, requestUri, request, cancellationToken);
    }

    public async Task Delete(
        string fullTableName,
        string constraintName, 
        bool cascade = false, 
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/constraints/{fullTableName}?constraint_name={constraintName}&cascade={cascade.ToString().ToLower()}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken);
    }
}

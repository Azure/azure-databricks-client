using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class TableConstraintsApiClient : ApiClient, ITableConstraintsApi
{
    public TableConstraintsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<ConstraintRecord> Create(TableConstraintAttributes constraintAttributes, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/constraints";
        return await HttpPost<TableConstraintAttributes, ConstraintRecord>(this.HttpClient, requestUri, constraintAttributes, cancellationToken);
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

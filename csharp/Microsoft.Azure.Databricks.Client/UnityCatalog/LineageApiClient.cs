using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog.Interfaces;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class LineageApiClient : ApiClient, ILineageApi
{
    public LineageApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<TablesLineage> GetTablesLineage(string fullTableName, bool includeEntityLineage = false, CancellationToken cancellationToken = default)
    {
        var requestUri =
            $"{ApiVersion}/lineage-tracking/table-lineage?table_name={fullTableName}&include_entity_lineage={includeEntityLineage.ToString().ToLower()}";

        return await HttpGet<TablesLineage>(HttpClient, requestUri, cancellationToken);
    }

    public async Task<ColumnsLineage> GetColumnsLineage(
        string fullTableName,
        string columnName,
        CancellationToken cancellationToken = default)
    {
        var requestUri =
            $"{ApiVersion}/lineage-tracking/table-lineage?table_name={fullTableName}&column_name={columnName}";

        return await HttpGet<ColumnsLineage>(HttpClient, requestUri, cancellationToken);
    }
}

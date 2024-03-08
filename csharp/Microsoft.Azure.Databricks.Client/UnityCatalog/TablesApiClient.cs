using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class TablesApiClient : ApiClient, ITablesApi
{
    public TablesApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<(IEnumerable<TableSummary>, string)> ListSummaries(
        string catalogName,
        int maxResults = 10000,
        string schemaNamePattern = default,
        string tableNamePattern = default,
        string pageToken = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder(
            $"{BaseUnityCatalogUri}/table-summaries?catalog_name={catalogName}&max_results={maxResults}");

        if (schemaNamePattern != null)
        {
            requestUriSb.Append($"&schema_name_pattern={schemaNamePattern}");
        }

        if (tableNamePattern != null)
        {
            requestUriSb.Append($"&table_name_pattern={tableNamePattern}");
        }

        if (pageToken != null)
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var requestUri = requestUriSb.ToString();

        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        response.TryGetPropertyValue("tables", out var tablesNode);
        response.TryGetPropertyValue("next_page_token", out var nextPageTokenNode);

        var tables = tablesNode?.Deserialize<IEnumerable<TableSummary>>(Options) ?? Enumerable.Empty<TableSummary>();
        var nextPageToken = nextPageTokenNode?.Deserialize<string>(Options) ?? string.Empty;

        return (tables, nextPageToken);
    }

    public async Task<(IEnumerable<Table>, string)> List(
        string catalogName,
        string schemaName,
        int? maxResults = default,
        string pageToken = default,
        bool? includeDeltaMetadata = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder(
            $"{BaseUnityCatalogUri}/tables?catalog_name={catalogName}&schema_name={schemaName}");

        if (maxResults != null)
        {
            requestUriSb.Append($"&max_results={maxResults}");
        }

        if (pageToken != null)
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        if (includeDeltaMetadata != null)
        {
            requestUriSb.Append($"&include_delta_metadata={includeDeltaMetadata.ToString().ToLower()}");
        }

        var requestUri = requestUriSb.ToString();

        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        response.TryGetPropertyValue("tables", out var tablesNode);
        response.TryGetPropertyValue("next_page_token", out var nextPageTokenNode);

        var tables = tablesNode?.Deserialize<IEnumerable<Table>>(Options) ?? Enumerable.Empty<Table>();
        var nextPageToken = nextPageTokenNode?.Deserialize<string>(Options) ?? string.Empty;

        return (tables, nextPageToken);
    }

    public async Task<Table> Get(
        string fullTableName,
        bool? includeDeltaMetadata = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder($"{BaseUnityCatalogUri}/tables/{fullTableName}");

        if (includeDeltaMetadata != null)
        {
            requestUriSb.Append($"?include_delta_metadata={includeDeltaMetadata.ToString().ToLower()}");
        }

        var requestUri = requestUriSb.ToString();

        return await HttpGet<Table>(this.HttpClient, requestUri, cancellationToken);
    }

    public async Task Delete(string fullTableName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/tables/{fullTableName}";

        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ITablesApi : IDisposable
{

    Task<(IEnumerable<TableSummary>, string)> ListSummaries(
        string catalogName,
        int maxResults = 10000,
        string schemaNamePattern = default,
        string tableNamePattern = default,
        string pageToken = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an array of all tables for the current metastore under the parent catalog and schema.
    /// </summary>
    Task<(IEnumerable<Table>, string)> List(
        string catalogName,
        string schemaName,
        int? maxResults = default,
        string pageToken = default,
        bool? includeDeltaMetadata = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a table from the metastore for a specific catalog and schema.
    /// </summary>
    Task<Table> Get(
        string fullTableName,
        bool? includeDeltaMetadata = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a table from the specified parent catalog and schema.
    /// </summary>
    Task Delete(string fullTableName, CancellationToken cancellationToken = default);
}

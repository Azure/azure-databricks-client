using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog.Interfaces;

public interface ILineageApi : IDisposable
{
    Task<TablesLineage> GetTablesLineage(
        string fullTableName,
        bool includeEntityLineage = false,
        CancellationToken cancellationToken = default);

    Task<ColumnsLineage> GetColumnsLineage(
        string fullTableName,
        string columnName,
        CancellationToken cancellationToken = default);
}

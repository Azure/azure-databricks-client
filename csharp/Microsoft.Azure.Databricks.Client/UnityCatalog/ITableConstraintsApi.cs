using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ITableConstraintsApi : IDisposable
{
    /// <summary>
    /// Creates a new table constraint.
    /// </summary>
    Task<ConstraintRecord> Create(TableConstraintAttributes constraint, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a table constraint.
    /// </summary>
    Task Delete(
        string fullTableName,
        string constraintName,
        bool cascade = false,
        CancellationToken cancellationToken = default);
}

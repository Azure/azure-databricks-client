using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ISystemSchemas
{
    /// <summary>
    /// Gets an array of system schemas for a metastore. The caller must be an account admin or a metastore admin.
    /// </summary>
    Task<IEnumerable<SystemSchema>> List(string metastoreId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enables the system schema and adds it to the system catalog. The caller must be an account admin or a metastore admin.
    /// </summary>
    Task Enable(string metastoreId, SystemSchemaName schemaName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disables the system schema and removes it from the system catalog. The caller must be an account admin or a metastore admin.
    /// </summary>
    Task Disable(string metastoreId, SystemSchemaName schemaName, CancellationToken cancellationToken = default);

}

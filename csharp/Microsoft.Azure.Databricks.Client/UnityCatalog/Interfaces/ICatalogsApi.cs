using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ICatalogsApi : IDisposable
{
    /// <summary>
    /// Gets an array of catalogs in the metastore. If the caller is the metastore admin, all catalogs will be retrieved.
    /// Otherwise, only catalogs owned by the caller (or for which the caller has the USE_CATALOG privilege) will be retrieved. 
    /// There is no guarantee of a specific ordering of the elements in the array.
    /// </summary>
    Task<IEnumerable<Catalog>> List(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new catalog instance in the parent metastore if the caller is a metastore admin or has the CREATE_CATALOG privilege.
    /// </summary>
    Task<Catalog> Create(CatalogAttributes catalog, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the specified catalog in a metastore. The caller must be a metastore admin, 
    /// the owner of the catalog, or a user that has the USE_CATALOG privilege set for their account.
    /// </summary>
    Task<Catalog> Get(string catalogName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the catalog that matches the supplied name. The caller must be either the owner of the catalog, 
    /// or a metastore admin (when changing the owner field of the catalog).
    /// </summary>
    Task<Catalog> Update(
        string catalogName,
        string name = default,
        string owner = default,
        string comment = default,
        Dictionary<string, string> properties = default,
        IsolationMode? isolationMode = IsolationMode.OPEN,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the catalog that matches the supplied name. The caller must be a metastore admin or the owner of the catalog.
    /// </summary>
    Task Delete(string catalogName, bool forceDeletion = false, CancellationToken cancellationToken = default);
}

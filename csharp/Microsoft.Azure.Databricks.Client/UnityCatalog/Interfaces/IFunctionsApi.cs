using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IFunctionsApi : IDisposable
{
    /// <summary>
    /// Gets an array of external locations (ExternalLocationInfo objects) from the metastore. 
    /// The caller must be a metastore admin, the owner of the external location, 
    /// or a user that has some privilege on the external location. 
    /// There is no guarantee of a specific ordering of the elements in the array.
    /// </summary>
    Task<IEnumerable<Function>> List(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new external location entry in the metastore. The caller must be a metastore admin 
    /// or have the CREATE_EXTERNAL_LOCATION privilege on both the metastore and the associated storage credential.
    /// </summary>
    Task<Function> Create(Function newFunction, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an external location from the metastore. The caller must be either a metastore admin, 
    /// the owner of the external location, or a user that has some privilege on the external location.
    /// </summary>
    Task<Function> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an external location in the metastore. The caller must be the owner of the external location, or be a metastore admin.
    /// In the second case, the admin can only update the name of the external location.
    /// </summary>
    Task<Function> Update(string functionName, string owner, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified external location from the metastore. The caller must be the owner of the external location.
    /// </summary>
    Task Delete(string name, CancellationToken cancellationToken = default);
}

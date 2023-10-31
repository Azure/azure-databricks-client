using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ISecurableWorkspaceBindingsApi : IDisposable
{
    /// <summary>
    /// Gets workspace bindings of the securable. The caller must be a metastore admin or an owner of the securable.
    /// </summary>
    Task<IEnumerable<SecurableWorkspaceBinding>> Get(
        string securableType,
        string securableName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates workspace bindings of the securable. The caller must be a metastore admin or an owner of the securable.
    /// </summary>
    Task<IEnumerable<SecurableWorkspaceBinding>> Update(
        string securableType,
        string securableName,
        IEnumerable<SecurableWorkspaceBinding> add,
        IEnumerable<SecurableWorkspaceBinding> remove,
        CancellationToken cancellationToken = default);
}

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IUnityCatalogPermissionsApi : IDisposable
{
    /// <summary>
    /// Gets the permissions for a securable.
    /// </summary>
    Task<IEnumerable<Permission>> Get(
        SecurableType securableType,
        string securableFullName,
        string principal,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the permissions for a securable.
    /// </summary>
    Task<IEnumerable<Permission>> Update(
        SecurableType securableType,
        string securableFullName,
        IEnumerable<PermissionsUpdate> permissionsUpdates,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the effective permissions for a securable.
    /// </summary>
    Task<IEnumerable<EffectivePermission>> GetEffective(
        SecurableType securableType,
        string securableFullName,
        string principal,
        CancellationToken cancellationToken = default);
}

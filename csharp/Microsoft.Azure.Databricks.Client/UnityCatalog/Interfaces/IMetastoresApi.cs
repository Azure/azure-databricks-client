using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IMetastoresApi : IDisposable
{
    /// <summary>
    /// Gets information about a metastore. This summary includes the storage credential,
    /// the cloud vendor, the cloud region, and the global metastore ID.
    /// </summary>
    Task<Metastore> GetSummary(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an array of the available metastores (as MetastoreInfo objects). 
    /// The caller must be an admin to retrieve this info. 
    /// There is no guarantee of a specific ordering of the elements in the array.
    /// </summary>
    Task<IEnumerable<Metastore>> List(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new metastore based on a provided name and storage root path.
    /// </summary>
    Task<Metastore> Create(
        string name,
        string storageRoot,
        string region = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a metastore that matches the supplied ID. The caller must be a metastore admin to retrieve this info.
    /// </summary>
    Task<Metastore> Get(string metastoreId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates information for a specific metastore. The caller must be a metastore admin.
    /// </summary>
    Task<Metastore> Update(
        string metastoreId,
        string newMetastoreName = default,
        string storageRootCredentialId = default,
        DeltaSharingScope? deltaSharingScope = default,
        long? deltaSharingRecipientTokenLifetimeInSeconds = default,
        string deltaSharingOrganizationName = default,
        string owner = default,
        string privilegeModelVersion = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a metastore. The caller must be a metastore admin.
    /// </summary>
    Task Delete(string metastoreId, bool force = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the metastore assignment for the workspace being accessed.
    /// </summary>
    Task<MetastoreAssignment> GetAssignment(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new metastore assignment. If an assignment for the same workspace_id exists, 
    /// it will be overwritten by the new metastore_id and default_catalog_name. The caller must be an account admin.
    /// </summary>
    Task CreateAssignment(
        long workspaceId,
        string metastoreId,
        string defaultCatalogName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a metastore assignment. This operation can be used to update metastore_id or default_catalog_name for a specified Workspace,
    /// if the Workspace is already assigned a metastore. The caller must be an account admin to update metastore_id; 
    /// otherwise, the caller can be a Workspace admin.
    /// </summary>
    Task UpdateAssignment(
        long workspaceId,
        string metastoreId = default,
        string defaultCatalogName = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a metastore assignment. The caller must be an account administrator.
    /// </summary>
    Task DeleteAssignment(
        long workspaceId,
        string metastoreId,
        CancellationToken cancellationToken = default);
}

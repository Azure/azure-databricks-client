using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IStorageCredentialsApi : IDisposable
{
    /// <summary>
    /// Gets an array of storage credentials (as StorageCredentialInfo objects). 
    /// The array is limited to only those storage credentials the caller has permission to access. 
    /// If the caller is a metastore admin, all storage credentials will be retrieved. 
    /// There is no guarantee of a specific ordering of the elements in the array.
    /// </summary>
    Task<IEnumerable<StorageCredential>> List(CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new storage credential.
    /// </summary>
    Task<StorageCredential> Create(
        StorageCredentialAttributes credentialAttributes,
        bool? skipValidation = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a storage credential from the metastore. The caller must be a metastore admin, 
    /// the owner of the storage credential, or have some permission on the storage credential.
    /// </summary>
    Task<StorageCredential> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a storage credential on the metastore.
    /// </summary>
    Task<StorageCredential> Update(
        string storageCredentialName,
        StorageCredentialAttributes credentialAttributes,
        bool? skipValidation = default,
        bool? force = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a storage credential from the metastore.
    /// </summary>
    Task Delete(string name, CancellationToken cancellationToken = default);
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog.Interfaces;

public interface ISharesApi
{
    /// <summary>
    /// Gets an array of data object shares from the metastore.
    /// The caller must be a metastore admin or the owner of the share.
    /// There is no guarantee of a specific ordering of the elements in the array.
    /// <param name="maxResults">
    /// Maximum number of shares to return.
    /// <list type="bullet">
    /// <item>
    /// <description>When set to 0, the page length is set to a server configured value (recommended).</description>
    /// </item>
    /// <item>
    /// <description>When set to a value greater than 0, the page length is the minimum of this value and a server configured value.</description>
    /// </item>
    /// <item>
    /// <description>When set to a value less than 0, an invalid parameter error is returned.</description>
    /// </item>
    /// <item>
    /// <description>Note: The number of returned shares might be less than the specified max_results size, even zero. The only definitive indication that no further shares can be fetched is when the next_page_token is unset from the response.</description>
    /// </item>
    /// </list>
    /// </param>
    /// </summary>
    /// <param name="pageToken">Opaque pagination token to go to next page based on previous query.</param>
    Task<SharesList> List(int maxResults = 0, string pageToken = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an array of data object shares from the metastore.
    /// The caller must be a metastore admin or the owner of the share.
    /// There is no guarantee of a specific ordering of the elements in the array.
    /// <param name="maxResultsPerPage">
    /// Maximum number of shares to return.
    /// <list type="bullet">
    /// <item>
    /// <description>When set to 0, the page length is set to a server configured value (recommended).</description>
    /// </item>
    /// <item>
    /// <description>When set to a value greater than 0, the page length is the minimum of this value and a server configured value.</description>
    /// </item>
    /// <item>
    /// <description>When set to a value less than 0, an invalid parameter error is returned.</description>
    /// </item>
    /// <item>
    /// <description>Note: The number of returned shares might be less than the specified max_results size, even zero. The only definitive indication that no further shares can be fetched is when the next_page_token is unset from the response.</description>
    /// </item>
    /// </list>
    /// </param>
    /// </summary>
    global::Azure.AsyncPageable<Share> ListPageable(int maxResultsPerPage = 0, CancellationToken cancellationToken = default);


    /// <summary>
    /// Creates a new share for data objects.
    /// Data objects can be added after creation with update.
    /// The caller must be a metastore admin or have the CREATE_SHARE privilege on the metastore.
    /// </summary>
    Task<Share> Create(ShareAttributes share, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a data object share from the metastore.
    /// The caller must be a metastore admin or the owner of the share.
    /// </summary>
    Task<Share> Get(string shareName, bool includeSharedData = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the share with the changes and data objects in the request.
    /// The caller must be the owner of the share or a metastore admin.
    ///
    /// When the caller is a metastore admin, only the owner field can be updated.
    ///
    /// In the case that the share name is changed, updateShare requires that the caller is both the share owner and a metastore admin.
    ///
    /// If there are notebook files in the share, the storage_root field cannot be updated.
    ///
    /// For each table that is added through this method, the share owner must also have SELECT privilege on the table.
    /// This privilege must be maintained indefinitely for recipients to be able to access the table.
    /// Typically, you should use a group as the share owner.
    ///
    /// Table removals through update do not require additional privileges.
    /// </summary>
    Task<Share> Update(
        string shareName,
        string newName = null,
        string owner = null,
        string comment = null,
        string storageRoot = null,
        IEnumerable<ShareObjectUpdate> updates = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a data object share from the metastore. The caller must be an owner of the share.
    /// </summary>
    Task Delete(string shareName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the permissions for a data share from the metastore.
    /// The caller must be a metastore admin or the owner of the share.
    /// </summary>
    /// <param name="maxResults">
    /// Maximum number of shares to return.
    /// <list type="bullet">
    /// <item>
    /// <description>When set to 0, the page length is set to a server configured value (recommended).</description>
    /// </item>
    /// <item>
    /// <description>When set to a value greater than 0, the page length is the minimum of this value and a server configured value.</description>
    /// </item>
    /// <item>
    /// <description>When set to a value less than 0, an invalid parameter error is returned.</description>
    /// </item>
    /// <item>
    /// <description>Note: The number of returned shares might be less than the specified max_results size, even zero. The only definitive indication that no further shares can be fetched is when the next_page_token is unset from the response.</description>
    /// </item>
    /// </list>
    /// </param>
    /// <param name="pageToken">Opaque pagination token to go to next page based on previous query.</param>
    Task<PermissionsList> GetPermissions(string shareName, int maxResults = 0, string pageToken = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the permissions for a data share from the metastore with pagination support.
    /// The caller must be a metastore admin or the owner of the share.
    /// </summary>
    /// <param name="shareName">The name of the share.</param>
    /// <param name="maxResultsPerPage">
    /// Maximum number of shares to return.
    /// <list type="bullet">
    /// <item>
    /// <description>When set to 0, the page length is set to a server configured value (recommended).</description>
    /// </item>
    /// <item>
    /// <description>When set to a value greater than 0, the page length is the minimum of this value and a server configured value.</description>
    /// </item>
    /// <item>
    /// <description>When set to a value less than 0, an invalid parameter error is returned.</description>
    /// </item>
    /// <item>
    /// <description>Note: The number of returned shares might be less than the specified max_results size, even zero. The only definitive indication that no further shares can be fetched is when the next_page_token is unset from the response.</description>
    /// </item>
    /// </list>
    /// </param>
    global::Azure.AsyncPageable<Permission> GetPermissionsPageable(string shareName, int maxResultsPerPage = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the permissions for a data share in the metastore.
    /// The caller must be a metastore admin or an owner of the share.
    ///
    /// For new recipient grants, the user must also be the recipient owner or metastore admin.
    /// recipient revocations do not require additional privileges.
    /// </summary>
    Task<IEnumerable<Permission>> UpdatePermissions(
        string shareName,
        IEnumerable<PermissionsUpdate> changes,
        CancellationToken cancellationToken = default);
}

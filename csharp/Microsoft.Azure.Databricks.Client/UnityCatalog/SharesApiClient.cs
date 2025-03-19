using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog.Interfaces;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class SharesApiClient : ApiClient, ISharesApi
{
    public SharesApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<SharesList> List(int maxResults = 0, string pageToken = default, CancellationToken cancellationToken = default)
    {
        if (maxResults < 0 || maxResults > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(maxResults), "maxResults must be between 0 and 1000");
        }

        StringBuilder requestUriSb = new($"{BaseUnityCatalogUri}/shares?max_results={maxResults}");

        if (!string.IsNullOrEmpty(pageToken))
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var requestUri = requestUriSb.ToString();
        return await HttpGet<SharesList>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public global::Azure.AsyncPageable<Share> ListPageable(int maxResultsPerPage = 0, CancellationToken cancellationToken = default)
    {
        return new AsyncPageable<Share>(
            async (string pageToken) =>
            {
                var response = await List(maxResultsPerPage, pageToken, cancellationToken).ConfigureAwait(false);
                return (response.Shares.ToList(), response.HasMore, response.NextPageToken);
            }
        );
    }

    public async Task<Share> Create(ShareAttributes share, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares";

        return await HttpPost<ShareAttributes, Share>(this.HttpClient, requestUri, share, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Share> Get(string shareName, bool includeSharedData = false, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares/{shareName}?include_shared_data={includeSharedData.ToString().ToLower()}";
        return await HttpGet<Share>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Share> Update(string shareName, string newName = default, string owner = default, string comment = default,
        string storageRoot = default, IEnumerable<ShareObjectUpdate> updates = default, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares/{shareName}";
        var request = new { new_name = newName, owner, comment, storage_root = storageRoot, updates };
        return await HttpPatch<dynamic, Share>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string shareName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares/{shareName}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PermissionsList> GetPermissions(string shareName, int maxResults = 0, string pageToken = default, CancellationToken cancellationToken = default)
    {
        if (maxResults < 0 || maxResults > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(maxResults), "maxResults must be between 0 and 1000");
        }

        StringBuilder requestUriSb = new($"{BaseUnityCatalogUri}/shares/{shareName}/permissions?max_results={maxResults}");

        if (!string.IsNullOrEmpty(pageToken))
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var requestUri = requestUriSb.ToString();
        return await HttpGet<PermissionsList>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public global::Azure.AsyncPageable<Permission> GetPermissionsPageable(string shareName, int maxResultsPerPage = 0, CancellationToken cancellationToken = default)
    {
        return new AsyncPageable<Permission>(
            async (string pageToken) =>
            {
                var response = await GetPermissions(shareName, maxResultsPerPage, pageToken, cancellationToken).ConfigureAwait(false);
                return (response.Permissions.ToList(), response.HasMore, response.NextPageToken);
            }
        );
    }

    public async Task<IEnumerable<Permission>> UpdatePermissions(string shareName, IEnumerable<PermissionsUpdate> changes, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares/{shareName}/permissions";
        var request = new { changes };

        var permissionsList = await HttpPatch<dynamic, JsonObject>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
        permissionsList.TryGetPropertyValue("privilege_assignments", out var permissions);

        return permissions.Deserialize<IEnumerable<Permission>>(Options) ?? Enumerable.Empty<Permission>();
    }
}

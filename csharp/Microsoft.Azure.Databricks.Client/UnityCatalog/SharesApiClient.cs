using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

    public async Task<IEnumerable<Share>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares";
        var sharesList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        sharesList.TryGetPropertyValue("shares", out var shares);

        return shares?.Deserialize<IEnumerable<Share>>(Options) ?? Enumerable.Empty<Share>();
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

    public async Task<IEnumerable<Permission>> GetPermissions(string shareName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/shares/{shareName}/permissions";
        var permissionsList = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        permissionsList.TryGetPropertyValue("privilege_assignments", out var permissions);

        return permissions.Deserialize<IEnumerable<Permission>>(Options) ?? Enumerable.Empty<Permission>();
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

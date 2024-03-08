using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class MetastoresApiClient : ApiClient, IMetastoresApi
{
    public MetastoresApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<Metastore> Create(
        string name,
        string storageRoot,
        string region = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores";
        var request = new { name, storage_root = storageRoot, region };
        return await HttpPost<dynamic, Metastore>(HttpClient, requestUri, request, cancellationToken);
    }

    public async Task CreateAssignment(
        long workspaceId,
        string metastoreId,
        string defaultCatalogName,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/workspaces/{workspaceId}/metastore";
        var request = new { metastore_id = metastoreId, default_catalog_name = defaultCatalogName };
        await HttpPut<dynamic>(HttpClient, requestUri, request, cancellationToken);
    }

    public async Task Delete(
        string metastoreId,
        bool force = false,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}?force={force.ToString().ToLower()}";
        await HttpDelete(HttpClient, requestUri, cancellationToken);
    }

    public async Task DeleteAssignment(
        long workspaceId,
        string metastoreId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/workspaces/{workspaceId}/metastore?metastore_id={metastoreId}";
        await HttpDelete(HttpClient, requestUri, cancellationToken);
    }

    public Task<Metastore> Get(string metastoreId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}";
        return HttpGet<Metastore>(HttpClient, requestUri, cancellationToken);
    }

    public Task<MetastoreAssignment> GetAssignment(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/current-metastore-assignment";
        return HttpGet<MetastoreAssignment>(HttpClient, requestUri, cancellationToken);
    }

    public Task<Metastore> GetSummary(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastore_summary";
        return HttpGet<Metastore>(HttpClient, requestUri, cancellationToken);
    }

    public async Task<IEnumerable<Metastore>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores";
        var metastoresList = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        metastoresList.TryGetPropertyValue("metastores", out var metastores);
        return metastores?.Deserialize<IEnumerable<Metastore>>(Options) ?? Enumerable.Empty<Metastore>();
    }

    public async Task<Metastore> Update(
        string metastoreId,
        string newMetastoreName = null,
        string storageRootCredentialId = null,
        DeltaSharingScope? deltaSharingScope = null,
        long? deltaSharingRecipientTokenLifetimeInSeconds = null,
        string deltaSharingOrganizationName = null,
        string owner = null,
        string privilegeModelVersion = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}";

        var request = new
        {
            name = newMetastoreName,
            storage_root_credential_id = storageRootCredentialId,
            delta_sharing_scope = deltaSharingScope,
            delta_sharing_recipient_token_lifetime_in_seconds = deltaSharingRecipientTokenLifetimeInSeconds,
            delta_sharing_organization_name = deltaSharingOrganizationName,
            owner,
            privilege_model_version = privilegeModelVersion
        };

        return await HttpPatch<dynamic, Metastore>(HttpClient, requestUri, request, cancellationToken);
    }

    public async Task UpdateAssignment(
        long workspaceId,
        string metastoreId = null,
        string defaultCatalogName = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/workspaces/{workspaceId}/metastore";

        var request = new
        {
            metastore_id = metastoreId,
            default_catalog_name = defaultCatalogName
        };

        await HttpPatch<dynamic>(HttpClient, requestUri, request, cancellationToken);
    }
}

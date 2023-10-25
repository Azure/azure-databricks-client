using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        var request = new Dictionary<string, string>()
        {
            { "name", name },
            { "storage_root", storageRoot }
        };

        if (region != null )
        {
            request["region"] = region;
        }

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        return await HttpPost<JsonObject, Metastore>(HttpClient, requestUri, requestJson, cancellationToken);
    }

    public async Task CreateAssignment(
        long workspaceId,
        string metastoreId,
        string defaultCatalogName,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/workspaces/{workspaceId}/metastore";
        
        var request = new Dictionary<string, string>()
        {
            { "metastore_id", metastoreId },
            { "default_catalog_name", defaultCatalogName }
        };

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        await HttpPut<JsonObject>(HttpClient, requestUri, requestJson, cancellationToken);
    }

    public async Task Delete(
        string metastoreId,
        bool force = false,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}?force={force.ToString().ToLower()}";
        await HttpDelete(HttpClient, requestUri, cancellationToken);
    }

    public async Task DeleteAsignment(
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
        var metastoresList = await HttpGet<JsonObject>(HttpClient, requestUri , cancellationToken).ConfigureAwait(false);
        metastoresList.TryGetPropertyValue("metastores", out var metastores);

        return metastores?.Deserialize<IEnumerable<Metastore>>(Options) ?? Enumerable.Empty<Metastore>();
    }

    public async Task<Metastore> Update(
        string metastoreId,
        string newMetastoreName = null,
        string storageRootCredentialId = null,
        DeltaSharingScope? deltaSharingScope = null,
        long? deltaSharingRecipentTokenLifetimeInSeconds = null,
        string deltaSharingOrganizationName = null,
        string owner = null,
        string privilegeModelVersion = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/metastores/{metastoreId}";

        var request = new Dictionary<string, string>();

        if (newMetastoreName != null)
        {
            request["name"] = newMetastoreName;
        }

        if (storageRootCredentialId != null)
        {
            request["storage_root_credential_id"] = storageRootCredentialId;
        }

        if (deltaSharingScope != null)
        {
            request["delta_sharing_scope"] = deltaSharingScope.ToString();
        }

        if (deltaSharingRecipentTokenLifetimeInSeconds != null)
        {
            request["delta_sharing_recipient_token_lifetime_in_seconds"] = deltaSharingRecipentTokenLifetimeInSeconds.ToString();
        }

        if (deltaSharingOrganizationName != null)
        {
            request["delta_sharing_organization_name"] = deltaSharingOrganizationName;
        }

        if (owner != null) 
        {
            request["owner"] = owner;
        }

        if (privilegeModelVersion != null)
        {
            request["privilege_model_version"] = privilegeModelVersion;
        }

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        return await HttpPatch<JsonObject, Metastore>(HttpClient, requestUri, requestJson, cancellationToken);
    }

    public async Task UpdateAssignment(
        long workspaceId,
        string metastoreId = null,
        string defaultCatalogName = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/workspaces/{workspaceId}/metastore";

        var request = new Dictionary<string, string>();

        if (metastoreId != null)
        {
            request["metastore_id"] = metastoreId;
        }

        if (defaultCatalogName != null)
        {
            request["default_catalog_name"] = defaultCatalogName;
        }

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        await HttpPatch<JsonObject>(HttpClient, requestUri, requestJson, cancellationToken);
    }
}

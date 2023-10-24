using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class UnityCatalogPermissionsApiClient : ApiClient, IUnityCatalogPermissionsApi
{
    public UnityCatalogPermissionsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

  
    public async Task<PermissionsList> Get(
        SecurableType securableType,
        string securableFullName,
        string principal = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder(
            $"{BaseUnityCatalogUri}/permissions/{securableType.ToString().ToLower()}/{securableFullName}");
        
        if (principal != null)
        {
            requestUriSb.Append($"?principal={principal}");
        }

        var requestUri = requestUriSb.ToString();
        return await HttpGet<PermissionsList>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PermissionsList> Update(
        SecurableType securableType,
        string securableFullName,
        IEnumerable<PermissionsUpdate> permisionsUpdates,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            changes = permisionsUpdates,
        };

        var requestUri = $"{BaseUnityCatalogUri}/permissions/{securableType.ToString().ToLower()}/{securableFullName}";
        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        return await HttpPatch<JsonObject, PermissionsList>(HttpClient, requestUri, requestJson, cancellationToken).ConfigureAwait(false);
    }

    public async Task<EffectivePermissionsList> GetEffective(
        SecurableType securableType,
        string securableFullName,
        string principal = default,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder(
            $"{BaseUnityCatalogUri}/effective-permissions/{securableType.ToString().ToLower()}/{securableFullName}");

        if (principal != null)
        {
            requestUriSb.Append($"?principal={principal}");
        }

        var requestUri = requestUriSb.ToString();
        return await HttpGet<EffectivePermissionsList>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

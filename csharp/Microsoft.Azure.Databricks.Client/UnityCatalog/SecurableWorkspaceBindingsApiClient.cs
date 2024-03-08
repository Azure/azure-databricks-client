using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class SecurableWorkspaceBindingsApiClient : ApiClient, ISecurableWorkspaceBindingsApi
{
    public SecurableWorkspaceBindingsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<SecurableWorkspaceBinding>> Get(
        string securableType,
        string securableName,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/bindings/{securableType}/{securableName}";
        var securityBindingsList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        securityBindingsList.TryGetPropertyValue("bindings", out var securityBindings);
        return securityBindings.Deserialize<IEnumerable<SecurableWorkspaceBinding>>(Options) ?? Enumerable.Empty<SecurableWorkspaceBinding>();
    }

    public async Task<IEnumerable<SecurableWorkspaceBinding>> Update(
        string securableType,
        string securableName,
        IEnumerable<SecurableWorkspaceBinding> add,
        IEnumerable<SecurableWorkspaceBinding> remove,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/bindings/{securableType}/{securableName}";
        var request = new { add, remove };
        var securityBindingsList = await HttpPatch<dynamic, JsonObject>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
        securityBindingsList.TryGetPropertyValue("bindings", out var securityBindings);
        return securityBindings.Deserialize<IEnumerable<SecurableWorkspaceBinding>>(Options) ?? Enumerable.Empty<SecurableWorkspaceBinding>();
    }
}

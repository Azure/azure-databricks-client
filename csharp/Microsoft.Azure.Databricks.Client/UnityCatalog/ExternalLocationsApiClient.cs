using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class ExternalLocationsApiClient : ApiClient, IExternalLocationsApi
{
    private string ExternalLocationsApiUri => $"{BaseUnityCatalogUri}/external-locations";

    public ExternalLocationsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<ExternalLocation>> List(CancellationToken cancellationToken = default)
    {
        var externalLocationsList = await HttpGet<JsonObject>(HttpClient, this.ExternalLocationsApiUri, cancellationToken).ConfigureAwait(false);
        externalLocationsList.TryGetPropertyValue("external_locations", out var externalLocations);
        return externalLocations?.Deserialize<IEnumerable<ExternalLocation>>(Options) ?? Enumerable.Empty<ExternalLocation>();
    }

    public async Task<ExternalLocation> Create(
        ExternalLocationAttributes attributes,
        bool skipValidation = default,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(attributes)!.AsObject();
        request.Add("skip_validation", skipValidation);
        return await HttpPost<JsonObject, ExternalLocation>(HttpClient, this.ExternalLocationsApiUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ExternalLocation> Get(string name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{this.ExternalLocationsApiUri}/{name}";
        return await HttpGet<ExternalLocation>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ExternalLocation> Update(
        string externalLocationName,
        string newName = default,
        string url = default,
        string credentialName = default,
        bool? readOnly = default,
        string comment = default,
        string owner = default,
        bool? force = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{this.ExternalLocationsApiUri}/{externalLocationName}";

        var request = new
        {
            name = newName,
            url,
            credential_name = credentialName,
            read_only = readOnly,
            comment,
            owner,
            force
        };

        return await HttpPatch<dynamic, ExternalLocation>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{this.ExternalLocationsApiUri}/{name}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

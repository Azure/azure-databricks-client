using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
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

    public async Task<ExternalLocationsList> List(CancellationToken cancellationToken = default)
    {
        return await HttpGet<ExternalLocationsList>(HttpClient, this.ExternalLocationsApiUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ExternalLocation> Create(
        string name,
        string url,
        string credentialName,
        bool? readOnly = default,
        string comment = default,
        bool? skipValidation = default,
        CancellationToken cancellationToken = default)
    {
        var request = new Dictionary<string, string>()
        {
            {"name", name },
            {"url", url },
            {"credential_name", credentialName }
        };

        if (readOnly != null)
        {
            request["read_only"] = readOnly.ToString().ToLower();
        }

        if (comment != null)
        {
            request["comment"] = comment;
        }

        if (skipValidation != null)
        {
            request["skip_validation"] = skipValidation.ToString().ToLower();
        }

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();

        return await HttpPost<JsonObject, ExternalLocation>(HttpClient, this.ExternalLocationsApiUri, requestJson, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ExternalLocation> Get(string name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{this.ExternalLocationsApiUri}/{name}";
        return await HttpGet<ExternalLocation>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<ExternalLocation> Update(
        string externaLocationName,
        string newName = default,
        string url = default,
        string credentialName = default,
        bool? readOnly = default,
        string comment = default,
        string owner = default,
        bool? force = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{this.ExternalLocationsApiUri}/{externaLocationName}";

        var request = new Dictionary<string, string>();

        if (newName != null)
        {
            request["name"] = newName;
        }

        if (url != null)
        {
            request["url"] = url;
        }

        if (credentialName != null)
        {
            request["credential_name"] = credentialName;
        }

        if (readOnly != null)
        {
            request["read_only"] = readOnly.ToString().ToLower();
        }

        if (comment != null)
        {
            request["comment"] = comment;
        }

        if (owner != null)
        {
            request["owner"] = owner;
        }

        if (force != null)
        {
            request["force"] = force.ToString().ToLower();
        }

        var requestJson = JsonSerializer.SerializeToNode(request, Options).AsObject();
        return await HttpPatch<JsonObject, ExternalLocation>(HttpClient, requestUri, requestJson, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{this.ExternalLocationsApiUri}/{name}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

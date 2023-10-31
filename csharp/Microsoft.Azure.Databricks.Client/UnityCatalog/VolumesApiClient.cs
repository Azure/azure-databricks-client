using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class VolumesApiClient : ApiClient, IVolumesApi
{
    public VolumesApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<Volume> Create(VolumeAttributes volumeAttributes, CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(volumeAttributes, Options)!.AsObject();
        var requestUri = $"{BaseUnityCatalogUri}/volumes";
        return await HttpPost<JsonObject, Volume>(this.HttpClient, requestUri, request, cancellationToken);
    }

    public async Task<Volume> Get(string fullVolumeName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/volumes/{fullVolumeName}";

        return await HttpGet<Volume>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

   public async Task<Volume> Update(
        string fullVolumeName,
        string name = default,
        string owner = default,
        string comment = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/volumes/{fullVolumeName}";

        var requestDict = new Dictionary<string, string>();

        if (name !=  null)
        {
            requestDict["name"] = name;
        }

        if (owner != null)
        {
            requestDict["owner"] = owner;
        }

        if (comment != null)
        {
            requestDict["comment"] = comment;
        }

        var request = JsonSerializer.SerializeToNode(requestDict, Options)!.AsObject();

        return await HttpPatch<JsonObject, Volume>(this.HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string fullVolumeName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/volumes/{fullVolumeName}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

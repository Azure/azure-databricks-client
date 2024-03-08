using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Net.Http;
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
        var requestUri = $"{BaseUnityCatalogUri}/volumes";
        return await HttpPost<VolumeAttributes, Volume>(this.HttpClient, requestUri, volumeAttributes,
            cancellationToken);
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
        var request = new { name, owner, comment };
        return await HttpPatch<dynamic, Volume>(this.HttpClient, requestUri, request, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Delete(string fullVolumeName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/volumes/{fullVolumeName}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

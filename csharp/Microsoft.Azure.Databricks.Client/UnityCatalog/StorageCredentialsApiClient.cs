using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public class StorageCredentialsApiClient : ApiClient, IStorageCredentialsApi
{
    public StorageCredentialsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IEnumerable<StorageCredential>> List(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/storage-credentials";
        var credentialsList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        credentialsList.TryGetPropertyValue("storage_credentials", out var credentials);
        return credentials?.Deserialize<IEnumerable<StorageCredential>>(Options) ?? Enumerable.Empty<StorageCredential>();
    }

    public async Task<StorageCredential> Create(
        StorageCredentialAttributes credentialAttributes,
        bool? skipValidation = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/storage-credentials";

        var request = JsonSerializer.SerializeToNode(credentialAttributes, Options).AsObject();

        if (skipValidation != null)
        {
            request.Add("skip_validation", skipValidation);
        }

        return await HttpPost<JsonObject, StorageCredential>(this.HttpClient, requestUri, request, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<StorageCredential> Get(string name, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/storage-credentials/{name}";
        return await HttpGet<StorageCredential>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<StorageCredential> Update(
        string storageCredentialName,
        StorageCredentialAttributes credentialAttributes = default,
        bool? skipValidation = default,
        bool? force = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/storage-credentials/{storageCredentialName}";
        var request = JsonSerializer.SerializeToNode(credentialAttributes, Options).AsObject();

        if (skipValidation != null)
        {
            request.Add("skip_validation", skipValidation);
        }

        if (force != null)
        {
            request.Add("force", force);
        }

        return await HttpPatch<JsonObject, StorageCredential>(HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string storageCredentialName, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseUnityCatalogUri}/storage-credentials/{storageCredentialName}";
        await HttpDelete(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

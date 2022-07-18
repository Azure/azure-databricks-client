// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public class SecretsApiClient : ApiClient, ISecretsApi
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecretsApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    public SecretsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    [Obsolete("This method has been renamed to " + nameof(CreateDatabricksBackedScope) + ".")]
    public async Task CreateScope(string scope, string initialManagePrincipal,
        CancellationToken cancellationToken = default)
    {
        await CreateDatabricksBackedScope(scope, initialManagePrincipal, cancellationToken);
    }

    public async Task CreateDatabricksBackedScope(string scope, string initialManagePrincipal,
        CancellationToken cancellationToken = default)
    {
        var request = new { scope, initial_manage_principal = initialManagePrincipal };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/scopes/create", request, cancellationToken).ConfigureAwait(false);
    }

    /*
     This API call is currently not working per https://github.com/MicrosoftDocs/azure-docs/issues/65000. Comment out for now.
    public async Task CreateAkvBackedScope(string scope, string initialManagePrincipal, string akvResourceId, string akvDnsName,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            scope,
            initial_manage_principal = initialManagePrincipal,
            scope_backend_type = "AZURE_KEYVAULT",
            backend_azure_keyvault = new
            {
                resource_id = akvResourceId,
                dns_name = akvDnsName
            }
        };

        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/scopes/create", request, cancellationToken).ConfigureAwait(false);
    }
    */

    public async Task DeleteScope(string scope, CancellationToken cancellationToken = default)
    {
        var request = new { scope };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/scopes/delete", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SecretScope>> ListScopes(CancellationToken cancellationToken = default)
    {
        var scopeList =
            await HttpGet<JsonObject>(this.HttpClient, $"{ApiVersion}/secrets/scopes/list", cancellationToken)
                .ConfigureAwait(false);

        return scopeList.TryGetPropertyValue("scopes", out var scopes)
            ? scopes.Deserialize<IEnumerable<SecretScope>>(Options)
            : Enumerable.Empty<SecretScope>();
    }

    public async Task PutSecret(string secretValue, string scope, string key, CancellationToken cancellationToken = default)
    {
        var request = new { scope, key, string_value = secretValue };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/put", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task PutSecret(byte[] secretValue, string scope, string key, CancellationToken cancellationToken = default)
    {
        var request = new { scope, key, bytes_value = secretValue };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/put", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteSecret(string scope, string key, CancellationToken cancellationToken = default)
    {
        var request = new { scope, key };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/delete", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<SecretMetadata>> ListSecrets(string scope,
        CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/secrets/list?scope={scope}";
        var secretList = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        return secretList.TryGetPropertyValue("secrets", out var secrets)
            ? secrets.Deserialize<IEnumerable<SecretMetadata>>(Options)
            : Enumerable.Empty<SecretMetadata>();
    }

    public async Task PutSecretAcl(string scope, string principal, PermissionLevelV1 permission, CancellationToken cancellationToken = default)
    {
        var request = new { scope, principal, permission };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/acls/put", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteSecretAcl(string scope, string principal, CancellationToken cancellationToken = default)
    {
        var request = new { scope, principal };
        await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/acls/delete", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<AclPermissionItemV1> GetSecretAcl(string scope, string principal, CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/secrets/acls/get?scope={scope}&principal={principal}";
        return await HttpGet<AclPermissionItemV1>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItemV1>> ListSecretAcl(string scope,
        CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/secrets/acls/list?scope={scope}";
        var aclList = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        return aclList.TryGetPropertyValue("items", out var items)
            ? items.Deserialize<IEnumerable<AclPermissionItemV1>>(Options)
            : Enumerable.Empty<AclPermissionItemV1>();
    }
}
using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
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
            var request = new {scope, initial_manage_principal = initialManagePrincipal};
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
            var request = new {scope};
            await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/scopes/delete", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SecretScope>> ListScopes(CancellationToken cancellationToken = default)
        {
            var scopeList = await HttpGet<JsonObject>(this.HttpClient, $"{ApiVersion}/secrets/scopes/list", cancellationToken).ConfigureAwait(false);

            if (scopeList.TryGetPropertyValue("scopes", out var scopes))
            {
                return scopes.Deserialize<IEnumerable<SecretScope>>(Options);
            }
            else
            {
                return Enumerable.Empty<SecretScope>();
            }
        }

        public async Task PutSecret(string secretValue, string scope, string key, CancellationToken cancellationToken = default)
        {
            var request = new {scope, key, string_value = secretValue};
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

        public async Task<IEnumerable<SecretMetadata>> ListSecrets(string scope, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/secrets/list?scope={scope}";
            var secretList = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            if (secretList.TryGetPropertyValue("secrets", out var secrets))
            {
                return secrets.Deserialize<IEnumerable<SecretMetadata>>(Options);
            }
            else
            {
                return Enumerable.Empty<SecretMetadata>();
            }
        }

        public async Task PutSecretAcl(string scope, string principal, AclPermission permission, CancellationToken cancellationToken = default)
        {
            var request = new { scope, principal, permission = permission.ToString() };
            await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/acls/put", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteSecretAcl(string scope, string principal, CancellationToken cancellationToken = default)
        {
            var request = new { scope, principal };
            await HttpPost(this.HttpClient, $"{ApiVersion}/secrets/acls/delete", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<AclItem> GetSecretAcl(string scope, string principal, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/secrets/acls/get?scope={scope}&principal={principal}";
            return await HttpGet<AclItem>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<AclItem>> ListSecretAcl(string scope, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/secrets/acls/list?scope={scope}";
            var aclList = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            if (aclList.TryGetPropertyValue("items", out var items))
            {
                return items.Deserialize<IEnumerable<AclItem>>(Options);
            }
            else
            {
                return Enumerable.Empty<AclItem>();
            }
        }
    }
}

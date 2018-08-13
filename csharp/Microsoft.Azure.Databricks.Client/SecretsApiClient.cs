using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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


        public async Task CreateScope(string scope, string initialManagePrincipal)
        {
            var request = new {scope, initial_manage_principal = initialManagePrincipal};
            await HttpPost(this.HttpClient, "secrets/scopes/create", request).ConfigureAwait(false);
        }

        public async Task DeleteScope(string scope)
        {
            var request = new {scope};
            await HttpPost(this.HttpClient, "secrets/scopes/delete", request).ConfigureAwait(false);
        }

        private static readonly JsonSerializer SecretScopeConverter = new JsonSerializer
        {
            Converters = { new SecretScopeConverter() }
        };

        public async Task<IEnumerable<SecretScope>> ListScopes()
        {
            var scopeList = await HttpGet<dynamic>(this.HttpClient, "secrets/scopes/list").ConfigureAwait(false);

            return PropertyExists(scopeList, "scopes")
                ? scopeList.scopes.ToObject<IEnumerable<SecretScope>>(SecretScopeConverter)
                : Enumerable.Empty<SecretScope>();
        }

        public async Task PutSecret(string secretValue, string scope, string key)
        {
            var request = new {scope, key, string_value = secretValue};
            await HttpPost(this.HttpClient, "secrets/put", request).ConfigureAwait(false);
        }

        public async Task PutSecret(byte[] secretValue, string scope, string key)
        {
            var request = new { scope, key, bytes_value = secretValue };
            await HttpPost(this.HttpClient, "secrets/put", request).ConfigureAwait(false);
        }

        public async Task DeleteSecret(string scope, string key)
        {
            var request = new { scope, key };
            await HttpPost(this.HttpClient, "secrets/delete", request).ConfigureAwait(false);
        }

        public async Task<IEnumerable<SecretMetadata>> ListSecrets(string scope)
        {
            var url = $"secrets/list?scope={scope}";
            var secretList = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);
            return PropertyExists(secretList, "secrets")
                ? secretList.secrets.ToObject<IEnumerable<SecretMetadata>>()
                : Enumerable.Empty<SecretMetadata>();
        }

        public async Task PutSecretAcl(string scope, string principal, AclPermission permission)
        {
            var request = new { scope, principal, permission = permission.ToString() };
            await HttpPost(this.HttpClient, "secrets/acls/put", request).ConfigureAwait(false);
        }

        public async Task DeleteSecretAcl(string scope, string principal)
        {
            var request = new { scope, principal };
            await HttpPost(this.HttpClient, "secrets/acls/delete", request).ConfigureAwait(false);
        }

        public async Task<AclItem> GetSecretAcl(string scope, string principal)
        {
            var url = $"secrets/acls/get?scope={scope}&principal={principal}";
            return await HttpGet<AclItem>(this.HttpClient, url).ConfigureAwait(false);
        }

        public async Task<IEnumerable<AclItem>> ListSecretAcl(string scope)
        {
            var url = $"secrets/acls/list?scope={scope}";
            var aclList = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);
            return PropertyExists(aclList, "items")
                ? aclList.items.ToObject<IEnumerable<AclItem>>()
                : Enumerable.Empty<AclItem>();
        }
    }
}

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class TokenApiClient : ApiClient, ITokenApi
    {
        public TokenApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<(string, PublicTokenInfo)> Create(long? lifetimeSeconds, string comment)
        {
            var request = new {lifetime_seconds = lifetimeSeconds, comment};
            var result = await HttpPost<dynamic, dynamic>(this.HttpClient, "token/create", request)
                .ConfigureAwait(false);

            return (result.token_value.ToObject<string>(), result.token_info.ToObject<PublicTokenInfo>());
        }

        public async Task<IEnumerable<PublicTokenInfo>> List()
        {
            var result = await HttpGet<dynamic>(this.HttpClient, "token/list").ConfigureAwait(false);
            return result.token_infos.ToObject<IEnumerable<PublicTokenInfo>>();
        }

        public async Task Revoke(string tokenId)
        {
            var request = new {token_id = tokenId};
            await HttpPost(this.HttpClient, "token/delete", request).ConfigureAwait(false);
        }
    }
}
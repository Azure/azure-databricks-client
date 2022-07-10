using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class TokenApiClient : ApiClient, ITokenApi
    {
        public TokenApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<(string, PublicTokenInfo)> Create(long? lifetimeSeconds, string comment, CancellationToken cancellationToken = default)
        {
            var request = new {lifetime_seconds = lifetimeSeconds, comment};
            var result = await HttpPost<dynamic, dynamic>(this.HttpClient, $"{ApiVersion}/token/create", request, cancellationToken)
                .ConfigureAwait(false);

            return (result.token_value.ToObject<string>(), result.token_info.ToObject<PublicTokenInfo>());
        }

        public async Task<IEnumerable<PublicTokenInfo>> List(CancellationToken cancellationToken = default)
        {
            var result = await HttpGet<dynamic>(this.HttpClient, $"{ApiVersion}/token/list", cancellationToken).ConfigureAwait(false);
            return result.token_infos.ToObject<IEnumerable<PublicTokenInfo>>();
        }

        public async Task Revoke(string tokenId, CancellationToken cancellationToken = default)
        {
            var request = new {token_id = tokenId};
            await HttpPost(this.HttpClient, $"{ApiVersion}/token/delete", request, cancellationToken).ConfigureAwait(false);
        }
    }
}
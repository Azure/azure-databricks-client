using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class GroupsApiClient : ApiClient, IGroupsApi
    {
        public GroupsApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task AddMember(string parentGroupName, PrincipalName principalName, CancellationToken cancellationToken = default)
        {
            var groupMembership = new GroupMembership
            {
                UserName = principalName.UserName,
                GroupName = principalName.GroupName,
                ParentName = parentGroupName
            };

            await HttpPost(this.HttpClient, $"{ApiVersion}/groups/add-member", groupMembership, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> Create(string groupName, CancellationToken cancellationToken = default)
        {
            var response = await HttpPost<dynamic, JsonObject>(this.HttpClient, $"{ApiVersion}/groups/create", new {group_name = groupName}, cancellationToken)
                .ConfigureAwait(false);
            return response["group_name"].GetValue<string>();
        }

        public async Task<IEnumerable<PrincipalName>> ListMembers(string groupName, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/groups/list-members?group_name={groupName}";
            var response = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

            if (response.TryGetPropertyValue("members", out var members))
            {
                return members.Deserialize<IEnumerable<PrincipalName>>();
            }
            else
            {
                return Enumerable.Empty<PrincipalName>();
            }
        }

        public async Task<IEnumerable<string>> List(CancellationToken cancellationToken = default)
        {
            var response = await HttpGet<JsonObject>(this.HttpClient, $"{ApiVersion}/groups/list", cancellationToken).ConfigureAwait(false);
            if (response.TryGetPropertyValue("group_names", out var group_names))
            {
                return group_names.Deserialize<IEnumerable<string>>();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public async Task<IEnumerable<string>> ListParent(PrincipalName principalName, CancellationToken cancellationToken = default)
        {
            var url = !string.IsNullOrEmpty(principalName.UserName)
                ? $"{ApiVersion}/groups/list-parents?user_name={principalName.UserName}"
                : $"{ApiVersion}/groups/list-parents?group_name={principalName.GroupName}";

            var response = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

            if (response.TryGetPropertyValue("group_names", out var group_names))
            {
                return group_names.Deserialize<IEnumerable<string>>();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public async Task RemoveMember(string parentGroupName, PrincipalName principalName, CancellationToken cancellationToken = default)
        {
            var groupMembership = new GroupMembership
            {
                UserName = principalName.UserName,
                GroupName = principalName.GroupName,
                ParentName = parentGroupName
            };

            await HttpPost(this.HttpClient, $"{ApiVersion}/groups/remove-member", groupMembership, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(string groupName, CancellationToken cancellationToken = default)
        {
            var request = new {group_name = groupName};
            await HttpPost(this.HttpClient, $"{ApiVersion}/groups/delete", request, cancellationToken).ConfigureAwait(false);
        }
    }
}

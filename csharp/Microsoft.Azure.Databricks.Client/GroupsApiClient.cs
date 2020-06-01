using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

            await HttpPost(this.HttpClient, "groups/add-member", groupMembership, cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> Create(string groupName, CancellationToken cancellationToken = default)
        {
            var response = await HttpPost<dynamic, dynamic>(this.HttpClient, "groups/create", new {group_name = groupName}, cancellationToken)
                .ConfigureAwait(false);
            return response.group_name.ToObject<string>();
        }

        public async Task<IEnumerable<PrincipalName>> ListMembers(string groupName, CancellationToken cancellationToken = default)
        {
            var url = $"groups/list-members?group_name={groupName}";
            var response = await HttpGet<dynamic>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            return PropertyExists(response, "members")
                ? response.members.ToObject<IEnumerable<PrincipalName>>()
                : Enumerable.Empty<PrincipalName>();
        }

        public async Task<IEnumerable<string>> List(CancellationToken cancellationToken = default)
        {
            var response = await HttpGet<dynamic>(this.HttpClient, "groups/list", cancellationToken).ConfigureAwait(false);
            return PropertyExists(response, "group_names")
                ? response.group_names.ToObject<IEnumerable<string>>()
                : Enumerable.Empty<string>();
        }

        public async Task<IEnumerable<string>> ListParent(PrincipalName principalName, CancellationToken cancellationToken = default)
        {
            var url = !string.IsNullOrEmpty(principalName.UserName)
                ? $"groups/list-parents?user_name={principalName.UserName}"
                : $"groups/list-parents?group_name={principalName.GroupName}";

            var response = await HttpGet<dynamic>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            return PropertyExists(response, "group_names")
                ? response.group_names.ToObject<IEnumerable<string>>()
                : Enumerable.Empty<string>();
        }

        public async Task RemoveMember(string parentGroupName, PrincipalName principalName, CancellationToken cancellationToken = default)
        {
            var groupMembership = new GroupMembership
            {
                UserName = principalName.UserName,
                GroupName = principalName.GroupName,
                ParentName = parentGroupName
            };

            await HttpPost(this.HttpClient, "groups/remove-member", groupMembership, cancellationToken).ConfigureAwait(false);
        }

        public async Task Delete(string groupName, CancellationToken cancellationToken = default)
        {
            var request = new {group_name = groupName};
            await HttpPost(this.HttpClient, "groups/delete", request, cancellationToken).ConfigureAwait(false);
        }
    }
}

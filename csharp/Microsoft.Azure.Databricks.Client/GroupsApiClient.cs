using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class GroupsApiClient : ApiClient, IGroupsApi
    {
        public GroupsApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task AddMember(string parentGroupName, PrincipalName principalName)
        {
            dynamic request = principalName;
            request.parent_name = parentGroupName;

            await HttpPost(this.HttpClient, "groups/add-member", request).ConfigureAwait(false);
        }

        public async Task<string> Create(string groupName)
        {
            var response = await HttpPost<dynamic, dynamic>(this.HttpClient, "groups/create", new {group_name = groupName})
                .ConfigureAwait(false);
            return response.group_name.ToObject<string>();
        }

        public async Task<IEnumerable<PrincipalName>> ListMembers(string groupName)
        {
            var url = $"groups/list-members?group_name={groupName}";
            var response = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);
            return response.members.ToObject<IEnumerable<PrincipalName>>();
        }

        public async Task<IEnumerable<string>> List()
        {
            var response = await HttpGet<dynamic>(this.HttpClient, "groups/list").ConfigureAwait(false);
            return response.members.ToObject<IEnumerable<string>>();
        }

        public async Task<IEnumerable<string>> ListParent(PrincipalName principalName)
        {
            var url = !string.IsNullOrEmpty(principalName.UserName)
                ? $"groups/list-parents?user_name={principalName.UserName}"
                : $"groups/list-parents?group_name={principalName.GroupName}";

            var response = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);
            return response.group_names.ToObject<IEnumerable<string>>();
        }

        public async Task RemoveMember(string parentGroupName, PrincipalName principalName)
        {
            dynamic request = principalName;
            request.parent_name = parentGroupName;

            await HttpPost(this.HttpClient, "groups/remove-member", request).ConfigureAwait(false);
        }

        public async Task Delete(string groupName)
        {
            var request = new {group_name = groupName};
            await HttpPost(this.HttpClient, "groups/delete", request).ConfigureAwait(false);
        }
    }
}

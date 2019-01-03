using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class GroupMembership : PrincipalName
    {
        /// <summary>
        /// Name of the parent group to which the new member will be added.
        /// </summary>
        [JsonProperty(PropertyName = "parent_name")]
        public string ParentName { get; set; }
    }
}
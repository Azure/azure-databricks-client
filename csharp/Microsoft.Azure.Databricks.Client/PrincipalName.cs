using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class PrincipalName
    {
        /// <summary>
        /// The user name.
        /// </summary>
        [JsonProperty(PropertyName = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [JsonProperty(PropertyName = "group_name")]
        public string GroupName { get; set; }
    }
}
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class PrincipalName
    {
        /// <summary>
        /// The user name.
        /// </summary>
        [JsonProperty(PropertyName = "user_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserName { get; set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [JsonProperty(PropertyName = "group_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string GroupName { get; set; }
    }
}
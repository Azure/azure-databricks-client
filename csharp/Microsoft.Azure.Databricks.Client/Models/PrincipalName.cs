using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record PrincipalName
    {
        /// <summary>
        /// The user name.
        /// </summary>
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [JsonPropertyName("group_name")]
        public string GroupName { get; set; }
    }
}
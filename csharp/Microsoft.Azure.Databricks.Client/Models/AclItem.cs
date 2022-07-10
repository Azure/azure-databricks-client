using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// An item representing an ACL rule applied to the given principal (user or group) on the associated scope point.
    /// </summary>
    public record AclItem
    {
        /// <summary>
        /// The principal to which the permission is applied. This field is required.
        /// </summary>
        [JsonPropertyName("principal")]
        public string Principal { get; set; }

        /// <summary>
        /// The permission level applied to the principal. This field is required.
        /// </summary>
        [JsonPropertyName("permission")]
        public AclPermission Permission { get; set; }
    }
}
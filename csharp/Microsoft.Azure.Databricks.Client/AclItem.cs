using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An item representing an ACL rule applied to the given principal (user or group) on the associated scope point.
    /// </summary>
    public class AclItem
    {
        /// <summary>
        /// The principal to which the permission is applied. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "principal")]
        public string Principal { get; set; }

        /// <summary>
        /// The permission level applied to the principal. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "permission")]
        public AclPermission Permission { get; set; }
    }
}
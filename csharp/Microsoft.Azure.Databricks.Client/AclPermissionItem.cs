using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An abstract item representing an ACL rule for an object. To be used with the permissions API
    /// </summary>
    public abstract class AclPermissionItem
    {
        /// <summary>
        /// The principal to which the permission is applied. This field is required.
        /// </summary>
        public virtual string Principal { get; set; }

        /// <summary>
        /// The permission level applied to the principal. This field is required.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("permission_level")]
        public virtual PermissionLevel Permission { get; set; }
    }
}
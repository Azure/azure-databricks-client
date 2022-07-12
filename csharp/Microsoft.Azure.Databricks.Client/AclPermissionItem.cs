using System.Collections.Generic;

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
        public string Principal { get; set; }

        /// <summary>
        /// The permission level applied to the principal. This field is required.
        /// </summary>
        public PermissionLevel Permission { get; set; }

        internal abstract Dictionary<string,string> ToDictionaryRepresentation();
    }
}
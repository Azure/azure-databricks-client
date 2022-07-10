// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// The ACL permission levels for Secret ACLs applied to secret scopes.
    /// </summary>
    public enum AclPermission
    {
        /// <summary>
        /// Allowed to perform read operations (get, list) on secrets in this scope.
        /// </summary>
        READ,

        /// <summary>
        /// Allowed to read and write secrets to this secret scope.
        /// </summary>
        WRITE,

        /// <summary>
        /// Allowed to read/write ACLs, and read/write secrets to this secret scope.
        /// </summary>
        MANAGE
    }
}
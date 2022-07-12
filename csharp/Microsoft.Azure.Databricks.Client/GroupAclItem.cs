
using System.Collections.Generic;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An item representing an ACL rule applied to a specific group. To be used when updated permission levels through the permissions Api
    /// </summary>
    public class GroupAclItem : AclPermissionItem
    {
        internal override Dictionary<string, string> ToDictionaryRepresentation()
        {
            return new Dictionary<string, string>
            {
                {"group_name", Principal},
                {"permission_level", Permission.ToString()}
            };
        }
    }
}
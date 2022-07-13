
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An item representing an ACL rule applied to a specific group. To be used when updated permission levels through the permissions Api
    /// </summary>
    public class GroupAclItem : AclPermissionItem
    {
        [JsonProperty("group_name")]
        public override string Principal { get => base.Principal; set => base.Principal = value; }
    }
}
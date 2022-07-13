using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// An item representing an ACL rule applied to a specific user. To be used when updated permission levels through the permissions Api
    /// </summary>
    public class UserAclItem : AclPermissionItem
    {
        [JsonProperty("user_name")]
        public override string Principal { get => base.Principal; set => base.Principal = value; }
    }
}
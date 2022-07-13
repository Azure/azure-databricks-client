using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

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

        internal static IEnumerable<AclPermissionItem> ParseFromPermissionsHttpResult(JObject result)
        {
            if (!result.ContainsKey("access_control_list"))
            {
                return Enumerable.Empty<AclPermissionItem>();
            }
            List<AclPermissionItem> list = new List<AclPermissionItem>();
            var acl = result["access_control_list"].Children<JObject>();
            var groups = 
                from rules in acl
                where rules.ContainsKey("group_name")
                select new GroupAclItem
                {
                    Principal = (string)rules["group_name"],
                    Permission = (PermissionLevel)Enum.Parse(typeof(PermissionLevel), (string)rules["all_permissions"][0]["permission_level"])
                };
            var users = 
                from rules in acl
                where rules.ContainsKey("user_name")
                select new UserAclItem
                {
                    Principal = (string)rules["user_name"],
                    Permission = (PermissionLevel)Enum.Parse(typeof(PermissionLevel), (string)rules["all_permissions"][0]["permission_level"])
                };
            var servicePrincipals = 
                from rules in acl
                where rules.ContainsKey("service_principal_name")
                select new ServicePrincipalAclItem
                {
                    Principal = (string)rules["service_principal_name"],
                    Permission = (PermissionLevel)Enum.Parse(typeof(PermissionLevel), (string)rules["all_permissions"][0]["permission_level"])
                };
            list.AddRange(groups);
            list.AddRange(users);
            list.AddRange(servicePrincipals);
            return list;
        }
    }
}
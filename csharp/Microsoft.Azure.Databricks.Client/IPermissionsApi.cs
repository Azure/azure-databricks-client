using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
        public interface IPermissionsApi : IDisposable
        {
                /// <summary>
                /// Returns a JSON representation of the possible permissions levels for tokens. For details, 
                /// see the required token permission levels for various actions. The results of this request do not change 
                /// based on the state of the workspace or the permissions of the calling user. This request is published
                /// for consistency with other permissions APIs.
                /// </summary>
                Task<IEnumerable<(PermissionLevel, string description)>> GetTokenPermissionsLevels();

                /// <summary>
                /// Get the set of all token permissions for the workspace. 
                /// </summary>
                /// <returns></returns>
                Task<IEnumerable<AclItem>> GetTokenPermissions();

                /// <summary>
                /// Grant token permissions for one or more users, groups, or service principals. 
                /// You can only grant the Can Use (CAN_USE) permission. 
                /// The Can Manage (CAN_MANAGE) permission level cannot be granted with this API 
                /// because it is tied automatically to membership in the admins group.
                /// IMPORTANT: You cannot use this request to revoke (remove) any permissions. 
                /// The only way to remove permissions is with the replace token permissions for 
                /// entire workspace API, which requires you specify the complete set of permissions 
                /// for all objects that are granted permissions. 
                /// </summary>
                /// <param name="AccessControlList"></param>
                /// <returns></returns>
                Task UpdateTokenPermissions(IEnumerable<AclPermissionItem> AccessControlList);

                /// <summary>
                /// Update all token permissions for all users, groups, and service principals for 
                /// the entire workspace. The permissions that you specify in this request overwrite 
                /// the existing permissions entirely. You must provide a complete set of all 
                /// permissions for all objects in one request.
                /// At the end of processing your request, all users and service principals that 
                /// do not have either CAN_USE or CAN_MANAGE permission either explicitly or implicitly 
                /// due to group assignment no longer have any tokens permissions. Affected users or 
                /// service principals immediately have all their tokens deleted.
                /// </summary>
                /// <param name="AccessControlList"></param>
                /// <returns></returns>
                Task ReplaceTokenPermissionsForWorkspace(IEnumerable<AclPermissionItem> AccessControlList);
        }
}
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
                Task<IEnumerable<AclPermissionItem>> GetTokenPermissions();

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

                Task<IEnumerable<(PermissionLevel, string description)>> GetClusterPermissionLevels(string clusterId);
                Task<IEnumerable<AclPermissionItem>> GetClusterPermissions(string clusterId);
                Task UpdateClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId);
                Task ReplaceClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetInstancePoolPermissionLevels(string instancePoolId);
                Task<IEnumerable<AclPermissionItem>> GetInstancePoolPermissions(string instancePoolId);
                Task UpdateInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId);
                Task ReplaceInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetJobPermissionLevels(string jobId);
                Task<IEnumerable<AclPermissionItem>> GetJobPermissions(string jobId);
                Task UpdateJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId);
                Task ReplaceJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetPipelinePermissionLevels(string pipelineId);
                Task<IEnumerable<AclPermissionItem>> GetPipelinePermissions(string pipelineId);
                Task UpdatePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId);
                Task ReplacePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetNotebookPermissionLevels(string notebookId);
                Task<IEnumerable<AclPermissionItem>> GetNotebookPermissions(string notebookId);
                Task UpdateNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId);
                Task ReplaceNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetDirectoryPermissionLevels(string directoryId);
                Task<IEnumerable<AclPermissionItem>> GetDirectoryPermissions(string directoryId);
                Task UpdateDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId);
                Task ReplaceDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetExperimentPermissionLevels(string experimentId);
                Task<IEnumerable<AclPermissionItem>> GetExperimentPermissions(string experimentId);
                Task UpdateExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId);
                Task ReplaceExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetRegisteredModelPermissionLevels(string registeredModelId);
                Task<IEnumerable<AclPermissionItem>> GetRegisteredModelPermissions(string registeredModelId);
                Task UpdateRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId);
                Task ReplaceRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetSqlWarehousePermissionLevels(string endpointId);
                Task<IEnumerable<AclPermissionItem>> GetSqlWarehousePermissions(string endpointId);
                Task UpdateSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId);
                Task ReplaceSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId);

                Task<IEnumerable<(PermissionLevel, string description)>> GetRepoPermissionLevels(string repoId);
                Task<IEnumerable<AclPermissionItem>> GetRepoPermissions(string repoId);
                Task UpdateRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId);
                Task ReplaceRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId);
        }
}
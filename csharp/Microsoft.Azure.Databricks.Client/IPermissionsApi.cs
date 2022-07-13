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
                Task<IEnumerable<AclPermissionDescription>> GetTokenPermissionsLevels(CancellationToken cancellationToken = default);

                /// <summary>
                /// Get the set of all token permissions for the workspace. 
                /// </summary>
                /// <returns></returns>
                Task<IEnumerable<AclPermissionItem>> GetTokenPermissions(CancellationToken cancellationToken = default);

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
                Task UpdateTokenPermissions(IEnumerable<AclPermissionItem> AccessControlList, CancellationToken cancellationToken = default);

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
                Task ReplaceTokenPermissionsForWorkspace(IEnumerable<AclPermissionItem> AccessControlList, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetClusterPermissionLevels(string clusterId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetClusterPermissions(string clusterId, CancellationToken cancellationToken = default);
                Task UpdateClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId, CancellationToken cancellationToken = default);
                Task ReplaceClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetInstancePoolPermissionLevels(string instancePoolId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetInstancePoolPermissions(string instancePoolId, CancellationToken cancellationToken = default);
                Task UpdateInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId, CancellationToken cancellationToken = default);
                Task ReplaceInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetJobPermissionLevels(string jobId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetJobPermissions(string jobId, CancellationToken cancellationToken = default);
                Task UpdateJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId, CancellationToken cancellationToken = default);
                Task ReplaceJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetPipelinePermissionLevels(string pipelineId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetPipelinePermissions(string pipelineId, CancellationToken cancellationToken = default);
                Task UpdatePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId, CancellationToken cancellationToken = default);
                Task ReplacePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetNotebookPermissionLevels(string notebookId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetNotebookPermissions(string notebookId, CancellationToken cancellationToken = default);
                Task UpdateNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId, CancellationToken cancellationToken = default);
                Task ReplaceNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetDirectoryPermissionLevels(string directoryId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetDirectoryPermissions(string directoryId, CancellationToken cancellationToken = default);
                Task UpdateDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId, CancellationToken cancellationToken = default);
                Task ReplaceDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetExperimentPermissionLevels(string experimentId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetExperimentPermissions(string experimentId, CancellationToken cancellationToken = default);
                Task UpdateExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId, CancellationToken cancellationToken = default);
                Task ReplaceExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetRegisteredModelPermissionLevels(string registeredModelId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetRegisteredModelPermissions(string registeredModelId, CancellationToken cancellationToken = default);
                Task UpdateRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId, CancellationToken cancellationToken = default);
                Task ReplaceRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetSqlWarehousePermissionLevels(string endpointId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetSqlWarehousePermissions(string endpointId, CancellationToken cancellationToken = default);
                Task UpdateSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId, CancellationToken cancellationToken = default);
                Task ReplaceSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId, CancellationToken cancellationToken = default);

                Task<IEnumerable<AclPermissionDescription>> GetRepoPermissionLevels(string repoId, CancellationToken cancellationToken = default);
                Task<IEnumerable<AclPermissionItem>> GetRepoPermissions(string repoId, CancellationToken cancellationToken = default);
                Task UpdateRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId, CancellationToken cancellationToken = default);
                Task ReplaceRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId, CancellationToken cancellationToken = default);
        }
}
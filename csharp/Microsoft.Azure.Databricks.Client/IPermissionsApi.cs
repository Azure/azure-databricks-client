using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IPermissionsApi : IDisposable
    {
        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to tokens
        /// </summary>
        Task<IEnumerable<AclPermissionDescription>> GetTokenPermissionsLevels(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to tokens
        /// </summary>
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
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
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
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        Task ReplaceTokenPermissionsForWorkspace(IEnumerable<AclPermissionItem> AccessControlList, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target cluster
        /// </summary>
        /// <param name="clusterId">The id of the target cluster</param>
        Task<IEnumerable<AclPermissionDescription>> GetClusterPermissionLevels(string clusterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target cluster
        /// </summary>
        /// <param name="clusterId">The id of the target cluster</param>
        Task<IEnumerable<AclPermissionItem>> GetClusterPermissions(string clusterId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant cluster permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceClusterPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="clusterId">The id of the target cluster</param>
        Task UpdateClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all clusters permissions for a specific cluster, specifying all users, groups, or service principal.
        /// WARNING: This request overwrites all existing direct (non-inherited) permissions on the cluster and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="clusterId">The id of the target cluster</param>
        Task ReplaceClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target instance pool
        /// </summary>
        /// <param name="instancePoolId">The id of the target instance pool</param>
        Task<IEnumerable<AclPermissionDescription>> GetInstancePoolPermissionLevels(string instancePoolId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target instance pool
        /// </summary>
        /// <param name="instancePoolId">The id of the target instance pool</param>
        Task<IEnumerable<AclPermissionItem>> GetInstancePoolPermissions(string instancePoolId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant pool permissions for one or more users, groups, or service principal.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceInstancePoolPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="instancePoolId">The id of the target instance pool</param>
        Task UpdateInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all pool permissions for all users, groups, or service principal for a specific pool.
        /// WARNING: This request overwrites all existing permissions on the pool and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="instancePoolId">The id of the target instance pool</param>
        Task ReplaceInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target job
        /// </summary>
        /// <param name="jobId">The id of the target job</param>
        Task<IEnumerable<AclPermissionDescription>> GetJobPermissionLevels(string jobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target job
        /// </summary>
        /// <param name="jobId">The id of the target job</param>
        Task<IEnumerable<AclPermissionItem>> GetJobPermissions(string jobId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant jobs permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceJobPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="jobId">The id of the target job</param>
        Task UpdateJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all jobs permissions for all users, groups, or service principal for a specific job.
        /// WARNING: This request overwrites all existing direct permissions on the job and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="jobId">The id of the target job</param>
        Task ReplaceJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target pipeline
        /// </summary>
        /// <param name="pipelineId">The id of the target pipeline</param>
        Task<IEnumerable<AclPermissionDescription>> GetPipelinePermissionLevels(string pipelineId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target pipeline
        /// </summary>
        /// <param name="pipelineId">The id of the target pipeline</param>
        Task<IEnumerable<AclPermissionItem>> GetPipelinePermissions(string pipelineId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant permissions on a pipeline for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplacePipelinePermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="pipelineId">The id of the target pipeline</param>
        Task UpdatePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update permissions granted to users, groups and service principals on the specified pipeline.
        /// WARNING: This request overwrites all existing direct (non-inherited) permissions on the pipeline and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="pipelineId">The id of the target pipeline</param>
        Task ReplacePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target notebook
        /// </summary>
        /// <param name="notebookId">The id of the target notebook</param>
        Task<IEnumerable<AclPermissionDescription>> GetNotebookPermissionLevels(string notebookId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target notebook
        /// </summary>
        /// <param name="notebookId">The id of the target notebook</param>
        Task<IEnumerable<AclPermissionItem>> GetNotebookPermissions(string notebookId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant a notebook new permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceNotebookPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="notebookId">The id of the target notebook</param>
        Task UpdateNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all notebooks permissions for all users, groups, or service principal for a specific notebook.
        /// WARNING: This request overwrites all existing direct permissions on the notebook and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="notebookId">The id of the target notebook</param>
        Task ReplaceNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target directory
        /// </summary>
        /// <param name="directoryId">The id of the target directory</param>
        Task<IEnumerable<AclPermissionDescription>> GetDirectoryPermissionLevels(string directoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target directory
        /// </summary>
        /// <param name="directoryId">The id of the target directory</param>
        Task<IEnumerable<AclPermissionItem>> GetDirectoryPermissions(string directoryId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant a directory new permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceDirectoryPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="directoryId">The id of the target directory</param>
        Task UpdateDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all directory permissions for all users, groups, or service principal for a specific directory.
        /// WARNING: This request overwrites all existing direct permissions on the directory and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="directoryId">The id of the target directory</param>
        Task ReplaceDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target ML Flow experiment
        /// </summary>
        /// <param name="experimentId">The id of the target ML Flow experiment</param>
        Task<IEnumerable<AclPermissionDescription>> GetExperimentPermissionLevels(string experimentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target ML Flow experiment
        /// </summary>
        /// <param name="experimentId">The id of the target ML Flow experiment</param>
        Task<IEnumerable<AclPermissionItem>> GetExperimentPermissions(string experimentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant an experiment new permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceExperimentPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="experimentId">The id of the target ML Flow experiment</param>
        Task UpdateExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all experiment permissions for all users, groups or service principal for a specific experiment.
        /// WARNING: This request overwrites all existing direct permissions on the experiment and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="experimentId">The id of the target ML Flow experiment</param>
        Task ReplaceExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target ML Flow registered model
        /// </summary>
        /// <param name="registeredModelId">The id of the target ML Flow registered model</param>
        Task<IEnumerable<AclPermissionDescription>> GetRegisteredModelPermissionLevels(string registeredModelId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target ML Flow registered model
        /// </summary>
        /// <param name="registeredModelId">The id of the target ML Flow registered model</param>
        Task<IEnumerable<AclPermissionItem>> GetRegisteredModelPermissions(string registeredModelId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant MLflow registered model permissions for one or more users, groups, or service principals.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="registeredModelId">The id of the target Ml Flow registered model</param>
        Task UpdateRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all MLflow registered model permissions for all users, groups, or service principal for a specific registered model.
        /// WARNING: This request overwrites all existing direct permissions on the registered model and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="registeredModelId">The id of the target Ml Flow registered model</param>
        Task ReplaceRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target Sql warehouse
        /// </summary>
        /// <param name="endpointId">The endpoint id of the target Sql warehouse</param>
        Task<IEnumerable<AclPermissionDescription>> GetSqlWarehousePermissionLevels(string endpointId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target Sql warehous
        /// </summary>
        /// <param name="endpointId">The endpoint id of the target Sql warehouse</param>
        Task<IEnumerable<AclPermissionItem>> GetSqlWarehousePermissions(string endpointId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant SQL warehouse permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceSqlWarehousePermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="endpointId">The id of the target Sql warehous</param>
        Task UpdateSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all permissions for a specific SQL warehouse, specifying all users, groups or service principal.
        /// WARNING: This request overwrites all existing direct (non-inherited) permissions on the SQL warehouse and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="endpointId">The id of the target Sql warehous</param>
        Task ReplaceSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionDescription"/> representing all possible
        /// <see cref="PermissionLevel"/> values that can be applied to the target repository
        /// </summary>
        /// <param name="repoId">The id of the target repository</param>
        Task<IEnumerable<AclPermissionDescription>> GetRepoPermissionLevels(string repoId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously returns a list of <see cref="AclPermissionItem"/> representing all current
        /// permissions that are applied to the target repository
        /// </summary>
        /// <param name="repoId">The id of the target repository</param>
        Task<IEnumerable<AclPermissionItem>> GetRepoPermissions(string repoId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Grant a repo new permissions for one or more users, groups, or service principals.
        /// This request only grants (adds) permissions. To revoke, use <see cref="ReplaceRepoPermissions"/>.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="repoId">The id of the target repository</param>
        Task UpdateRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Update all repos permissions for all users, groups or service principal for a specific repo.
        /// WARNING: This request overwrites all existing direct permissions on the repo and replaces it with the new permissions specified in the request body.
        /// </summary>
        /// <param name="AccessControlList">A collection of <see cref="AclPermissionItem"/>
        /// representing the permissions to be updated</param>
        /// <param name="repoId">The id of the target repository</param>
        Task ReplaceRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId, CancellationToken cancellationToken = default);
    }
}
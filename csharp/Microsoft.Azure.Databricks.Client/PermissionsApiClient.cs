using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
        public class PermissionsApiClient : ApiClient, IPermissionsApi
        {
                public PermissionsApiClient(HttpClient httpClient) : base(httpClient)
                {
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetClusterPermissionLevels(string clusterId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/clusters/{clusterId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetClusterPermissions(string clusterId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/clusters/{clusterId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetDirectoryPermissionLevels(string directoryId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/directories/{directoryId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetDirectoryPermissions(string directoryId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/directories/{directoryId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetExperimentPermissionLevels(string experimentId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/experiments/{experimentId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetExperimentPermissions(string experimentId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/experiments/{experimentId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetInstancePoolPermissionLevels(string instancePoolId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/instance-pools/{instancePoolId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetInstancePoolPermissions(string instancePoolId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/instance-pools/{instancePoolId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetJobPermissionLevels(string jobId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/jobs/{jobId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetJobPermissions(string jobId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/jobs/{jobId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetNotebookPermissionLevels(string notebookId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/notebooks/{notebookId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetNotebookPermissions(string notebookId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/notebooks/{notebookId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetPipelinePermissionLevels(string pipelineId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/pipelines/{pipelineId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetPipelinePermissions(string pipelineId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/pipelines/{pipelineId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetRegisteredModelPermissionLevels(string registeredModelId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/registered-models/{registeredModelId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetRegisteredModelPermissions(string registeredModelId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/registered-models/{registeredModelId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetRepoPermissionLevels(string repoId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/repos/{repoId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetRepoPermissions(string repoId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/repos/{repoId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetSqlWarehousePermissionLevels(string endpointId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/sql/endpoints/{endpointId}/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetSqlWarehousePermissions(string endpointId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/sql/endpoints/{endpointId}";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<AclPermissionItem>> GetTokenPermissions(CancellationToken cancellationToken = default)
                {
                        const string requestUri = "permissions/authorization/tokens";
                        return await HttpGet<AclPermissionItem[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task<IEnumerable<(PermissionLevel, string description)>> GetTokenPermissionsLevels(CancellationToken cancellationToken = default)
                {
                        const string requestUri = "permissions/authorization/tokens/permissionLevels";
                        return await HttpGet<(PermissionLevel, string)[]>(HttpClient, requestUri, cancellationToken);
                }

                public async Task ReplaceClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/clusters/{clusterId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/directories/{directoryId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/experiments/{experimentId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/instance-pools/{instancePoolId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/jobs/{jobId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/notebooks/{notebookId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplacePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/pipelines/{pipelineId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/registered-models/{registeredModelId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/repos/{repoId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/sql/endpoints/{endpointId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task ReplaceTokenPermissionsForWorkspace(IEnumerable<AclPermissionItem> AccessControlList, CancellationToken cancellationToken = default)
                {
                        const string requestUri = "permissions/authorization/tokens";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPut(HttpClient, requestUri, body, cancellationToken);
                }

                public async Task UpdateClusterPermissions(IEnumerable<AclPermissionItem> AccessControlList, string clusterId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/clusters/{clusterId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateDirectoryPermissions(IEnumerable<AclPermissionItem> AccessControlList, string directoryId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/directories/{directoryId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateExperimentPermissions(IEnumerable<AclPermissionItem> AccessControlList, string experimentId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/experiments/{experimentId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateInstancePoolPermissions(IEnumerable<AclPermissionItem> AccessControlList, string instancePoolId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/instance-pools/{instancePoolId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateJobPermissions(IEnumerable<AclPermissionItem> AccessControlList, string jobId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/jobs/{jobId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateNotebookPermissions(IEnumerable<AclPermissionItem> AccessControlList, string notebookId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/notebooks/{notebookId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdatePipelinePermissions(IEnumerable<AclPermissionItem> AccessControlList, string pipelineId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/pipelines/{pipelineId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateRegisteredModelPermissions(IEnumerable<AclPermissionItem> AccessControlList, string registeredModelId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/registered-models/{registeredModelId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateRepoPermissions(IEnumerable<AclPermissionItem> AccessControlList, string repoId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/repos/{repoId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateSqlWarehousePermissions(IEnumerable<AclPermissionItem> AccessControlList, string endpointId, CancellationToken cancellationToken = default)
                {
                        var requestUri = $"permissions/sql/endpoints/{endpointId}";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }

                public async Task UpdateTokenPermissions(IEnumerable<AclPermissionItem> AccessControlList, CancellationToken cancellationToken = default)
                {
                        const string requestUri = "permissions/authorization/tokens";
                        var body = new {access_control_list = AccessControlList};
                        await HttpPatch(HttpClient, requestUri, body);
                }
        }
}
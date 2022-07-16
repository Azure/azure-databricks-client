using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client;

public class PermissionsApiClient : ApiClient, IPermissionsApi
{
    public PermissionsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    private async Task<IEnumerable<(PermissionLevel, string)>> GetPermissionLevels(string apiUri,
        CancellationToken cancellationToken = default)
    {
        var result = await HttpGet<JsonObject>(HttpClient, apiUri, cancellationToken).ConfigureAwait(false);

        result.TryGetPropertyValue("permission_levels", out var permissionLevelsNode);

        return from node in permissionLevelsNode!.AsArray()
            let permLevel = node!["permission_level"]!.Deserialize<PermissionLevel>(Options)
            let desc = node!["description"]?.GetValue<string>() ?? string.Empty
            select (permLevel, desc);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetClusterPermissionLevels(string clusterId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/clusters/{clusterId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetClusterPermissions(string clusterId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/clusters/{clusterId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetDirectoryPermissionLevels(string directoryId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/directories/{directoryId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetDirectoryPermissions(string directoryId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/directories/{directoryId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetExperimentPermissionLevels(string experimentId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/experiments/{experimentId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetExperimentPermissions(string experimentId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/experiments/{experimentId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetInstancePoolPermissionLevels(string instancePoolId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/instance-pools/{instancePoolId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetInstancePoolPermissions(string instancePoolId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/instance-pools/{instancePoolId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetJobPermissionLevels(string jobId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/jobs/{jobId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetJobPermissions(string jobId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/jobs/{jobId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetNotebookPermissionLevels(string notebookId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/notebooks/{notebookId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetNotebookPermissions(string notebookId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/notebooks/{notebookId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetPipelinePermissionLevels(string pipelineId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/pipelines/{pipelineId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetPipelinePermissions(string pipelineId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/pipelines/{pipelineId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetRegisteredModelPermissionLevels(
        string registeredModelId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/registered-models/{registeredModelId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetRegisteredModelPermissions(string registeredModelId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/registered-models/{registeredModelId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetRepoPermissionLevels(string repoId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/repos/{repoId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetRepoPermissions(string repoId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/repos/{repoId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetSqlWarehousePermissionLevels(string endpointId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/sql/endpoints/{endpointId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetSqlWarehousePermissions(string endpointId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/sql/endpoints/{endpointId}";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetTokenPermissionLevels(
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/authorization/tokens/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetTokenPermissions(
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/authorization/tokens";
        var result = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
            select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    public async Task ReplaceClusterPermissions(IEnumerable<AclPermissionItem> accessControlList, string clusterId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/clusters/{clusterId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceDirectoryPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string directoryId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/directories/{directoryId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceExperimentPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string experimentId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/experiments/{experimentId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceInstancePoolPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string instancePoolId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/instance-pools/{instancePoolId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceJobPermissions(IEnumerable<AclPermissionItem> accessControlList, string jobId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/jobs/{jobId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceNotebookPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string notebookId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/notebooks/{notebookId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplacePipelinePermissions(IEnumerable<AclPermissionItem> accessControlList,
        string pipelineId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/pipelines/{pipelineId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceRegisteredModelPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string registeredModelId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/registered-models/{registeredModelId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceRepoPermissions(IEnumerable<AclPermissionItem> accessControlList, string repoId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/repos/{repoId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceSqlWarehousePermissions(IEnumerable<AclPermissionItem> accessControlList,
        string endpointId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/sql/endpoints/{endpointId}";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task ReplaceTokenPermissionsForWorkspace(IEnumerable<AclPermissionItem> accessControlList,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/authorization/tokens";
        var body = new {access_control_list = accessControlList};
        await HttpPut(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateClusterPermissions(IEnumerable<AclPermissionItem> accessControlList, string clusterId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/clusters/{clusterId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateDirectoryPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string directoryId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/directories/{directoryId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateExperimentPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string experimentId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/experiments/{experimentId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateInstancePoolPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string instancePoolId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/instance-pools/{instancePoolId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateJobPermissions(IEnumerable<AclPermissionItem> accessControlList, string jobId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/jobs/{jobId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateNotebookPermissions(IEnumerable<AclPermissionItem> accessControlList, string notebookId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/notebooks/{notebookId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdatePipelinePermissions(IEnumerable<AclPermissionItem> accessControlList, string pipelineId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/pipelines/{pipelineId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateRegisteredModelPermissions(IEnumerable<AclPermissionItem> accessControlList,
        string registeredModelId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/registered-models/{registeredModelId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateRepoPermissions(IEnumerable<AclPermissionItem> accessControlList, string repoId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/repos/{repoId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateSqlWarehousePermissions(IEnumerable<AclPermissionItem> accessControlList,
        string endpointId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/sql/endpoints/{endpointId}";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }

    public async Task UpdateTokenPermissions(IEnumerable<AclPermissionItem> accessControlList,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/authorization/tokens";
        var body = new {access_control_list = accessControlList};
        await HttpPatch(HttpClient, requestUri, body, cancellationToken);
    }
}
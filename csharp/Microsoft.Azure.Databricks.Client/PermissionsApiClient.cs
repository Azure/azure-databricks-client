// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

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

    private async Task<IEnumerable<AclPermissionItem>> GetPermissions(string apiUri,
        CancellationToken cancellationToken = default)
    {
        var result = await HttpGet<JsonObject>(HttpClient, apiUri, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
               select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    private async Task<IEnumerable<AclPermissionItem>> PutPermissions<TBody>(string apiUri, TBody requestBody,
        CancellationToken cancellationToken)
    {
        var result = await HttpPut<TBody, JsonObject>(HttpClient, apiUri, requestBody, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
               select aclItemNode.Deserialize<AclPermissionItem>(Options);
    }

    private async Task<IEnumerable<AclPermissionItem>> PatchPermissions<TBody>(string apiUri, TBody requestBody,
        CancellationToken cancellationToken)
    {
        var result = await HttpPatch<TBody, JsonObject>(HttpClient, apiUri, requestBody, cancellationToken);
        return from aclItemNode in result["access_control_list"]!.AsArray()
               select aclItemNode.Deserialize<AclPermissionItem>(Options);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<(PermissionLevel, string)>> GetClusterPolicyPermissionLevels(string policyId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/cluster-policies/{policyId}/permissionLevels";
        return await GetPermissionLevels(requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> GetClusterPolicyPermissions(string policyId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/cluster-policies/{policyId}";
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
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
        return await GetPermissions(requestUri, cancellationToken).ConfigureAwait(false);
    }



    public async Task<IEnumerable<AclPermissionItem>> ReplaceClusterPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string clusterId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/clusters/{clusterId}";
        var body = new { access_control_list = accessControlList };

        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceClusterPolicyPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string policyId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/cluster-policies/{policyId}";
        var body = new { access_control_list = accessControlList };

        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceDirectoryPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string directoryId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/directories/{directoryId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceExperimentPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string experimentId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/experiments/{experimentId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceInstancePoolPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string instancePoolId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/instance-pools/{instancePoolId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceJobPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string jobId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/jobs/{jobId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceNotebookPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string notebookId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/notebooks/{notebookId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplacePipelinePermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string pipelineId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/pipelines/{pipelineId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceRegisteredModelPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string registeredModelId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/registered-models/{registeredModelId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceRepoPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string repoId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/repos/{repoId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }


    public async Task<IEnumerable<AclPermissionItem>> ReplaceSqlWarehousePermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string endpointId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/sql/endpoints/{endpointId}";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> ReplaceTokenPermissionsForWorkspace(
        IEnumerable<AclPermissionItem> accessControlList,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/authorization/tokens";
        var body = new { access_control_list = accessControlList };
        return await PutPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateClusterPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string clusterId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/clusters/{clusterId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateClusterPolicyPermissions(IEnumerable<AclPermissionItem> accessControlList, string policyId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/cluster-policies/{policyId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateDirectoryPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string directoryId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/directories/{directoryId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateExperimentPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string experimentId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/experiments/{experimentId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateInstancePoolPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string instancePoolId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/instance-pools/{instancePoolId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateJobPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string jobId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/jobs/{jobId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateNotebookPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string notebookId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/notebooks/{notebookId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdatePipelinePermissions(
        IEnumerable<AclPermissionItem> accessControlList, string pipelineId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/pipelines/{pipelineId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateRegisteredModelPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string registeredModelId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/registered-models/{registeredModelId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateRepoPermissions(
        IEnumerable<AclPermissionItem> accessControlList, string repoId,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/repos/{repoId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateSqlWarehousePermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        string endpointId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/sql/endpoints/{endpointId}";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<AclPermissionItem>> UpdateTokenPermissions(
        IEnumerable<AclPermissionItem> accessControlList,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/permissions/authorization/tokens";
        var body = new { access_control_list = accessControlList };
        return await PatchPermissions(requestUri, body, cancellationToken).ConfigureAwait(false);
    }
}
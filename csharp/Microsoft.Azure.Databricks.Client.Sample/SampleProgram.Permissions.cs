// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    // Replace with an existing delta live pipeline ID to try out the permissions API
    // for a delta live pipeline in your workspace
    private static readonly string DeltaLivePipelineId = null;

    // Replace with an existing experiment ID to try out the permissions API 
    // for an experiment in your workspace
    private static readonly string ExperimentId = null;

    // Replace with an existing registered model ID to try out the permissions API 
    // for a registered model in your workspace
    private static readonly string RegisteredModelId = null;

    // Replace with an existing sql warehouse endpoint ID to try out the permissions API 
    // for a sql warehouse in your workspace
    private static readonly string SqlWareHouseEndpointId = null;

    // Replace with an existing repository id to try out the permissions API
    // for a repository in your workspace
    private static readonly string RepositoryId = null;

    private static readonly string ClusterPolicyId = null;

    private static async Task TestPermissionsApi(DatabricksClient client)
    {
        await DirectoryPermissions(client);
        await TokenPermissions(client);
        await ClusterPermissions(client);
        await PoolPermissions(client);
        await JobPermissions(client);
        await PipelinePermissions(client);
        await NotebookPermissions(client);
        await ExperimentsPermissions(client);
        await RegisteredModelsPermissions(client);
        await SqlWarehousePermissions(client);
        await RepoPermissions(client);
        await ClusterPolicyPermissions(client);
    }

    private static async Task TestPermissions(string resourceId, string principal,
        Func<string, CancellationToken, Task<IEnumerable<(PermissionLevel, string)>>> funcGetPermissionLevels,
        Func<string, CancellationToken, Task<IEnumerable<AclPermissionItem>>> funcGetPermissions,
        Func<IEnumerable<AclPermissionItem>, string, CancellationToken, Task> funcUpdatePermissions,
        Func<IEnumerable<AclPermissionItem>, string, CancellationToken, Task> funcReplacePermissions
    )
    {
        if (resourceId == null)
            return;

        Console.WriteLine($"Getting and displaying the allowable permission levels for resource {resourceId}");

        var permissionLevels = (await funcGetPermissionLevels(resourceId, default)).ToList();

        foreach (var (permissionLevel, description) in permissionLevels)
        {
            Console.WriteLine($"{permissionLevel}: {description}");
        }

        var allowedLevels = (from p in permissionLevels
                             select p.Item1).ToList();

        Console.WriteLine($"Getting and displaying current access levels for resource {resourceId}");

        var currentAclItems = (await funcGetPermissions(resourceId, default)).ToList();

        foreach (var aclItem in currentAclItems)
        {
            Console.WriteLine($"Principal: {aclItem.Principal}, Permission Level: {aclItem.PermissionLevel}");
        }

        var aclItems = from level in allowedLevels
                       select new UserAclItem { Principal = principal, PermissionLevel = level };

        foreach (var aclItem in aclItems.Where(item => item.PermissionLevel != PermissionLevel.IS_OWNER))
        {
            Console.WriteLine(
                $"Updating permissions for principal {aclItem.Principal}, permission level {aclItem.PermissionLevel}"
            );

            await funcUpdatePermissions(new[] { aclItem }, resourceId, default);
        }

        Console.WriteLine("Resetting user permissions");
        await funcReplacePermissions(currentAclItems, resourceId, default);
    }

    private static async Task DirectoryPermissions(DatabricksClient client)
    {
        Console.WriteLine("Creating a new workspace...");
        await client.Workspace.Mkdirs(SampleWorkspacePath);

        var dirInfo = await client.Workspace.GetStatus(SampleWorkspacePath);

        await TestPermissions(
            dirInfo.ObjectId.ToString(),
            DatabricksUserName,
            client.Permissions.GetDirectoryPermissionLevels,
            client.Permissions.GetDirectoryPermissions,
            client.Permissions.UpdateDirectoryPermissions,
            client.Permissions.ReplaceDirectoryPermissions
        );

        await client.Workspace.Delete(SampleWorkspacePath, true);
        Console.WriteLine("Sample workspace removed");
    }

    private static async Task TokenPermissions(DatabricksClient client)
    {
        //only the getters are shown here, since updating these permissions might invalidate 
        //the token that we are currently using to connect in the first place.
        Console.WriteLine("Getting and displaying the allowable permission levels for databricks tokens...");
        var allowablePermissions = await client.Permissions.GetTokenPermissionLevels();

        foreach (var (permissionLevel, description) in allowablePermissions)
        {
            Console.WriteLine($"{permissionLevel}: {description}");
        }

        Console.WriteLine("Getting and displaying current access levels for tokens...");
        var currentAclItems = await client.Permissions.GetTokenPermissions();
        foreach (var aclItem in currentAclItems)
        {
            Console.WriteLine($"Principal: {aclItem.Principal}, Permission Level: {aclItem.PermissionLevel}");
        }
    }

    private static async Task ClusterPermissions(DatabricksClient client)
    {
        Console.WriteLine("Creating standard cluster");

        var clusterConfig = ClusterAttributes.GetNewClusterConfiguration("Sample cluster")
            .WithRuntimeVersion(RuntimeVersions.Runtime_10_5)
            .WithAutoTermination(20)
            .WithNodeType(NodeTypes.Standard_D3_v2)
            .WithClusterMode(ClusterMode.SingleNode);

        var clusterId = await client.Clusters.Create(clusterConfig);

        await TestPermissions(
            clusterId,
            DatabricksUserName,
            client.Permissions.GetClusterPermissionLevels,
            client.Permissions.GetClusterPermissions,
            client.Permissions.UpdateClusterPermissions,
            client.Permissions.ReplaceClusterPermissions
        );

        Console.WriteLine("Deleting Sample cluster");
        await client.Clusters.Delete(clusterId);
    }

    private static async Task PoolPermissions(DatabricksClient client)
    {
        Console.WriteLine("Creating Testing Instance Pool");
        var poolAttributes = new InstancePoolAttributes
        {
            PoolName = "Sample pool",
            PreloadedSparkVersions = new[] { RuntimeVersions.Runtime_10_5 },
            MinIdleInstances = 2,
            MaxCapacity = 100,
            IdleInstanceAutoTerminationMinutes = 15,
            NodeTypeId = NodeTypes.Standard_D3_v2,
            EnableElasticDisk = true,
            DiskSpec = new DiskSpec
            { DiskCount = 2, DiskSize = 64, DiskType = DiskType.FromAzureDisk(AzureDiskVolumeType.STANDARD_LRS) },
            AzureAttributes = new InstancePoolAzureAttributes
            { Availability = AzureAvailability.SPOT_AZURE, SpotBidMaxPrice = -1 }
        };

        var poolId = await client.InstancePool.Create(poolAttributes).ConfigureAwait(false);

        await TestPermissions(
            poolId,
            DatabricksUserName,
            client.Permissions.GetInstancePoolPermissionLevels,
            client.Permissions.GetInstancePoolPermissions,
            client.Permissions.UpdateInstancePoolPermissions,
            client.Permissions.ReplaceInstancePoolPermissions
        );

        Console.WriteLine("Deleting Sample pool");
        await client.InstancePool.Delete(poolId);
    }

    private static async Task JobPermissions(DatabricksClient client)
    {
        Console.WriteLine("Creating new job");
        Console.WriteLine($"Creating workspace {SampleWorkspacePath}");
        await client.Workspace.Mkdirs(SampleWorkspacePath);

        Console.WriteLine("Downloading sample notebook");
        var content = await DownloadSampleNotebook();

        Console.WriteLine($"Importing sample HTML notebook to {SampleNotebookPath}");
        await client.Workspace.Import(SampleNotebookPath, ExportFormat.HTML, null,
            content, true);

        var newCluster = ClusterAttributes.GetNewClusterConfiguration()
            .WithClusterMode(ClusterMode.SingleNode)
            .WithNodeType(NodeTypes.Standard_D3_v2)
            .WithRuntimeVersion(RuntimeVersions.Runtime_10_4);

        var jobSettings = new JobSettings { MaxConcurrentRuns = 1, Name = "Sample Job" };
        jobSettings.AddTask("task1", new NotebookTask { NotebookPath = SampleNotebookPath })
            .WithDescription("Sample Job - task1")
            .WithNewCluster(newCluster);

        Console.WriteLine("Creating new job");
        var jobId = await client.Jobs.Create(jobSettings);

        Console.WriteLine("Job created: {0}", jobId);

        await TestPermissions(
            jobId.ToString(),
            DatabricksUserName,
            client.Permissions.GetJobPermissionLevels,
            client.Permissions.GetJobPermissions,
            client.Permissions.UpdateJobPermissions,
            client.Permissions.ReplaceJobPermissions
        );

        await client.Jobs.Delete(jobId);
        await client.Workspace.Delete(SampleNotebookPath, true);
    }

    private static async Task PipelinePermissions(DatabricksClient client)
    {
        await TestPermissions(
            DeltaLivePipelineId,
            DatabricksUserName,
            client.Permissions.GetPipelinePermissionLevels,
            client.Permissions.GetPipelinePermissions,
            client.Permissions.UpdatePipelinePermissions,
            client.Permissions.ReplacePipelinePermissions
        );
    }

    private static async Task NotebookPermissions(DatabricksClient client)
    {
        Console.WriteLine($"Creating workspace {SampleWorkspacePath}");
        await client.Workspace.Mkdirs(SampleWorkspacePath);

        Console.WriteLine("Downloading sample notebook");
        var content = await DownloadSampleNotebook();

        Console.WriteLine($"Importing sample HTML notebook to {SampleNotebookPath}");
        await client.Workspace.Import(SampleNotebookPath, ExportFormat.HTML, null,
            content, true);
        var dirInfo = await client.Workspace.GetStatus(SampleNotebookPath);
        var notebookId = dirInfo.ObjectId.ToString();

        await TestPermissions(
            notebookId,
            DatabricksUserName,
            client.Permissions.GetNotebookPermissionLevels,
            client.Permissions.GetNotebookPermissions,
            client.Permissions.UpdateNotebookPermissions,
            client.Permissions.ReplaceNotebookPermissions
        );

        Console.WriteLine("Deleting sample workspace");
        await client.Workspace.Delete(SampleWorkspacePath, true);
    }

    private static async Task ExperimentsPermissions(DatabricksClient client)
    {
        await TestPermissions(
            ExperimentId,
            DatabricksUserName,
            client.Permissions.GetExperimentPermissionLevels,
            client.Permissions.GetExperimentPermissions,
            client.Permissions.UpdateExperimentPermissions,
            client.Permissions.ReplaceExperimentPermissions
        );
    }

    private static async Task RegisteredModelsPermissions(DatabricksClient client)
    {
        await TestPermissions(
            RegisteredModelId,
            DatabricksUserName,
            client.Permissions.GetRegisteredModelPermissionLevels,
            client.Permissions.GetRegisteredModelPermissions,
            client.Permissions.UpdateRegisteredModelPermissions,
            client.Permissions.ReplaceRegisteredModelPermissions
        );
    }

    private static async Task SqlWarehousePermissions(DatabricksClient client)
    {
        await TestPermissions(
            SqlWareHouseEndpointId,
            DatabricksUserName,
            client.Permissions.GetSqlWarehousePermissionLevels,
            client.Permissions.GetSqlWarehousePermissions,
            client.Permissions.UpdateSqlWarehousePermissions,
            client.Permissions.ReplaceSqlWarehousePermissions
        );
    }

    private static async Task RepoPermissions(DatabricksClient client)
    {
        await TestPermissions(
            RepositoryId,
            DatabricksUserName,
            client.Permissions.GetRepoPermissionLevels,
            client.Permissions.GetRepoPermissions,
            client.Permissions.UpdateRepoPermissions,
            client.Permissions.ReplaceRepoPermissions
        );
    }

    private static async Task ClusterPolicyPermissions(DatabricksClient client)
    {
        await TestPermissions(
            ClusterPolicyId,
            DatabricksUserName,
            client.Permissions.GetClusterPolicyPermissionLevels,
            client.Permissions.GetClusterPolicyPermissions,
            client.Permissions.UpdateClusterPolicyPermissions,
            client.Permissions.ReplaceClusterPolicyPermissions
        );
    }
}
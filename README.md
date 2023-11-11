# Azure Databricks Client Library

----------
[![Build Status](https://dev.azure.com/microsoft/Data%20Science/_apis/build/status/azure-databricks-client-2.0?branchName=master)](https://dev.azure.com/microsoft/Data%20Science/_build/latest?definitionId=87621&branchName=master)
[![CodeQL](https://github.com/Azure/azure-databricks-client/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/Azure/azure-databricks-client/actions/workflows/codeql-analysis.yml)
[![Linter](https://github.com/Azure/azure-databricks-client/actions/workflows/linter.yml/badge.svg)](https://github.com/Azure/azure-databricks-client/actions/workflows/linter.yml)
[![NuGet](https://img.shields.io/badge/nuget-blue.svg)](https://www.nuget.org/packages/Microsoft.Azure.Databricks.Client/)
[![Version 1.1 ()](https://img.shields.io/badge/1.1%20release-informational.svg)](https://github.com/Azure/azure-databricks-client/tree/releases/1.1)

The Azure Databricks Client Library offers a convenient interface for automating your Azure Databricks workspace through Azure Databricks REST API.

The implementation of this library is based on [REST API version 2.0 and above](https://docs.azuredatabricks.net/api/latest/index.html#).

> The master branch is for version 2. Version 1.1 (stable) is in the [releases/1.1](https://github.com/Azure/azure-databricks-client/tree/releases/1.1) branch.

## Requirements

You must have personal access tokens (PAT) or Azure Active Directory tokens (AAD Token) to access the databricks REST API.

- To generate a PAT, follow the steps listed in [this document](https://learn.microsoft.com/en-us/azure/databricks/dev-tools/auth#--azure-databricks-personal-access-token-authentication).
- To generate a AAD token, follow the steps listed in [this document](https://learn.microsoft.com/en-us/azure/databricks/dev-tools/service-prin-aad-token).

## Supported APIs

| REST API                                                                                   | Version | Description                                                                                                                                                                                                           |
|--------------------------------------------------------------------------------------------|---------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| [Clusters](https://docs.databricks.com/api/azure/workspace/clusters)                       | 2.0     | The Clusters API allows you to create, start, edit, list, terminate, and delete clusters.                                                                                                                             |
| [Jobs](https://docs.databricks.com/api/azure/workspace/jobs)                               | 2.1     | The Jobs API allows you to programmatically manage Azure Databricks jobs.                                                                                                                                             |
| [Dbfs](https://docs.databricks.com/api/azure/workspace/dbfs)                               | 2.0     | The DBFS API is a Databricks API that makes it simple to interact with various data sources without having to include your credentials every time you read a file.                                                    |
| [Secrets](https://docs.databricks.com/api/azure/workspace/secrets)                         | 2.0     | The Secrets API allows you to manage secrets, secret scopes, and access permissions.                                                                                                                                  |
| [Groups](https://docs.databricks.com/api/azure/workspace/groups)                           | 2.0     | The Groups API allows you to manage groups of users.                                                                                                                                                                  |
| [Libraries](https://docs.databricks.com/api/azure/workspace/libraries)                     | 2.0     | The Libraries API allows you to install and uninstall libraries and get the status of libraries on a cluster.                                                                                                         |
| [Token](https://docs.databricks.com/api/azure/workspace/tokens)                            | 2.0     | The Token API allows you to create, list, and revoke tokens that can be used to authenticate and access Azure Databricks REST APIs.                                                                                   |
| [Workspace](https://docs.databricks.com/api/azure/workspace/workspace)                     | 2.0     | The Workspace API allows you to list, import, export, and delete notebooks and folders.                                                                                                                               |
| [InstancePool](https://docs.databricks.com/api/azure/workspace/instancepools)              | 2.0     | The Instance Pools API allows you to create, edit, delete and list instance pools.                                                                                                                                    |
| [Permissions](https://docs.databricks.com/api/azure/workspace/permissions)                 | 2.0     | The Permissions API lets you manage permissions for Token, Cluster, Pool, Job, Delta Live Tables pipeline, Notebook, Directory, MLflow experiment, MLflow registered model, SQL warehouse, Repo and Cluster Policies. |
| [Cluster Policies](https://docs.databricks.com/api/azure/workspace/clusterpolicies)        | 2.0     | The Cluster Policies API allows you to create, list, and edit cluster policies.                                                                                                                                       |
| [Global Init Scripts](https://docs.databricks.com/api/azure/workspace/globalinitscripts)   | 2.0     | The Global Init Scripts API lets Azure Databricks administrators add global cluster initialization scripts in a secure and controlled manner.                                                                         |
| [SQL Warehouses](https://docs.databricks.com/api/azure/workspace/warehouses)               | 2.0     | The SQL Warehouses API allows you to manage compute resources that lets you run SQL commands on data objects within Databricks SQL.                                                                                   |
| [Repos](https://docs.databricks.com/api/azure/workspace/repos)                             | 2.0     | The Repos API allows users to manage their git repos. Users can use the API to access all repos that they have manage permissions on.                                                                                 |
| [Pipelines (Delta Live Tables)](https://docs.databricks.com/api/azure/workspace/pipelines) | 2.0     | The Delta Live Tables API allows you to create, edit, delete, start, and view details about pipelines.                                                                                                                |

## Usage

Check out the Sample project for more detailed usages.

In the following examples, the `baseUrl` variable should be set to the workspace base URL, which looks like `https://adb-<workspace-id>.<random-number>.azuredatabricks.net`, and `token` variable should be set to your Databricks personal access token.

### Creating client

```cs
using (var client = DatabricksClient.CreateClient(baseUrl, token))
{
    // ...
}

```

### Cluster API

- Create a single node cluster:

```cs
var clusterConfig = ClusterAttributes
            .GetNewClusterConfiguration("Sample cluster")
            .WithRuntimeVersion(RuntimeVersions.Runtime_10_4)
            .WithAutoScale(3, 7)
            .WithAutoTermination(30)
            .WithClusterLogConf("dbfs:/logs/")
            .WithNodeType(NodeTypes.Standard_D3_v2)
            .WithClusterMode(ClusterMode.SingleNode);

var clusterId = await client.Clusters.Create(clusterConfig);
```

- Wait for the cluster to be ready (or fail to start):

```cs
using Policy = Polly.Policy;

static async Task WaitForCluster(IClustersApi clusterClient, string clusterId, int pollIntervalSeconds = 15)
{
    var retryPolicy = Policy.Handle<WebException>()
        .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.BadGateway)
        .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.InternalServerError)
        .Or<ClientApiException>(e => e.Message.Contains("\"error_code\":\"TEMPORARILY_UNAVAILABLE\""))
        .Or<TaskCanceledException>(e => !e.CancellationToken.IsCancellationRequested)
        .OrResult<ClusterInfo>(info => info.State is not (ClusterState.RUNNING or ClusterState.ERROR or ClusterState.TERMINATED))
        .WaitAndRetryForeverAsync(
            _ => TimeSpan.FromSeconds(pollIntervalSeconds),
            (delegateResult, _) =>
            {
                if (delegateResult.Exception != null)
                {
                    Console.WriteLine($"[{DateTime.UtcNow:s}] Failed to query cluster info - {delegateResult.Exception}");
                }
            });
    await retryPolicy.ExecuteAsync(async () =>
    {
        var info = await clusterClient.Get(clusterId);
        Console.WriteLine($"[{DateTime.UtcNow:s}] Cluster:{clusterId}\tState:{info.State}\tMessage:{info.StateMessage}");
        return info;
    });
}

await WaitForCluster(client.Clusters, clusterId);

```

- Stop a cluster:

```cs
await client.Clusters.Terminate(clusterId);
await WaitForCluster(client.Clusters, clusterId);
```

- Delete a cluster:

```cs
await client.Clusters.Delete(clusterId);
```

### Jobs API

- Create a job:

```cs
// Job schedule
var schedule = new CronSchedule
{
    QuartzCronExpression = "0 0 9 ? * MON-FRI",
    TimezoneId = "Europe/London",
    PauseStatus = PauseStatus.UNPAUSED
};

// Run with a job cluster
var newCluster = ClusterAttributes.GetNewClusterConfiguration()
    .WithClusterMode(ClusterMode.SingleNode)
    .WithNodeType(NodeTypes.Standard_D3_v2)
    .WithRuntimeVersion(RuntimeVersions.Runtime_10_4);

// Create job settings
var jobSettings = new JobSettings
{
    MaxConcurrentRuns = 1,
    Schedule = schedule,
    Name = "Sample Job"
};

// Adding 3 tasks to the job settings.
var task1 = jobSettings.AddTask("task1", new NotebookTask { NotebookPath = SampleNotebookPath })
    .WithDescription("Sample Job - task1")
    .WithNewCluster(newCluster);
var task2 = jobSettings.AddTask("task2", new NotebookTask { NotebookPath = SampleNotebookPath })
    .WithDescription("Sample Job - task2")
    .WithNewCluster(newCluster);
jobSettings.AddTask("task3", new NotebookTask { NotebookPath = SampleNotebookPath }, new[] { task1, task2 })
    .WithDescription("Sample Job - task3")
    .WithNewCluster(newCluster);

// Create the job.
Console.WriteLine("Creating new job");
var jobId = await client.Jobs.Create(jobSettings);
Console.WriteLine("Job created: {0}", jobId);
```

- Start a job run

```cs
// Start the job and retrieve the run id.
Console.WriteLine("Run now: {0}", jobId);
var runId = await client.Jobs.RunNow(jobId);
```

- Wait for a job run to complete

```cs
using Policy = Polly.Policy;

static async Task WaitForRun(IJobsApi jobClient, long runId, int pollIntervalSeconds = 15)
{
    var retryPolicy = Policy.Handle<WebException>()
        .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.BadGateway)
        .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.InternalServerError)
        .Or<ClientApiException>(e => e.Message.Contains("\"error_code\":\"TEMPORARILY_UNAVAILABLE\""))
        .Or<TaskCanceledException>(e => !e.CancellationToken.IsCancellationRequested)
        .OrResult<RunState>(state =>
            state.LifeCycleState is RunLifeCycleState.PENDING or RunLifeCycleState.RUNNING
                or RunLifeCycleState.TERMINATING)
        .WaitAndRetryForeverAsync(
            _ => TimeSpan.FromSeconds(pollIntervalSeconds),
            (delegateResult, _) =>
            {
                if (delegateResult.Exception != null)
                {
                    Console.WriteLine(
                        $"[{DateTime.UtcNow:s}] Failed to query run - {delegateResult.Exception}");
                }
            });
    await retryPolicy.ExecuteAsync(async () =>
    {
        var (run, _) = await jobClient.RunsGet(runId);
        Console.WriteLine(
            $"[{DateTime.UtcNow:s}] Run:{runId}\tLifeCycleState:{run.State.LifeCycleState}\tResultState:{run.State.ResultState}\tCompleted:{run.IsCompleted}"
        );
        return run.State;
    });
}

await WaitForRun(client.Jobs, runId);
```

- Export a job run

```cs
var (run, _) = await client.Jobs.RunsGet(runId);
foreach (var runTask in run.Tasks)
{
    var viewItems = await client.Jobs.RunsExport(runTask.RunId);
    foreach (var viewItem in viewItems)
    {
        Console.WriteLine($"Exported view item from run {runTask.RunId}, task \"{runTask.TaskKey}\", view \"{viewItem.Name}\"");
        Console.WriteLine("====================");
        Console.WriteLine(viewItem.Content[..200] + "...");
        Console.WriteLine("====================");
    }
}
```

### Secrets API

Creating secret scope

```cs
const string scope = "SampleScope";
await client.Secrets.CreateScope(scope, null);
```

Create text secret

```cs
var secretName = "secretkey.text";
await client.Secrets.PutSecret("secret text", scope, secretName);
```

Create binary secret

```cs
var secretName = "secretkey.bin";
await client.Secrets.PutSecret(new byte[]{0x01, 0x02, 0x03, 0x04}, scope, secretName);
```

### Resiliency

The `clusters/create`, `jobs/run-now` and `jobs/runs/submit` APIs support idempotency token. It is optional token to guarantee the idempotency of requests. If a resource (a cluster or a run) with the provided token already exists, the request does not create a new resource but returns the ID of the existing resource instead.

If you specify the idempotency token, upon failure you can retry until the request succeeds. Databricks guarantees that exactly one resource is launched with that idempotency token.

The following code illustrates how to use [Polly](https://github.com/App-vNext/Polly) to retry the request with `idempotency_token` if the request fails.

```cs
using Polly;

double retryIntervalSec = 15;
string idempotencyToken = Guid.NewGuid().ToString();

var clusterInfo = ClusterAttributes.GetNewClusterConfiguration("my-cluster")
    .WithNodeType("Standard_D3_v2")
    .WithNumberOfWorkers(25)
    .WithRuntimeVersion(RuntimeVersions.Runtime_7_3);

var retryPolicy = Policy.Handle<WebException>()
    .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.BadGateway)
    .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.InternalServerError)
    .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.ServiceUnavailable)
    .Or<ClientApiException>(e => e.Message.Contains("\"error_code\":\"TEMPORARILY_UNAVAILABLE\""))
    .Or<TaskCanceledException>(e => !e.CancellationToken.IsCancellationRequested)
    .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(retryIntervalSec));

var clusterId = await retryPolicy.ExecuteAsync(async () => await client.Clusters.Create(clusterInfo, idempotencyToken));

```

## Breaking changes

- The v2 of the library targets .NET 6 runtime.

- The Jobs API was redesigned to align with the version 2.1 of the REST API.

  - In the previous version, the Jobs API only supports single task per job. The new Jobs API supports multiple tasks per job, where the tasks are represented as a DAG.

  - The new version supports two more types of task: Python Wheel task and Delta Live Tables pipeline task.

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit [Microsoft Contributor License Agreement (CLA)](https://cla.microsoft.com).

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

# Azure Databricks Client Library

----------

[![NuGet version ()](https://img.shields.io/badge/nuget-1.0.810.3-blue.svg)](https://www.nuget.org/packages/Microsoft.Azure.Databricks.Client/)

The Azure Databricks Client Library allows you to automate your Azure Databricks environment through Azure Databricks REST Api.

The implementation of this library is based on [REST Api version 2.0](https://docs.azuredatabricks.net/api/latest/index.html#).  

## Usage

Check out the Sample project for more detailed usages.

### Creating client

```cs
using (var client = DatabricksClient.CreateClient(baseUrl, token))
    {
        // ...
    }

```

### Cluster API

Create a standard cluster

```cs
var clusterConfig = ClusterInfo.GetNewClusterConfiguration("Sample cluster")
    .WithRuntimeVersion(RuntimeVersions.Runtime_4_2_Scala_2_11)
    .WithAutoScale(3, 7)
    .WithAutoTermination(30)
    .WithClusterLogConf("dbfs:/logs/")
    .WithNodeType(NodeTypes.Standard_D3_v2)
    .WithPython3(true);

var clusterId = await client.Clusters.Create(clusterConfig);
```

Delete a cluster

```cs
await client.Clusters.Delete(clusterId);
```

### Jobs API

```cs
// New cluster config
var newCluster = ClusterInfo.GetNewClusterConfiguration()
    .WithNumberOfWorkers(3)
    .WithPython3(true)
    .WithNodeType(NodeTypes.Standard_D3_v2)
    .WithRuntimeVersion(RuntimeVersions.Runtime_4_2_Scala_2_11);

// Create job settings
var jobSettings = JobSettings.GetNewNotebookJobSettings(
        "Sample Job",
        SampleNotebookPath,
        null)
    .WithNewCluster(newCluster);

// Create new job
var jobId = await client.Jobs.Create(jobSettings);

// Start the job and retrieve the run id.
var runId = (await client.Jobs.RunNow(jobId, null)).RunId;

// Keep polling the run by calling RunsGet until run terminates:
//  var run = await client.Jobs.RunsGet(runId);
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

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

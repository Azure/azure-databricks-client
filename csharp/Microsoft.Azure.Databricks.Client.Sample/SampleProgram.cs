// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Identity;
using Microsoft.Azure.Databricks.Client.Converters;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        Converters = {
            new JsonStringEnumConverter(),
            new MillisecondEpochDateTimeConverter(),
            new LibraryConverter(),
            new SecretScopeConverter(),
            new AclPermissionItemConverter(),
            new DepedencyConverter(),
            new TableConstraintConverter(),
        }
    };

    /// <summary>
    /// Must be an existing user in the databricks environment, otherwise you will get a "DIRECTORY_PROTECTED" error.
    /// </summary>
    private const string DatabricksUserName = "user@company.com";
    private const string SampleWorkspacePath = $"/Users/{DatabricksUserName}/SampleWorkspace";
    private const string SampleNotebookPath = $"{SampleWorkspacePath}/Quick Start Using Scala";

    public static async Task Main(string[] args)
    {
        DatabricksClient client;
        if (args.Length == 0)
        {
            await Console.Error.WriteLineAsync("Usage: <Azure databricks base url>");
            return;
        }

        if (args.Length == 1)
        {
            client = DatabricksClient.CreateClient(args[0], new DefaultAzureCredential());
        }
        else
        {
            var baseUrl = args[0];
            var token = args[1];
            client = DatabricksClient.CreateClient(baseUrl, token);
        }

        Console.WriteLine("Creating client");
        using (client)
        {
            await TestGlobalInitScriptsApi(client);
            await TestClusterPoliciesApi(client);
            await TestWorkspaceApi(client);
            await TestLibrariesApi(client);
            await TestSecretsApi(client);
            await TestTokenApi(client);
            await TestInstancePoolApi(client);
            await TestClustersApi(client);
            await TestGroupsApi(client);
            await TestDbfsApi(client);
            await TestFilesApi(client);
            await TestJobsApi(client);
            await TestPermissionsApi(client);
            await TestWarehouseApi(client);
            await TestReposApi(client);
            await TestPipelineApi(client);
            await TestUnityCatalogApi(client);
            await TestStatementExecutionApi(client);
            await TestExperimentApiClient(client);
        }

        Console.WriteLine("Press enter to exit");
        Console.ReadLine();
    }

    private static async Task<byte[]> DownloadSampleNotebook()
    {
        using var httpClient = new HttpClient();
        var content = await httpClient.GetByteArrayAsync(
            "https://docs.databricks.com/_extras/notebooks/source/mlflow/mlflow-quick-start-scala.html"
        );

        return content;
    }
}

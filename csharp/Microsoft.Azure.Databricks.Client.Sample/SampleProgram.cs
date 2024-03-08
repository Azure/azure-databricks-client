// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        if (args.Length < 2)
        {
            await Console.Error.WriteLineAsync("Usage: <Azure databricks base url> <access token>");
            return;
        }

        var baseUrl = args[0];
        var token = args[1];

        Console.WriteLine("Creating client");
        using (var client = DatabricksClient.CreateClient(baseUrl, token))
        {
            //await TestGlobalInitScriptsApi(client);
            //await TestClusterPoliciesApi(client);
            //await TestWorkspaceApi(client);
            //await TestLibrariesApi(client);
            //await TestSecretsApi(client);
            //await TestTokenApi(client);
            //await TestInstancePoolApi(client);
            //await TestClustersApi(client);
            //await TestGroupsApi(client);
            //await TestDbfsApi(client);
            //await TestJobsApi(client);
            //await TestPermissionsApi(client);
            //await TestWarehouseApi(client);
            //await TestReposApi(client);
            //await TestPipelineApi(client);
            await TestUnityCatalogApi(client);
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

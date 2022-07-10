using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Converters;

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
            new AccessControlRequestConverter()
        }
    };

    /// <summary>
    /// Must be an existing user in the databricks environment, otherwise you will get a "DIRECTORY_PROTECTED" error.
    /// </summary>
    private const string DatabricksUserName = "username@company.com";

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
            await TestWorkspaceApi(client);
            await TestLibrariesApi(client);
            await TestSecretsApi(client);
            await TestTokenApi(client);
            await TestInstancePoolApi(client);
            await TestClustersApi(client);
            await TestGroupsApi(client);
            await TestDbfsApi(client);
            await TestJobsApi(client);

        }

        Console.WriteLine("Press enter to exit");
        Console.ReadLine();
    }

    private static async Task<byte[]> DownloadSampleNotebook()
    {
        using var httpClient = new HttpClient();
        var content = await httpClient.GetByteArrayAsync(
            "https://docs.databricks.com/_static/notebooks/getting-started/quickstartusingscala.html"
        );

        return content;
    }
}
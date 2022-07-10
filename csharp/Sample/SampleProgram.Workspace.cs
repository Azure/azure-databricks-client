using System;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestWorkspaceApi(DatabricksClient client)
    {
        Console.WriteLine($"Creating workspace {SampleWorkspacePath}");
        await client.Workspace.Mkdirs(SampleWorkspacePath);

        Console.WriteLine("Downloading sample notebook");
        var content = await DownloadSampleNotebook();

        Console.WriteLine($"Importing sample HTML notebook to {SampleNotebookPath}");
        await client.Workspace.Import(SampleNotebookPath, ExportFormat.HTML, null,
            content, true);

        Console.WriteLine($"Getting status of sample notebook {SampleNotebookPath}");
        var objectInfo = await client.Workspace.GetStatus(SampleNotebookPath);
        Console.WriteLine($"Object type: {objectInfo.ObjectType}\tObject language: {objectInfo.Language}");

        Console.WriteLine("Listing sample workspace");
        var list = await client.Workspace.List(SampleWorkspacePath);
        foreach (var obj in list)
        {
            Console.WriteLine($"\tPath: {obj.Path}\tType: {obj.ObjectType}\tLanguage: {obj.Language}");
        }

        Console.WriteLine($"Exporting sample notebook in SOURCE format from {SampleNotebookPath}");
        var exported = await client.Workspace.Export(SampleNotebookPath, ExportFormat.SOURCE);
        var exportedString = System.Text.Encoding.ASCII.GetString(exported);
        Console.WriteLine("Exported notebook:");
        Console.WriteLine("====================");
        Console.WriteLine(string.Concat(exportedString.AsSpan(0, 100), "..."));
        Console.WriteLine("====================");

        Console.WriteLine("Deleting sample workspace");
        await client.Workspace.Delete(SampleWorkspacePath, true);
    }
}
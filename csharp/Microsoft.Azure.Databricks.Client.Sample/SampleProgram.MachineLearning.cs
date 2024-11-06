using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestExperimentApiClient(DatabricksClient client)
    {
        string run_id = "sample_run_id";
        PrintDelimiter();
        Console.WriteLine("Listing experiment runs");
        var data = await client.MachineLearning.Experiments.GetRun(run_id);
        Console.WriteLine($"\t{data.Info.RunId}");
        PrintDelimiter();
    }
}
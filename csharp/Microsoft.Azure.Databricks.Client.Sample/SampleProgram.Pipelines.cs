using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static void PrintDelimiter()
    {
        Console.WriteLine("---------------------------");
    }
    private static async Task TestPipelineApi(DatabricksClient client)
    {
        Console.WriteLine("Creating new pipeline");
        var pipelineSpecification = new PipelineSpecification()
        {
            Development = true,
            Clusters = new[]
            {
                new ClusterAttributes()
                {
                    AutoScale = new AutoScale()
                    {
                        MinWorkers = 1,
                        MaxWorkers = 5,
                    }
                }
            },
            Continuous = false,
            Channel = "CURRENT",
            Photon = true,
            Libraries = new[]
            {
                new NotebookLibrary()
                {
                    Notebook = new PathSpec()
                    {
                        Path = SampleNotebookPath
                    }
                }
            },
            Name = "samplePipeline",
            Edition = "CORE"
        };

        var createRes = await client.Pipelines.Create(pipelineSpecification, false, true);
        var pipelineId = createRes.Item1;
        Console.WriteLine($"Created a pipeline of id: {pipelineId}");
        PrintDelimiter();

        Console.WriteLine("Listing pipelines (max 10)");
        var pipelinesList = await client.Pipelines.List(maxResults: 10);
        foreach (var pipeline in pipelinesList.Pipelines)
        {
            Console.WriteLine($"\t{pipeline.Name}\t{pipeline.State}");
        }
        PrintDelimiter();

        Console.WriteLine("Getting created pipeline");
        var createdPipeline = await client.Pipelines.Get(pipelineId);
        Console.WriteLine($"\t{createdPipeline.PipelineId}\t{createdPipeline.Specification.Name}");
        PrintDelimiter();

        Console.WriteLine("Resetting created pipeline");
        await client.Pipelines.Reset(pipelineId);
        PrintDelimiter();

        Console.WriteLine("Stopping a pipeline");
        await client.Pipelines.Stop(pipelineId);
        PrintDelimiter();

        Console.WriteLine("Editting a pipeline - changing its' name");
        pipelineSpecification.Name = "samplePipelineNewName";
        await client.Pipelines.Edit(pipelineId, pipelineSpecification, allowDuplicateNames: true);
        PrintDelimiter();

        Console.WriteLine("Listing created pipeline updates (max 10)");
        var pipelineUpdates = await client.Pipelines.ListUpdates(pipelineId, maxResults: 10);
        foreach (var pipelineUpdate in pipelineUpdates.Updates)
        {
            Console.WriteLine($"\t{pipelineUpdate.UpdateId}\t{pipelineUpdate.State}");
        }
        PrintDelimiter();

        Console.WriteLine("Start pipeline update");
        await client.Pipelines.Stop(pipelineId);
        var pipelineUpdateId = await client.Pipelines.Start(pipelineId);
        PrintDelimiter();

        Console.WriteLine("Getting queued pipeline");
        var queuedPipelineUpdate = await client.Pipelines.GetUpdate(
            pipelineId,
            pipelineUpdateId);
        Console.WriteLine($"\t{queuedPipelineUpdate.UpdateId}\t{queuedPipelineUpdate.State}");
        PrintDelimiter();

        Console.WriteLine("Stopping a pipeline");
        await client.Pipelines.Stop(pipelineId);
        PrintDelimiter();

        Console.WriteLine("Listing pipeline events");
        var pipelineEvents = await client.Pipelines.ListEvents(
            pipelineId,
            maxResults: 10);
        foreach (var pipelineEvent in pipelineEvents.Events)
        {
            Console.WriteLine($"\t{pipelineEvent.Id}");
        }
        PrintDelimiter();

        Console.WriteLine("Deleting create pipeline");
        await client.Pipelines.Delete(pipelineId);

        return;
    }
}
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;
using Polly;

namespace Microsoft.Azure.Databricks.Client.Sample
{
    internal static partial class SampleProgram
    {
        private static async Task TestJobsApi(DatabricksClient client)
        {
            Console.WriteLine($"Creating workspace {SampleWorkspacePath}");
            await client.Workspace.Mkdirs(SampleWorkspacePath);

            Console.WriteLine("Downloading sample notebook");
            var content = await DownloadSampleNotebook();

            Console.WriteLine($"Importing sample HTML notebook to {SampleNotebookPath}");
            await client.Workspace.Import(SampleNotebookPath, ExportFormat.HTML, null,
                content, true);

            var schedule = new CronSchedule
            {
                QuartzCronExpression = "0 0 9 ? * MON-FRI",
                TimezoneId = "Europe/London",
                PauseStatus = PauseStatus.UNPAUSED
            };

            var newCluster = ClusterAttributes.GetNewClusterConfiguration()
                .WithClusterMode(ClusterMode.SingleNode)
                .WithNodeType(NodeTypes.Standard_D3_v2)
                .WithRuntimeVersion(RuntimeVersions.Runtime_10_4);

            var jobSettings = new JobSettings
            {
                MaxConcurrentRuns = 1,
                Schedule = schedule,
                Name = "Sample Job"
            };

            var task1 = jobSettings.AddTask("task1", new NotebookTask { NotebookPath = SampleNotebookPath })
                .WithDescription("Sample Job - task1")
                .WithNewCluster(newCluster);

            var task2 = jobSettings.AddTask("task2", new NotebookTask { NotebookPath = SampleNotebookPath })
                .WithDescription("Sample Job - task2")
                .WithNewCluster(newCluster);

            jobSettings.AddTask("task3", new NotebookTask { NotebookPath = SampleNotebookPath }, new[] { task1, task2 })
                .WithDescription("Sample Job - task3")
                .WithNewCluster(newCluster);
            
            Console.WriteLine("Creating new job");
            var jobId = await client.Jobs.Create(jobSettings);
            Console.WriteLine("Job created: {0}", jobId);

            // Adding email notifications.
            await client.Jobs.Update(jobId, new JobSettings
            {
                EmailNotifications = new JobEmailNotifications
                {
                    OnSuccess = new[] { "someone@example.com" }
                }
            });

            // Removing email notifications and libraries.
            await client.Jobs.Update(jobId, null, new[] { "email_notifications" });


            // Reset job by pausing schedule and attaching libraries to each task.

            var jobInfo = await client.Jobs.Get(jobId);
            jobInfo.Settings.Schedule.PauseStatus = PauseStatus.PAUSED;

            foreach (var task in jobSettings.Tasks)
            {
                task.AttachLibrary(
                    new MavenLibrary
                    {
                        MavenLibrarySpec = new MavenLibrarySpec { Coordinates = "com.microsoft.azure:synapseml_2.12:0.9.5" }
                    }
                );
            }

            Console.WriteLine("Resetting job");
            await client.Jobs.Reset(jobId, jobInfo.Settings);

            Console.WriteLine("Run now: {0}", jobId);
            var runId = await client.Jobs.RunNow(jobId);

            Console.WriteLine("Run Id: {0}", runId);

            await WaitForRun(client.Jobs, runId);

            Console.WriteLine($"Exporting tasks from run {runId}");

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

            Console.WriteLine($"Deleting run {runId}");
            await client.Jobs.RunsDelete(runId);

            Console.WriteLine($"Deleting job {jobId}");
            await client.Jobs.Delete(jobId);

            Console.WriteLine("Deleting sample workspace");
            await client.Workspace.Delete(SampleWorkspacePath, true);
        }

        private static async Task WaitForRun(IJobsApi jobClient, long runId, int pollIntervalSeconds = 15)
        {
            var retryPolicy = Policy.Handle<WebException>()
                .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.BadGateway)
                .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.InternalServerError)
                .Or<ClientApiException>(e => e.Message.Contains("\"error_code\":\"TEMPORARILY_UNAVAILABLE\""))
                .Or<TaskCanceledException>(e => !e.CancellationToken.IsCancellationRequested) // web request timeout
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
                    $"[{DateTime.UtcNow:s}]Run:{runId}\tLifeCycleState:{run.State.LifeCycleState}\tResultState:{run.State.ResultState}\tCompleted:{run.IsCompleted}"
                );

                return run.State;
            });
        }
    }
}

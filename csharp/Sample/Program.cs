using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client;

namespace Sample
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <Azure databricks base url> <access token>");
                return;
            }

            string baseUrl = args[0];
            string token = args[1];

            Console.WriteLine("Creating client");
            var client = Client.CreateClient(baseUrl, token);

            Console.WriteLine("Listing directories under dbfs:/");
            var result = client.Dbfs.List("/").Result;
            foreach (var fileInfo in result)
            {
                Console.WriteLine(fileInfo.IsDirectory ? "[{0}]\t{1}" : "{0}\t{1}", fileInfo.Path, fileInfo.FileSize);
            }

            Console.WriteLine("Creating new job");
            var newCluster = ClusterInfo.GetNewClusterConfiguration()
                .WithNumberOfWorkers(3)
                .WithPython3(true)
                .WithNodeType(NodeTypes.Standard_D3_v2)
                .WithRuntimeVersion(RuntimeVersions.Runtime_4_2_Scala_2_11);

            var jobSettings = JobSettings.GetNewNotebookJobSettings(
                    "Sample Job",
                    "/Users/jasowang@microsoft.com/Quick Start Using Scala",
                    null)
                .WithNewCluster(newCluster);

            var jobId = client.Jobs.Create(jobSettings).Result;

            Console.WriteLine("Job created: {0}", jobId);

            Console.WriteLine("Run now: {0}", jobId);
            var runId = client.Jobs.RunNow(jobId, null).Result.RunId;

            Console.WriteLine("Run Id: {0}", runId);

            while (true)
            {
                var run = client.Jobs.RunsGet(runId).Result;
                if (run.State.LifeCycleState == RunLifeCycleState.PENDING ||
                    run.State.LifeCycleState == RunLifeCycleState.RUNNING ||
                    run.State.LifeCycleState == RunLifeCycleState.TERMINATING)
                {
                    Console.WriteLine("[{0:s}] Run Id: {1}; LifeCycleState: {2}", DateTime.UtcNow, runId, run.State.LifeCycleState);
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
                else
                {
                    Console.WriteLine("[{0:s}] Run Id: {1}; LifeCycleState: {2}", DateTime.UtcNow, runId, run.State.LifeCycleState);
                    break;
                }
            }

            var viewItems = client.Jobs.RunsExport(runId).Result;

            foreach (var viewItem in viewItems)
            {
                Console.WriteLine(viewItem.Name + ": " + viewItem.Content.Substring(0, 200));
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}

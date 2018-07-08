using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client;
using Newtonsoft.Json;

namespace Sample
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <Azure databricks base url> <access token>");
                return;
            }
            
            var baseUrl = args[0];
            var token = args[1];

            Console.WriteLine("Creating client");
            using (var client = Client.CreateClient(baseUrl, token))
            {
                // await LibrariesApi(client);
                await SecretsApi(client);
                // await TokenApi(client);
                // await GroupsApi(client);
                // await DbfsApi(client);
                // await JobsApi(client);
                // await ClustersApi(client);
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static async Task LibrariesApi(Client client)
        {
            Console.WriteLine("All cluster statuses");
            var libraries = await client.Libraries.AllClusterStatuses();
            foreach (var (clusterId, libraryStatuses) in libraries)
            {
                Console.WriteLine("Cluster: {0}", clusterId);

                foreach (var status in libraryStatuses)
                {
                    Console.WriteLine("\t{0}\t{1}", status.Status, status.Library);
                }
            }

            const string testClusterId = "0530-210517-viced348";

            Console.WriteLine("Getting cluster statuses for {0}", testClusterId);
            var statuses = await client.Libraries.ClusterStatus(testClusterId);
            foreach (var status in statuses)
            {
                Console.WriteLine("\t{0}\t{1}", status.Status, status.Library);
            }

            const string libName = "org.jsoup:jsoup:1.7.2";

            var libraryToInstall = new MavenLibrary
            {
                MavenLibrarySpec = new MavenLibrarySpec
                {
                    Coordinates = libName,
                    Exclusions = new[] {"slf4j:slf4j"}
                }
            };

            Console.WriteLine("Installing library {0}", libraryToInstall);
            await client.Libraries.Install(testClusterId, new Library[] { libraryToInstall });

            while (true)
            {
                statuses = await client.Libraries.ClusterStatus(testClusterId);
                var targetLib = statuses.SingleOrDefault(status =>
                    status.Library is MavenLibrary library && library.MavenLibrarySpec.Coordinates == libName
                    );

                if (targetLib == null)
                {
                    Console.WriteLine("[{0:s}] Library {1} not found", DateTime.UtcNow, libName);
                    break;
                }

                if (targetLib.Status == LibraryInstallStatus.INSTALLED)
                {
                    Console.WriteLine("[{0:s}] Library {1} INSTALLED", DateTime.UtcNow, libName);
                    break;
                }

                Console.WriteLine("[{0:s}] Library {1} status {2}", DateTime.UtcNow, libName, targetLib.Status);

                if (targetLib.Status == LibraryInstallStatus.FAILED)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine("Uninstalling library {0}", libraryToInstall);
            await client.Libraries.Uninstall(testClusterId, new Library[] { libraryToInstall });

            statuses = await client.Libraries.ClusterStatus(testClusterId);
            var uninstalledLib = statuses.Single(status =>
                status.Library is MavenLibrary library && library.MavenLibrarySpec.Coordinates == libName
            );

            Console.WriteLine("[{0:s}] Library {1} status {2}", DateTime.UtcNow, libName, uninstalledLib.Status);

        }

        private static async Task SecretsApi(Client client)
        {
            const string scope = "SampleScope";
            Console.WriteLine("Creating secrets scope");
            await client.Secrets.CreateScope(scope, null);

            Console.WriteLine("Creating text secret");
            await client.Secrets.PutSecret("textvalue", scope, "secretkey.text");

            Console.WriteLine("Creating binary secret");
            await client.Secrets.PutSecret(new byte[]{0x01, 0x02, 0x03, 0x04}, scope, "secretkey.bin");

            Console.WriteLine("Listing secrets");
            var secrets = await client.Secrets.ListSecrets(scope);
            foreach (var secret in secrets)
            {
                Console.WriteLine("Secret key {0}, last updated: {1:s}", secret.Key, secret.LastUpdatedTimestamp);
            }

            Console.WriteLine("Deleting secrets");
            await client.Secrets.DeleteSecret(scope, "secretkey.text");
            await client.Secrets.DeleteSecret(scope, "secretkey.bin");

            Console.WriteLine("Deleting secrets scope");
            await client.Secrets.DeleteScope(scope);
        }

        private static async Task TokenApi(Client client)
        {
            Console.WriteLine("Creating token without expiry");
            var (tokenValue, tokenInfo) = await client.Token.Create(null, "Sample token");
            Console.WriteLine("Token value: {0}", tokenValue);
            Console.WriteLine("Token Id {0}", tokenInfo.TokenId);
            Console.WriteLine("Token comment {0}", tokenInfo.Comment);
            Console.WriteLine("Token creation time {0:s}", tokenInfo.CreationTime);
            Console.WriteLine("Token expiry time {0:s}", tokenInfo.ExpiryTime);
            Console.WriteLine("Deleting token");
            await client.Token.Revoke(tokenInfo.TokenId);
            
            Console.WriteLine("Creating token with expiry");
            (tokenValue, tokenInfo) = await client.Token.Create(3600, "Sample token");
            Console.WriteLine("Token value: {0}", tokenValue);
            Console.WriteLine("Token comment {0}", tokenInfo.Comment);
            Console.WriteLine("Token creation time {0:s}", tokenInfo.CreationTime);
            Console.WriteLine("Token expiry time {0:s}", tokenInfo.ExpiryTime);
            Console.WriteLine("Deleting token");
            await client.Token.Revoke(tokenInfo.TokenId);

            Console.WriteLine("Listing tokens");
            var tokens = await client.Token.List();
            foreach (var token in tokens)
            {
                Console.WriteLine("Token Id {0}\tComment {1}", token.TokenId, token.Comment);
            }
        }

        private static async Task ClustersApi(Client client)
        {
            var clusterConfig = ClusterInfo.GetNewClusterConfiguration("Sample cluster")
                .WithRuntimeVersion(RuntimeVersions.Runtime_4_2_Scala_2_11)
                .WithAutoScale(3, 7)
                .WithAutoTermination(30)
                .WithClusterLogConf("dbfs:/logs/")
                .WithNodeType(NodeTypes.Standard_D3_v2)
                .WithPython3(true);

            Console.WriteLine("Creating cluster");
            var clusterId = await client.Clusters.Create(clusterConfig);

            var createdCluster = await client.Clusters.Get(clusterId);
            var createdClusterConfig = JsonConvert.SerializeObject(createdCluster, Formatting.Indented);

            Console.WriteLine("Created cluster config: ");
            Console.WriteLine(createdClusterConfig);

            while (true)
            {
                var state = await client.Clusters.Get(clusterId);
                if (state.State == ClusterState.RUNNING)
                {
                    Console.WriteLine("[{0:s}] Cluster {1} is running", DateTime.UtcNow, clusterId);
                    break;
                }

                Console.WriteLine("[{0:s}] Cluster {1} state {2}", DateTime.UtcNow, clusterId, state.State);

                if (state.State == ClusterState.ERROR || state.State == ClusterState.TERMINATED)
                {
                    break;
                }
                
                await Task.Delay(TimeSpan.FromSeconds(30));
            }

            Console.WriteLine("Deleting cluster {0}", clusterId);
            await client.Clusters.Delete(clusterId);
        }

        private static async Task GroupsApi(Client client)
        {
            Console.WriteLine("Listing groups");
            var groupsList = await client.Groups.List();
            foreach (var group in groupsList)
            {
                Console.WriteLine("Group name: {0}", group);
            }

            const string newGroupName = "sample group";

            Console.WriteLine("Creating new group \"{0}\"", newGroupName);
            await client.Groups.Create(newGroupName);

            Console.WriteLine("Deleting group \"{0}\"", newGroupName);
            await client.Groups.Delete(newGroupName);

            Console.WriteLine("Listing members in admins group");
            var members = await client.Groups.ListMembers("admins");
            foreach (var member in members)
            {
                if (!string.IsNullOrEmpty(member.UserName))
                {
                    Console.WriteLine("Member (User): {0}", member.UserName);
                }
                else
                {
                    Console.WriteLine("Member (Group): {0}", member.GroupName);
                }
            }
        }

        private static async Task JobsApi(Client client)
        {
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

            var jobId = await client.Jobs.Create(jobSettings);

            Console.WriteLine("Job created: {0}", jobId);

            Console.WriteLine("Run now: {0}", jobId);
            var runId = (await client.Jobs.RunNow(jobId, null)).RunId;

            Console.WriteLine("Run Id: {0}", runId);

            while (true)
            {
                var run = await client.Jobs.RunsGet(runId);
                if (run.State.LifeCycleState == RunLifeCycleState.PENDING ||
                    run.State.LifeCycleState == RunLifeCycleState.RUNNING ||
                    run.State.LifeCycleState == RunLifeCycleState.TERMINATING)
                {
                    Console.WriteLine("[{0:s}] Run Id: {1}; LifeCycleState: {2}", DateTime.UtcNow, runId,
                        run.State.LifeCycleState);
                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
                else
                {
                    Console.WriteLine("[{0:s}] Run Id: {1}; LifeCycleState: {2}", DateTime.UtcNow, runId,
                        run.State.LifeCycleState);
                    break;
                }
            }

            var viewItems = await client.Jobs.RunsExport(runId);

            foreach (var viewItem in viewItems)
            {
                Console.WriteLine(viewItem.Name + ": " + viewItem.Content.Substring(0, 200));
            }
        }

        private static async Task DbfsApi(Client client)
        {
            Console.WriteLine("Listing directories under dbfs:/");
            var result = await client.Dbfs.List("/");
            foreach (var fileInfo in result)
            {
                Console.WriteLine(fileInfo.IsDirectory ? "[{0}]\t{1}" : "{0}\t{1}", fileInfo.Path, fileInfo.FileSize);
            }

            Console.WriteLine("Uploading a file");
            var uploadPath = "/test/" + Guid.NewGuid() + ".txt";

            using (var ms = new MemoryStream())
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("https://norvig.com/big.txt",
                        HttpCompletionOption.ResponseHeadersRead);
                    await response.Content.CopyToAsync(ms);
                }
                
                await client.Dbfs.Upload(uploadPath, true, ms);
            }

            Console.WriteLine("Getting info of the uploaded file");
            var uploadedFile = await client.Dbfs.GetStatus(uploadPath);
            Console.WriteLine("Path: {0}\tSize: {1}", uploadedFile.Path, uploadedFile.FileSize);

            Console.WriteLine("Deleting uploaded file");
            await client.Dbfs.Delete(uploadPath, false);
        }
    }
}

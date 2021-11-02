using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Must be an existing user in the databricks environment, otherwise you will get a "DIRECTORY_PROTECTED" error.
        /// </summary>
        private const string DatabricksUserName = "username@company.com"; 

        private static readonly string SampleWorkspacePath = $"/Users/{DatabricksUserName}/SampleWorkspace";
        private static readonly string SampleNotebookPath = $"{SampleWorkspacePath}/Quick Start Using Scala";

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
                await WorkspaceApi(client);
                await LibrariesApi(client);
                await SecretsApi(client);
                await TokenApi(client);
                await GroupsApi(client);
                await DbfsApi(client);
                await JobsApi(client);
                await ClustersApi(client);
                await InstancePoolApi(client);
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        private static async Task WorkspaceApi(DatabricksClient client)
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
            Console.WriteLine(exportedString.Substring(0, 100) + "...");
            Console.WriteLine("====================");

            Console.WriteLine("Deleting sample workspace");
            await client.Workspace.Delete(SampleWorkspacePath, true);
        }

        private static async Task<byte[]> DownloadSampleNotebook()
        {
            byte[] content;

            using (var httpClient = new HttpClient())
            {
                content = await httpClient.GetByteArrayAsync(
                    "https://cdn2.hubspot.net/hubfs/438089/notebooks/Quick_Start/Quick_Start_Using_Scala.html"
                );
            }

            return content;
        }

        private static async Task LibrariesApi(DatabricksClient client)
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

            var mvnlibraryToInstall = new MavenLibrary
            {
                MavenLibrarySpec = new MavenLibrarySpec
                {
                    Coordinates = "org.jsoup:jsoup:1.7.2",
                    Exclusions = new[] {"slf4j:slf4j"}
                }
            };

            await TestInstallUninstallLibrary(client, mvnlibraryToInstall, testClusterId);

            var whlLibraryToInstall = new WheelLibrary
            {
                Wheel = "dbfs:/mnt/dbfsmount1/temp/docutils-0.14-py3-none-any.whl"
            };

            await TestInstallUninstallLibrary(client, whlLibraryToInstall, testClusterId);
        }

        private static async Task TestInstallUninstallLibrary(DatabricksClient client, Library library, string clusterId)
        {
            Console.WriteLine("Installing library {0}", library);
            await client.Libraries.Install(clusterId, new [] { library });

            while (true)
            {
                var statuses = await client.Libraries.ClusterStatus(clusterId);
                var targetLib = statuses.SingleOrDefault(status => status.Library.Equals(library));

                if (targetLib == null)
                {
                    Console.WriteLine("[{0:s}] Library {1} not found", DateTime.UtcNow, library);
                    break;
                }

                if (targetLib.Status == LibraryInstallStatus.INSTALLED)
                {
                    Console.WriteLine("[{0:s}] Library {1} INSTALLED", DateTime.UtcNow, library);
                    break;
                }

                Console.WriteLine("[{0:s}] Library {1} status {2}", DateTime.UtcNow, library, targetLib.Status);

                if (targetLib.Status == LibraryInstallStatus.FAILED)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine("Uninstalling library {0}", library);
            await client.Libraries.Uninstall(clusterId, new [] { library });

            var s = await client.Libraries.ClusterStatus(clusterId);
            var uninstalledLib = s.Single(status => status.Library.Equals(library));

            Console.WriteLine("[{0:s}] Library {1} status {2}", DateTime.UtcNow, library, uninstalledLib.Status);
        }

        private static async Task SecretsApi(DatabricksClient client)
        {
            Console.WriteLine("Listing secrets scope");
            var scopes = await client.Secrets.ListScopes();
            foreach (var scope in scopes)
            {
                Console.WriteLine("Secret scope: {0}, backend type: {1}", scope.Name, scope.BackendType);
            }

            const string scopeName = "SampleScope";
            Console.WriteLine("Creating secrets scope");
            await client.Secrets.CreateDatabricksBackedScope(scopeName, null);

            Console.WriteLine("Creating text secret");
            await client.Secrets.PutSecret("textvalue", scopeName, "secretkey.text");

            Console.WriteLine("Creating binary secret");
            await client.Secrets.PutSecret(new byte[]{0x01, 0x02, 0x03, 0x04}, scopeName, "secretkey.bin");

            Console.WriteLine("Listing secrets");
            var secrets = await client.Secrets.ListSecrets(scopeName);
            foreach (var secret in secrets)
            {
                Console.WriteLine("Secret key {0}, last updated: {1:s}", secret.Key, secret.LastUpdatedTimestamp);
            }

            Console.WriteLine("Deleting secrets");
            await client.Secrets.DeleteSecret(scopeName, "secretkey.text");
            await client.Secrets.DeleteSecret(scopeName, "secretkey.bin");

            Console.WriteLine("Deleting secrets scope");
            await client.Secrets.DeleteScope(scopeName);
        }

        private static async Task TokenApi(DatabricksClient client)
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

        private static async Task InstancePoolApi(DatabricksClient client)
        {
            Console.WriteLine("Creating Testing Instance Pool");
            var poolAttributes = new InstancePoolAttributes
            {
                PoolName = "TestInstancePool",
                PreloadedSparkVersions = new[] {RuntimeVersions.Runtime_6_4_ESR},
                MinIdleInstances = 2,
                MaxCapacity = 100,
                IdleInstanceAutoTerminationMinutes = 15,
                NodeTypeId = NodeTypes.Standard_D3_v2,
                EnableElasticDisk = true,
                DiskSpec = new DiskSpec
                    {DiskCount = 2, DiskSize = 64, DiskType = DiskType.FromAzureDisk(AzureDiskVolumeType.STANDARD_LRS)},
                PreloadedDockerImages = new[]
                {
                    new DockerImage {Url = "databricksruntime/standard:latest"}
                },
                AzureAttributes = new InstancePoolAzureAttributes {Availability = AzureAvailability.SPOT_AZURE, SpotBidMaxPrice = -1}
            };

            var poolId = await client.InstancePool.Create(poolAttributes).ConfigureAwait(false);

            Console.WriteLine("Listing pools");
            var pools = await client.InstancePool.List().ConfigureAwait(false);
            foreach (var pool in pools)
            {
                Console.WriteLine($"\t{pool.PoolId}\t{pool.PoolName}\t{pool.State}");
            }

            Console.WriteLine("Getting created pool by poolId");
            var targetPoolInfo = await client.InstancePool.Get(poolId).ConfigureAwait(false);

            Console.WriteLine("Editing pool");
            targetPoolInfo.MinIdleInstances = 3;
            await client.InstancePool.Edit(poolId, targetPoolInfo).ConfigureAwait(false);

            Console.WriteLine("Getting edited pool by poolId");
            targetPoolInfo = await client.InstancePool.Get(poolId).ConfigureAwait(false);
            Console.WriteLine($"MinIdleInstances: {targetPoolInfo.MinIdleInstances}");

            Console.WriteLine("Creating a sample cluster in the pool.");
            var clusterConfig = ClusterInfo.GetNewClusterConfiguration("Sample cluster")
                .WithRuntimeVersion(RuntimeVersions.Runtime_7_3)
                .WithAutoScale(3, 7)
                .WithAutoTermination(30)
                .WithClusterLogConf("dbfs:/logs/");
            clusterConfig.InstancePoolId = poolId;

            var clusterId = await client.Clusters.Create(clusterConfig);

            var createdCluster = await client.Clusters.Get(clusterId);

            Console.WriteLine($"Created cluster pool Id: {createdCluster.InstancePoolId}");

            Console.WriteLine("Deleting pool");
            await client.InstancePool.Delete(poolId);

            Console.WriteLine("Deleting cluster");
            await client.Clusters.Delete(clusterId);
        }

        private static async Task ClustersApi(DatabricksClient client)
        {
            Console.WriteLine("Listing node types (take 10)");
            var nodeTypes = await client.Clusters.ListNodeTypes();
            foreach (var nodeType in nodeTypes.Take(10))
            {
                Console.WriteLine($"\t{nodeType.NodeTypeId}\tMemory: {nodeType.MemoryMb} MB\tCores: {nodeType.NumCores}\tAvailable Quota: {nodeType.ClusterCloudProviderNodeInfo.AvailableCoreQuota}");
            }
            
            Console.WriteLine("Listing Databricks runtime versions");
            var sparkVersions = await client.Clusters.ListSparkVersions();
            foreach (var (key, name) in sparkVersions)
            {
                Console.WriteLine($"\t{key}\t\t{name}");
            }

            Console.WriteLine("Creating standard cluster");

            var clusterConfig = ClusterInfo.GetNewClusterConfiguration("Sample cluster")
                .WithRuntimeVersion(RuntimeVersions.Runtime_6_4_ESR)
                .WithAutoTermination(30)
                .WithClusterLogConf("dbfs:/logs/")
                .WithNodeType(NodeTypes.Standard_D3_v2)
                .WithClusterMode(ClusterMode.SingleNode);

            clusterConfig.DockerImage = new DockerImage { Url = "databricksruntime/standard:latest" };

            var clusterId = await client.Clusters.Create(clusterConfig);

            var createdCluster = await client.Clusters.Get(clusterId);
            var createdClusterConfig = JsonConvert.SerializeObject(createdCluster, Formatting.Indented);

            Console.WriteLine("Created cluster config: ");
            Console.WriteLine(createdClusterConfig);

            while (true)
            {
                var state = await client.Clusters.Get(clusterId);

                Console.WriteLine("[{0:s}] Cluster {1}\tState {2}\tMessage {3}", DateTime.UtcNow, clusterId,
                    state.State, state.StateMessage);

                if (state.State == ClusterState.RUNNING || state.State == ClusterState.ERROR || state.State == ClusterState.TERMINATED)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
            
            Console.WriteLine("Deleting cluster {0}", clusterId);
            await client.Clusters.Delete(clusterId);

            Console.WriteLine("Creating HighConcurrency cluster");

            clusterConfig = ClusterInfo.GetNewClusterConfiguration("Sample cluster")
                .WithRuntimeVersion(RuntimeVersions.Runtime_6_4_ESR)
                .WithAutoScale(3, 7)
                .WithAutoTermination(30)
                .WithClusterLogConf("dbfs:/logs/")
                .WithNodeType(NodeTypes.Standard_D3_v2)
                .WithClusterMode(ClusterMode.HighConcurrency)
                .WithTableAccessControl(true);

            clusterId = await client.Clusters.Create(clusterConfig);

            createdCluster = await client.Clusters.Get(clusterId);
            createdClusterConfig = JsonConvert.SerializeObject(createdCluster, Formatting.Indented);

            Console.WriteLine("Created cluster config: ");
            Console.WriteLine(createdClusterConfig);

            while (true)
            {
                var state = await client.Clusters.Get(clusterId);

                Console.WriteLine("[{0:s}] Cluster {1}\tState {2}\tMessage {3}", DateTime.UtcNow, clusterId,
                    state.State, state.StateMessage);

                if (state.State == ClusterState.RUNNING || state.State == ClusterState.ERROR || state.State == ClusterState.TERMINATED)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }

            Console.WriteLine("Deleting cluster {0}", clusterId);
            await client.Clusters.Delete(clusterId);
            
            Console.WriteLine("Getting all events from a test cluster");
            const string testClusterId = "0530-210517-viced348";
            
            EventsResponse eventsResponse = null;
            var events = new List<ClusterEvent>();
            do
            {
                var nextPage = eventsResponse?.NextPage;
                eventsResponse = await client.Clusters.Events(
                    testClusterId, 
                    nextPage?.StartTime, 
                    nextPage?.EndTime,
                    nextPage?.Order, 
                    nextPage?.EventTypes, 
                    nextPage?.Offset, 
                    nextPage?.Limit
                    );
                events.AddRange(eventsResponse.Events);

            } while (eventsResponse.HasNextPage);

            Console.WriteLine("{0} events retrieved from cluster {1}.", events.Count, testClusterId);
            Console.WriteLine("Top 10 events: ");
            foreach (var e in events.Take(10))
            {
                Console.WriteLine("\t[{0:s}] {1}\t{2}", e.Timestamp, e.Type, e.Details.User);
            }
        }

        private static async Task GroupsApi(DatabricksClient client)
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

            Console.WriteLine($"Adding members in {newGroupName} group");
            await client.Groups.AddMember(newGroupName, new PrincipalName {UserName = DatabricksUserName });

            Console.WriteLine($"Listing members in {newGroupName} group");
            var members = await client.Groups.ListMembers(newGroupName);
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

            Console.WriteLine($"Removing members in {newGroupName} group");
            await client.Groups.RemoveMember(newGroupName, new PrincipalName {UserName = DatabricksUserName });


            Console.WriteLine("Deleting group \"{0}\"", newGroupName);
            await client.Groups.Delete(newGroupName);
        }

        private static async Task JobsApi(DatabricksClient client)
        {
            Console.WriteLine("Creating new job");
            var newCluster = ClusterInfo.GetNewClusterConfiguration()
                .WithNumberOfWorkers(3)
                .WithNodeType(NodeTypes.Standard_D3_v2)
                .WithRuntimeVersion(RuntimeVersions.Runtime_6_4_ESR);

            Console.WriteLine($"Creating workspace {SampleWorkspacePath}");
            await client.Workspace.Mkdirs(SampleWorkspacePath);

            Console.WriteLine("Downloading sample notebook");
            var content = await DownloadSampleNotebook();

            Console.WriteLine($"Importing sample HTML notebook to {SampleNotebookPath}");
            await client.Workspace.Import(SampleNotebookPath, ExportFormat.HTML, null,
                content, true);

            var schedule = new CronSchedule()
            {
                QuartzCronExpression = "0 0 9 ? * MON-FRI",
                TimezoneId = "Europe/London",
                PauseStatus = PauseStatus.UNPAUSED
            };
            
            var jobSettings = JobSettings.GetNewNotebookJobSettings(
                    "Sample Job",
                    SampleNotebookPath,
                    null)
                .WithNewCluster(newCluster)
                .WithSchedule(schedule);

            var jobId = await client.Jobs.Create(jobSettings);

            Console.WriteLine("Job created: {0}", jobId);

            var jobWithClusterInfo = await client.Jobs.Get(jobId);

            var existingSettings = jobWithClusterInfo.Settings;
            var existingSchedule = existingSettings.Schedule;
            existingSchedule.PauseStatus = PauseStatus.PAUSED;
            existingSettings.Schedule = existingSchedule;
            
            Console.WriteLine("Pausing job schedule");
            await client.Jobs.Reset(jobId, existingSettings);

            Console.WriteLine("Run now: {0}", jobId);
            var runId = (await client.Jobs.RunNow(jobId, null)).RunId;

            Console.WriteLine("Run Id: {0}", runId);

            while (true)
            {
                var run = await client.Jobs.RunsGet(runId);

                Console.WriteLine("[{0:s}] Run Id: {1}\tLifeCycleState: {2}\tStateMessage: {3}", DateTime.UtcNow, runId,
                    run.State.LifeCycleState, run.State.StateMessage);

                if (run.State.LifeCycleState == RunLifeCycleState.PENDING ||
                    run.State.LifeCycleState == RunLifeCycleState.RUNNING ||
                    run.State.LifeCycleState == RunLifeCycleState.TERMINATING)
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
                else
                {
                    break;
                }
            }

            var viewItems = await client.Jobs.RunsExport(runId);

            foreach (var viewItem in viewItems)
            {
                Console.WriteLine("Exported view item from run: " + viewItem.Name);
                Console.WriteLine("====================");
                Console.WriteLine(viewItem.Content.Substring(0, 100) + "...");
                Console.WriteLine("====================");
            }

            Console.WriteLine("Deleting sample workspace");
            await client.Workspace.Delete(SampleWorkspacePath, true);
        }

        private static async Task DbfsApi(DatabricksClient client)
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

            using (var ms = new MemoryStream())
            {
                await client.Dbfs.Download(uploadPath, ms);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                var content = await sr.ReadToEndAsync();
                Console.WriteLine(content.Substring(0, 100));
            }

            Console.WriteLine("Getting info of the uploaded file");
            var uploadedFile = await client.Dbfs.GetStatus(uploadPath);
            Console.WriteLine("Path: {0}\tSize: {1}", uploadedFile.Path, uploadedFile.FileSize);

            var newPath = "/test/" + Guid.NewGuid() + ".txt";
            await client.Dbfs.Move(uploadPath, newPath);

            Console.WriteLine("Deleting uploaded file");
            await client.Dbfs.Delete(newPath, false);
        }
    }
}

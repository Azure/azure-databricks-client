using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestLibrariesApi(DatabricksClient client)
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
                Exclusions = new[] { "slf4j:slf4j" }
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
        await client.Libraries.Install(clusterId, new[] {library});

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
        await client.Libraries.Uninstall(clusterId, new[] {library});

        var s = await client.Libraries.ClusterStatus(clusterId);
        var uninstalledLib = s.Single(status => status.Library.Equals(library));

        Console.WriteLine("[{0:s}] Library {1} status {2}", DateTime.UtcNow, library, uninstalledLib.Status);
    }
}
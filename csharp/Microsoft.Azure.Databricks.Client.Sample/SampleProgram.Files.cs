namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestFilesApi(DatabricksClient client)
    {
        Console.WriteLine("First specify a volume URI path where the tests will be executed...");
        var basePath = Console.ReadLine();

        Console.WriteLine($"Using '{basePath}' as base path for the next tests.");

        // Create directory
        var directoryPath = basePath + "/" + Guid.NewGuid();
        Console.WriteLine($"Creating empty '{directoryPath}' directory.");

        await client.Files.CreateDirectory(directoryPath);

        // Get newly created directory metadata
        Console.WriteLine("Fetching directory metadata.");
        var contentHeaders = await client.Files.GetDirectoryMetadata(directoryPath);
        foreach (var contentHeader in contentHeaders)
        {
            Console.WriteLine($"'{contentHeader.Key}': '{contentHeader.Value.First()}'");
        }

        // Populate the directory with a file
        Console.WriteLine($"Uploading test file to '{directoryPath}' directory.'");
        var uploadPath = directoryPath + "/" + Guid.NewGuid() + ".txt";

        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://norvig.com/big.txt",
            HttpCompletionOption.ResponseHeadersRead);

        var fileContents = await response.Content.ReadAsByteArrayAsync();
        await client.Files.Upload(uploadPath, fileContents, true);

        // Read the file first 100 bytes
        Console.WriteLine("Reading test file...");
        using var msDownload = new MemoryStream();
        await client.Files.Download(uploadPath, msDownload, "bytes=0-99");
        msDownload.Position = 0;
        var sr = new StreamReader(msDownload);
        var content = await sr.ReadToEndAsync();
        Console.WriteLine(content[..99]);

        // Read the file metadata
        Console.WriteLine("Getting file metadata...");
        var metadataHeaders = await client.Files.GetFileMetadata(uploadPath);
        foreach (var header in metadataHeaders)
        {
            Console.WriteLine($"'{header.Key}': '{header.Value.First()}'");
        }

        // List directory contents
        Console.WriteLine($"Listing test directory contents...");
        var result = await client.Files.ListDirectoryContents(directoryPath);
        foreach (var entry in result.Contents)
        {
            Console.WriteLine(entry);
        }

        // Delete the file
        Console.WriteLine("Deleting test file...");
        await client.Files.Delete(uploadPath);

        // Delete the directory
        Console.WriteLine("Deleting test directory...");
        await client.Files.DeleteDirectory(directoryPath);
    }
}

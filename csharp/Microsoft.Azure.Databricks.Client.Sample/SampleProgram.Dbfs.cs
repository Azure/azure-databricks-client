using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestDbfsApi(DatabricksClient client)
    {
        Console.WriteLine("Listing directories under dbfs:/");
        var result = await client.Dbfs.List("/");
        foreach (var fileInfo in result)
        {
            Console.WriteLine(fileInfo.IsDirectory ? "[{0}]\t{1}" : "{0}\t{1}", fileInfo.Path, fileInfo.FileSize);
        }

        Console.WriteLine("Uploading a file");
        var uploadPath = "/test/" + Guid.NewGuid() + ".txt";

        using var msUpload = new MemoryStream();
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://norvig.com/big.txt",
            HttpCompletionOption.ResponseHeadersRead);
        await response.Content.CopyToAsync(msUpload);

        await client.Dbfs.Upload(uploadPath, true, msUpload);

        using var msDownload = new MemoryStream();
        await client.Dbfs.Download(uploadPath, msDownload);
        msDownload.Position = 0;
        var sr = new StreamReader(msDownload);
        var content = await sr.ReadToEndAsync();
        Console.WriteLine(content[..100]);

        Console.WriteLine("Getting info of the uploaded file");
        var uploadedFile = await client.Dbfs.GetStatus(uploadPath);
        Console.WriteLine("Path: {0}\tSize: {1}", uploadedFile.Path, uploadedFile.FileSize);

        var newPath = "/test/" + Guid.NewGuid() + ".txt";
        await client.Dbfs.Move(uploadPath, newPath);

        Console.WriteLine("Deleting uploaded file");
        await client.Dbfs.Delete(newPath, false);
    }
}
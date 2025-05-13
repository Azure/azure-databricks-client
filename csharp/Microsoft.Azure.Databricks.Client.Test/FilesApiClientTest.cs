using System.Net;
using System.Text;
using System.Text.Json;

using Moq.Contrib.HttpClient;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class FilesApiClientTest : ApiClientTest
{
    private static readonly Uri DirectoriesApiUri = new(BaseApiUri, "2.0/fs/directories");
    private static readonly Uri FilesApiUri = new(BaseApiUri, "2.0/fs/files");

    private static readonly string DirectoryRelativePath = "/" + Guid.NewGuid();
    private static readonly string FileRelativePath = DirectoryRelativePath + "/" + Guid.NewGuid() + ".txt";

    [TestMethod]
    public async Task TestCreateDirectory()
    {
        var requestUri = $"{DirectoriesApiUri}{DirectoryRelativePath}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, requestUri)
            .ReturnsResponse(HttpStatusCode.NoContent);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        await client.CreateDirectory(DirectoryRelativePath);

        handler.VerifyRequest(
            HttpMethod.Put,
            requestUri);
    }

    [TestMethod]
    public async Task TestDeleteDirectory()
    {
        var requestUri = $"{DirectoriesApiUri}{DirectoryRelativePath}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.NoContent);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        await client.DeleteDirectory(DirectoryRelativePath);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

    [TestMethod]
    public async Task TestGetDirectoryMetadata()
    {
        const string expectedHeaderName = "X-Test-Header";
        const string expectedHeaderValue = "Value";
        var requestUri = $"{DirectoriesApiUri}{DirectoryRelativePath}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Head, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, message =>
            {
                message.Content.Headers.Add(expectedHeaderName, expectedHeaderValue);
            });

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        var response = await client.GetDirectoryMetadata(DirectoryRelativePath);

        Assert.IsTrue(response.Contains(expectedHeaderName));

        handler.VerifyRequest(
            HttpMethod.Head,
            requestUri);
    }

    [TestMethod]
    public async Task TestListDirectoryContents_ThrowsWithInvalidPageSize()
    {
        var handler = CreateMockHandler();
        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;
        using var client = new FilesApiClient(mockClient);
        await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(async () => await client.ListDirectoryContents(DirectoryRelativePath, 15000));
    }

    [TestMethod]
    public async Task TestListDirectoryContents()
    {
        var expectedResponse = @"
        {
          ""contents"": [
            {
              ""file_size"": 114864646,
              ""is_directory"": true,
              ""last_modified"": 646859599,
              ""name"": ""test"",
              ""path"": ""/Volumes/test-catalog/test-schema/test-volume/test/test.txt""
            }
          ],
          ""next_page_token"": ""test-token""
        }";

        var requestUri = $"{DirectoriesApiUri}{DirectoryRelativePath}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        var actual = await client.ListDirectoryContents(DirectoryRelativePath);

        var actualJson = JsonSerializer.Serialize(actual, Options);
        AssertJsonDeepEquals(expectedResponse, actualJson);
    }

    [TestMethod]
    public async Task TestUploadFile()
    {
        var requestUri = $"{FilesApiUri}{FileRelativePath}?overwrite=true";

        var fileContents = Guid.NewGuid().ToByteArray();

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, requestUri)
            .ReturnsResponse(HttpStatusCode.NoContent);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        await client.Upload(FileRelativePath, fileContents, true);

        handler.VerifyRequest(
            HttpMethod.Put,
            requestUri);
    }

    [TestMethod]
    public async Task TestGetFileMetadata()
    {
        const string expectedHeaderName = "X-Test-Header";
        const string expectedHeaderValue = "Value";
        var requestUri = $"{FilesApiUri}{FileRelativePath}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Head, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, message =>
            {
                message.Content.Headers.Add(expectedHeaderName, expectedHeaderValue);
            });

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        var response = await client.GetFileMetadata(FileRelativePath);

        Assert.IsTrue(response.Contains(expectedHeaderName));

        handler.VerifyRequest(
            HttpMethod.Head,
            requestUri);
    }

    [TestMethod]
    public async Task TestDownloadFile()
    {
        var requestUri = $"{FilesApiUri}{FileRelativePath}";
        var expectedResponse = "Hello World!"u8.ToArray();

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/octet-stream");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var msDownload = new MemoryStream();
        using var client = new FilesApiClient(mockClient);
        await client.Download(FileRelativePath, msDownload);
        msDownload.Position = 0;
        var sr = new StreamReader(msDownload, Encoding.UTF8);
        var content = await sr.ReadToEndAsync();

        Assert.AreEqual(Encoding.UTF8.GetString(expectedResponse), content);

        handler.VerifyRequest(
            HttpMethod.Get,
            requestUri);
    }

    [TestMethod]
    public async Task TestDeleteFile()
    {
        var requestUri = $"{FilesApiUri}{FileRelativePath}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.NoContent);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new FilesApiClient(mockClient);
        await client.Delete(FileRelativePath);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

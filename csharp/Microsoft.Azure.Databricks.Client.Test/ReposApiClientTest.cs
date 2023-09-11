using Microsoft.Azure.Databricks.Client.Models;
using Moq.Contrib.HttpClient;
using Moq;
using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class ReposApiClientTest : ApiClientTest
{
    private static readonly Uri ReposApiUri = new(BaseApiUri, "2.0/repos");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
                {
                  ""repos"": [
                    {
                      ""id"": 5249608814509279,
                      ""url"": ""https://github.com/jsmith/test"",
                      ""provider"": ""gitHub"",
                      ""path"": ""/Repos/Production/testrepo"",
                      ""branch"": ""main"",
                      ""head_commit_id"": ""7e0847ede61f07adede22e2bcce6050216489171""
                    }
                  ],
                  ""next_page_token"": ""test_token""
                }
                ";

        var expected = JsonSerializer.Deserialize<JsonObject>(expectedResponse, Options)!["repos"]!.Deserialize<List<Repo>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(ReposApiUri, "?&path_prefix=/Repos/Production"))
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ReposApiClient(hc);
        var (actual, token) = await client.List("/Repos/Production");

        CollectionAssert.AreEqual(expected!.ToArray(), actual.ToArray());
        Assert.AreEqual("test_token", token);

        handler.VerifyRequest(
            HttpMethod.Get,
            new Uri(ReposApiUri, "?&path_prefix=/Repos/Production"),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGet()
    {
        const string expectedResponse = @"
                {
                  ""id"": 5249608814509279,
                  ""url"": ""https://github.com/jsmith/test"",
                  ""provider"": ""gitHub"",
                  ""path"": ""/Repos/Production/testrepo"",
                  ""branch"": ""main"",
                  ""head_commit_id"": ""7e0847ede61f07adede22e2bcce6050216489171""
                }
                ";

        var expected = JsonSerializer.Deserialize<Repo>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(ReposApiUri + "/5249608814509279"))
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ReposApiClient(hc);
        var actual = await client.Get(5249608814509279);

        Assert.AreEqual(expected, actual);

        handler.VerifyRequest(
            HttpMethod.Get,
            new Uri(ReposApiUri + "/5249608814509279"),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var apiUri = new Uri(ReposApiUri + "/5249608814509279");

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ReposApiClient(hc);
        await client.Delete(5249608814509279);
        handler.VerifyRequest(HttpMethod.Delete, apiUri, Times.Once());
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var apiUri = new Uri(ReposApiUri + "/5249608814509279");

        long testId = 5249608814509279;
        const string expectedRequest = "{\"branch\":\"main\"}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, apiUri)
            .ReturnsResponse(HttpStatusCode.OK)
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ReposApiClient(hc);
        await client.Update(testId, branch: "main");

        handler.VerifyRequest(
            HttpMethod.Patch,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestCreate()
    {
        const string expectedRequest = "{\"url\": \"https://github.com/jsmith/test\",\"provider\":\"gitHub\",\"path\":\"/Repos/Production/testrepo\"}";
        const string expectedResponse = "{\"id\": 5249608814509279,\"url\": \"https://github.com/jsmith/test\",\"provider\":\"gitHub\",\"path\":\"/Repos/Production/testrepo\",\"branch\":\"main\",\"head_commit_id\":\"7e0847ede61f07adede22e2bcce6050216489171\"}";

        var expected = JsonSerializer.Deserialize<Repo>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, ReposApiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ReposApiClient(hc);
        var actual = await client.Create("https://github.com/jsmith/test", RepoProvider.gitHub, "/Repos/Production/testrepo");

        Assert.AreEqual(expected, actual);

        handler.VerifyRequest(
            HttpMethod.Post,
            ReposApiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class WorkspaceApiClientTest : ApiClientTest
{
    private static readonly Uri WorkspaceApiUri = new(BaseApiUri, "2.0/workspace/");

    [TestMethod]
    public async Task TestExport()
    {
        var apiUri = new Uri(WorkspaceApiUri, "export");
        const string expectedResponse = @"
                {
                    ""content"": ""Ly8gRGF0YWJyaWNrcyBub3RlYm9vayBzb3VyY2UKMSsx""
                }
                ";

        var expected = Convert.FromBase64String(
            JsonSerializer.Deserialize<JsonObject>(expectedResponse, Options)!["content"]!.GetValue<string>());

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(apiUri, "?path=/notebook_path&format=SOURCE"))
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WorkspaceApiClient(hc);
        var actual = await client.Export("/notebook_path", ExportFormat.SOURCE);

        CollectionAssert.AreEqual(actual, expected);

        handler.VerifyRequest(
            HttpMethod.Get,
            new Uri(apiUri, "?path=/notebook_path&format=SOURCE"),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestImport()
    {
        var apiUri = new Uri(WorkspaceApiUri, "import");

        var content = "Ly8gRGF0YWJyaWNrcyBub3RlYm9vayBzb3VyY2UKMSsx";
        var contentBytes = Encoding.ASCII.GetBytes(content);
        var contentBase64 = Convert.ToBase64String(contentBytes);

        string expectedRequest = $@"
            {{
              ""content"": ""{contentBase64}"",
              ""format"": ""AUTO"",
              ""language"": ""PYTHON"",
              ""path"": ""/notebook_path"",
              ""overwrite"": false
            }}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WorkspaceApiClient(hc);
        await client.Import("/notebook_path", ExportFormat.AUTO, Language.PYTHON, contentBytes, false);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }
}

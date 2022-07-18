// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class GlobalInitScriptsApiClientTest : ApiClientTest
{
    private static readonly Uri GlobalInitScriptsApiUri = new(BaseApiUri, "2.0/global-init-scripts");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
        {
            ""scripts"": [
              {
                ""created_at"": 1594437249910,
                ""created_by"": ""john.doe@databricks.com"",
                ""enabled"": false,
                ""name"": ""My example script name"",
                ""position"": 0,
                ""script_id"": ""714B166709FBD56F"",
                ""updated_at"": 1594444684786,
                ""updated_by"": ""jane.smith@example.com""
              }
            ]
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, GlobalInitScriptsApiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new GlobalInitScriptsApi(hc);

        var scriptsList = new { scripts = await client.List() };

        AssertJsonDeepEquals(expectedResponse, JsonSerializer.Serialize(scriptsList, Options));

        handler.VerifyRequest(
            HttpMethod.Get,
            GlobalInitScriptsApiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGet()
    {
        const string scriptId = "714B166709FBD56F";
        var apiUri = GlobalInitScriptsApiUri + "/" + scriptId;
        const string expectedResponse = @"
            {
              ""created_at"": 1594437249910,
              ""created_by"": ""john.doe@databricks.com"",
              ""enabled"": false,
              ""name"": ""My example script name"",
              ""position"": 0,
              ""script_id"": ""714B166709FBD56F"",
              ""updated_at"": 1594444684786,
              ""updated_by"": ""jane.smith@example.com"",
              ""script"": ""ZWNobyBoZWxsbw==""
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new GlobalInitScriptsApi(hc);
        var script = await client.Get(scriptId);
        Assert.AreEqual("echo hello", script.Script);
        var scriptJson = JsonSerializer.Serialize(script, Options);
        AssertJsonDeepEquals(expectedResponse, scriptJson);
        handler.VerifyRequest(HttpMethod.Get, apiUri, Times.Once());
    }

    [TestMethod]
    public async Task TestCreate()
    {
        const string expectedResponse = @"
            {
              ""script_id"": ""714B166709FBD56F""
            }
        ";

        const string expectedRequest = @"
            {
              ""enabled"": false,
              ""name"": ""My example script name"",
              ""position"": 0,
              ""script"": ""ZWNobyBoZWxsbw==""
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, GlobalInitScriptsApiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new GlobalInitScriptsApi(hc);

        var scriptId = await client.Create("My example script name", "echo hello", false, 0);

        Assert.AreEqual("714B166709FBD56F", scriptId);
        handler.VerifyRequest(HttpMethod.Post, GlobalInitScriptsApiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        const string scriptId = "714B166709FBD56F";
        var apiUri = GlobalInitScriptsApiUri + "/" + scriptId;

        const string expectedRequest = @"
            {
              ""enabled"": false,
              ""name"": ""My example script name"",
              ""position"": 0,
              ""script"": ""ZWNobyBoZWxsbw==""
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new GlobalInitScriptsApi(hc);

        await client.Update(scriptId, "My example script name", "echo hello", false, 0);

        Assert.AreEqual("714B166709FBD56F", scriptId);
        handler.VerifyRequest(HttpMethod.Patch, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestUpdatePartial()
    {
        const string scriptId = "714B166709FBD56F";
        var apiUri = GlobalInitScriptsApiUri + "/" + scriptId;

        const string expectedRequest = @"
            {
              ""enabled"": false,
              ""name"": ""My example script name""
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new GlobalInitScriptsApi(hc);

        await client.Update(scriptId, "My example script name", enabled: false);

        Assert.AreEqual("714B166709FBD56F", scriptId);
        handler.VerifyRequest(HttpMethod.Patch, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestDelete()
    {
        const string scriptId = "714B166709FBD56F";
        var apiUri = GlobalInitScriptsApiUri + "/" + scriptId;

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new GlobalInitScriptsApi(hc);

        await client.Delete(scriptId);

        Assert.AreEqual("714B166709FBD56F", scriptId);
        handler.VerifyRequest(HttpMethod.Delete, apiUri, Times.Once());
    }
}
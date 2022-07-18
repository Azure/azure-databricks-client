// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class TokenApiClientTest : ApiClientTest
{
    private static readonly Uri TokenApiUri = new(BaseApiUri, "2.0/token/");

    [TestMethod]
    public async Task TestCreate()
    {
        var apiUri = new Uri(TokenApiUri, "create");
        const string expectedRequest = @"{ ""comment"": ""This is an example token"", ""lifetime_seconds"": 7776000 }";
        const string expectedResponse = @"
            {
              ""token_value"": ""test_token_value"",
              ""token_info"": {
                ""token_id"": ""test_token_id"",
                ""creation_time"": 1626286601651,
                ""expiry_time"": 1634062601651,
                ""comment"": ""This is an example token""
              }
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new TokenApiClient(hc);

        var (tokenValue, tokenInfo) = await client.Create(7776000, "This is an example token");

        Assert.AreEqual("test_token_value", tokenValue);
        Assert.AreEqual("test_token_id", tokenInfo.TokenId);
        Assert.AreEqual(DateTimeOffset.FromUnixTimeMilliseconds(1626286601651), tokenInfo.CreationTime);
        Assert.AreEqual(DateTimeOffset.FromUnixTimeMilliseconds(1634062601651), tokenInfo.ExpiryTime);
        Assert.AreEqual("This is an example token", tokenInfo.Comment);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestList()
    {
        var apiUri = new Uri(TokenApiUri, "list");

        const string expectedResponse = @"
            {
              ""token_infos"": [
                {
                  ""token_id"": ""token_id_1"",
                  ""creation_time"": 1626286601651,
                  ""expiry_time"": 1634062601651,
                  ""comment"": ""This is an example token""
                },
                {
                  ""token_id"": ""token_id_2"",
                  ""creation_time"": 1626286906596,
                  ""expiry_time"": 1634062906596,
                  ""comment"": ""This is another example token""
                }
              ]
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new TokenApiClient(hc);

        var tokenInfoList = await client.List();

        AssertJsonDeepEquals(expectedResponse, JsonSerializer.Serialize(new { token_infos = tokenInfoList }, Options));

        handler.VerifyRequest(
            HttpMethod.Get,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRevoke()
    {
        var apiUri = new Uri(TokenApiUri, "delete");
        const string expectedRequest = @"{ ""token_id"": ""token_id_1"" }";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new TokenApiClient(hc);

        await client.Revoke("token_id_1");

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }
}
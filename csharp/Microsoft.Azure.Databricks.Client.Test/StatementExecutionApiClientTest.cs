// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class StatementExecutionApiClientTest : ApiClientTest
{
    private static readonly Uri StatementExecutionApiUri = new(BaseApiUri, "2.0/sql/statements");

    [TestMethod]
    public async Task TestExecute()
    {
        const string expectedRequest = "{\"statement\":\"string\",\"warehouse_id\":\"string\",\"parameters\":[]}";
        const string expectedResponse = @"
                {
                  ""statement_id"": ""string"",
                  ""status"": {
                        ""state"": ""SUCCEEDED""
                  },
                  ""manifest"": {
                      ""format"": ""JSON_ARRAY"",
                      ""schema"": {
                          ""column_count"": 1,
                          ""columns"": [
                              {
                                  ""name"": ""string"",
                                  ""position"": 0,
                                  ""type_name"": ""string"",
                                  ""type_text"": ""string""
                              }
                          ]
                      }
                  },
                  ""result"": {
                      ""chunk_index"": 0,
                      ""row_offset"": 0,
                      ""row_count"": 1,
                      ""data_array"": [
                        [ ""0"" ]
                      ]
                  }
                }
                ";

        var expected = JsonSerializer.Deserialize<StatementExecution>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, StatementExecutionApiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expected, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new StatementExecutionApiClient(hc);
        var statement = JsonNode.Parse(expectedRequest).Deserialize<SqlStatement>(Options);

        var actual = await client.Execute(statement);
        Assert.AreEqual(expected, actual);

        handler.VerifyRequest(
            HttpMethod.Post,
            StatementExecutionApiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGet()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{StatementExecutionApiUri}/{testId}";

        const string expectedResponse = @"
                {
                  ""statement_id"": ""string"",
                  ""status"": {
                        ""state"": ""SUCCEEDED""
                  },
                  ""manifest"": {
                      ""format"": ""JSON_ARRAY"",
                      ""schema"": {
                          ""column_count"": 1,
                          ""columns"": [
                              {
                                  ""name"": ""string"",
                                  ""position"": 0,
                                  ""type_name"": ""string"",
                                  ""type_text"": ""string""
                              }
                          ]
                      }
                  },
                  ""result"": {
                      ""chunk_index"": 0,
                      ""row_offset"": 0,
                      ""row_count"": 1,
                      ""data_array"": [
                        [ ""0"" ]
                      ]
                  }
                }
                ";

        var expected = JsonSerializer.Deserialize<StatementExecution>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new StatementExecutionApiClient(hc);
        var actual = await client.Get(testId);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TestCancel()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{StatementExecutionApiUri}/{testId}/cancel";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK)
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new StatementExecutionApiClient(hc);
        await client.Cancel(testId);
        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGetResultChunk()
    {
        string testId = "1234-567890-cited123";
        int chunkIndex = 0;
        string apiUri = $"{StatementExecutionApiUri}/{testId}/result/chunks/{chunkIndex}";

        const string expectedResponse = @"
                {
                    ""chunk_index"": 0,
                    ""row_offset"": 0,
                    ""row_count"": 1,
                    ""data_array"": [
                        [ ""0"" ]
                    ]
                }
                ";

        var expected = JsonSerializer.Deserialize<StatementExecutionResultChunk>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new StatementExecutionApiClient(hc);
        var actual = await client.GetResultChunk(testId, chunkIndex);
        Assert.AreEqual(expected, actual);
    }
}

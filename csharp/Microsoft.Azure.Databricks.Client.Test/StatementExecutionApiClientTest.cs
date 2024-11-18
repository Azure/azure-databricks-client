﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
    public record Person
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public static Person FromJsonArray(JsonArray array, StatementExecutionSchema schema) => new(array);

        public Person(JsonArray array)
        {
            Id = array[0]?.GetValue<string>() ?? string.Empty;
            Firstname = array[1]?.GetValue<string>() ?? string.Empty;
            Lastname = array[2]?.GetValue<string>() ?? string.Empty;
        }
    }

    private static readonly Uri StatementExecutionApiUri = new(BaseApiUri, "2.0/sql/statements");

    [TestMethod]
    public void TestDeserialization()
    {
        const string result = @"
        {
            ""chunk_index"": 0,
            ""row_offset"": 0,
            ""row_count"": 2,
            ""data_array"": [
              [
                ""id1"", ""Anthony"", ""Clark""
              ],
              [
                ""id2"", ""Clarissa"", ""Bradley""
              ]
            ]
        }
        ";

        var deserialized = JsonSerializer.Deserialize<StatementExecutionResultChunk>(result, Options);
        Assert.IsNotNull(deserialized);

        var columns = new StatementExecutionSchemaColumn[] {
            new() { Name = "id", Position = 0, TypeName = "string", TypeText = "string" },
            new() { Name = "firstname", Position = 1, TypeName = "string", TypeText = "string" },
            new() { Name = "lastname", Position = 2, TypeName = "string", TypeText = "string" }
        };

        var execution = new StatementExecution
        {
            Manifest = new StatementExecutionManifest { Schema = new StatementExecutionSchema { ColumnCount = 3, Columns = columns }, Format = StatementFormat.JSON_ARRAY },
            Result = deserialized
        };

        var leads = execution.DeserializeResults(Person.FromJsonArray);
        Assert.AreEqual(2, leads.Count());
    }

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

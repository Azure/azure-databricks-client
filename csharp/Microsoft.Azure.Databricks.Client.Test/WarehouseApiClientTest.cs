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
public class WarehouseApiClientTest : ApiClientTest
{
    private static readonly Uri WarehouseApiUri = new(BaseApiUri, "2.0/sql/warehouses");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
                {
                  ""warehouses"": [
                    {
                      ""id"": ""string"",
                      ""name"": ""string"",
                      ""cluster_size"": ""2X-Small"",
                      ""min_num_clusters"": 1,
                      ""max_num_clusters"": 0,
                      ""auto_stop_mins"": 120,
                      ""creator_name"": ""string"",
                      ""instance_profile_arn"": ""string"",
                      ""spot_instance_policy"": ""POLICY_UNSPECIFIED"",
                      ""enable_photon"": true,
                      ""channel"": {
                        ""name"": ""CHANNEL_NAME_UNSPECIFIED"",
                        ""dbsql_version"": ""string""
                      },
                      ""enable_serverless_compute"": true,
                      ""warehouse_type"": ""TYPE_UNSPECIFIED"",
                      ""num_clusters"": 0,
                      ""num_active_sessions"": 0,
                      ""state"": ""STARTING"",
                      ""jdbc_url"": ""string"",
                      ""odbc_params"": {
                        ""hostname"": ""string"",
                        ""path"": ""string"",
                        ""protocol"": ""string"",
                        ""port"": 0
                      },
                      ""health"": {
                        ""status"": ""STATUS_UNSPECIFIED"",
                        ""message"": ""string"",
                        ""failure_reason"": {
                          ""code"": ""UNKNOWN"",
                          ""type"": ""SUCCESS""
                        },
                        ""summary"": ""string"",
                        ""details"": ""string""
                      }
                    }
                  ]
                }
                ";

        var expected = JsonNode.Parse(expectedResponse)?["warehouses"].Deserialize<IEnumerable<WarehouseInfo>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, WarehouseApiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        var actual = await client.List();
        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestGet()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{WarehouseApiUri}/{testId}";

        const string expectedResponse = @"
                {
                  ""id"": ""string"",
                  ""name"": ""string"",
                  ""cluster_size"": ""string"",
                  ""min_num_clusters"": 1,
                  ""max_num_clusters"": 10,
                  ""auto_stop_mins"": 120,
                  ""creator_name"": ""string"",
                  ""spot_instance_policy"": ""POLICY_UNSPECIFIED"",
                  ""enable_photon"": true,
                  ""channel"": {
                    ""name"": ""CHANNEL_NAME_UNSPECIFIED"",
                    ""dbsql_version"": ""string""
                  },
                  ""enable_serverless_compute"": true,
                  ""warehouse_type"": ""TYPE_UNSPECIFIED"",
                  ""num_clusters"": 0,
                  ""num_active_sessions"": 0,
                  ""state"": ""STARTING"",
                  ""jdbc_url"": ""string"",
                  ""odbc_params"": {
                    ""hostname"": ""string"",
                    ""path"": ""string"",
                    ""protocol"": ""string"",
                    ""port"": 0
                  },
                  ""health"": {
                    ""status"": ""STATUS_UNSPECIFIED"",
                    ""message"": ""string"",
                    ""failure_reason"": {
                      ""code"": ""UNKNOWN"",
                      ""type"": ""SUCCESS""
                    },
                    ""summary"": ""string"",
                    ""details"": ""string""
                  }
                }
                ";

        var expected = JsonSerializer.Deserialize<WarehouseInfo>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        var actual = await client.Get(testId);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        const string expectedRequest = "{\"name\":\"string\",\"cluster_size\":\"string\",\"min_num_clusters\":1,\"max_num_clusters\":5,\"auto_stop_mins\":120,\"creator_name\":\"string\",\"tags\":{\"custom_tags\":[{\"key\":\"string\",\"value\":\"string\"}]},\"spot_instance_policy\":\"POLICY_UNSPECIFIED\",\"enable_photon\":true,\"channel\":{\"name\":\"CHANNEL_NAME_UNSPECIFIED\",\"dbsql_version\":\"string\"},\"enable_serverless_compute\":true,\"warehouse_type\":\"TYPE_UNSPECIFIED\"}";
        var expectedResponse = new { id = "1234-567890-cited123" };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, WarehouseApiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        var warehouse = JsonNode.Parse(expectedRequest).Deserialize<WarehouseAttributes>(Options);

        var id = await client.Create(warehouse);
        Assert.AreEqual(expectedResponse.id, id);

        handler.VerifyRequest(
            HttpMethod.Post,
            WarehouseApiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{WarehouseApiUri}/{testId}/edit";
        const string expectedRequest = "{\"name\":\"string\",\"cluster_size\":\"string\",\"min_num_clusters\":1,\"max_num_clusters\":5,\"auto_stop_mins\":120,\"creator_name\":\"string\",\"tags\":{\"custom_tags\":[{\"key\":\"string\",\"value\":\"string\"}]},\"spot_instance_policy\":\"POLICY_UNSPECIFIED\",\"enable_photon\":true,\"channel\":{\"name\":\"CHANNEL_NAME_UNSPECIFIED\",\"dbsql_version\":\"string\"},\"enable_serverless_compute\":true,\"warehouse_type\":\"TYPE_UNSPECIFIED\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK)
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        var warehouse = JsonNode.Parse(expectedRequest).Deserialize<WarehouseAttributes>(Options);

        await client.Update(testId, warehouse);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestStart()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{WarehouseApiUri}/{testId}/start";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK)
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        await client.Start(testId);
        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestStop()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{WarehouseApiUri}/{testId}/stop";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK)
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        await client.Stop(testId);
        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestDelete()
    {
        string testId = "1234-567890-cited123";
        string apiUri = $"{WarehouseApiUri}/{testId}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, apiUri)
            .ReturnsResponse(HttpStatusCode.OK)
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new WarehouseApiClient(hc);
        await client.Delete(testId);
        handler.VerifyRequest(
            HttpMethod.Delete,
            apiUri,
            Times.Once()
        );
    }
}

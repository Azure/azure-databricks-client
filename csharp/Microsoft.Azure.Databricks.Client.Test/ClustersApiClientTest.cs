
using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class ClustersApiClientTest: ApiClientTest
{
    private static readonly Uri ClusterApiUri = new(BaseApiUri, "2.0/clusters/");

    [TestMethod]
    public async Task TestCreateFixedWorker()
    {
        var apiUri = new Uri(ClusterApiUri, "create");
        const string expectedRequest = "{\"cluster_name\": \"my-cluster\",\"spark_version\": \"7.3.x-scala2.12\",\"node_type_id\": \"Standard_D3_v2\",\"spark_conf\": {\"spark.speculation\": \"true\"},\"num_workers\": 25}";
        var expectedResponse = new { cluster_id = "1234-567890-cited123" };
            
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var clusterInfo = ClusterAttributes.GetNewClusterConfiguration("my-cluster")
            .WithNodeType("Standard_D3_v2")
            .WithNumberOfWorkers(25)
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3);
        clusterInfo.SparkConfiguration = new Dictionary<string, string> { { "spark.speculation", "true" } };

        var clusterId = await client.Create(clusterInfo);
        Assert.AreEqual(expectedResponse.cluster_id, clusterId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestCreateAutoScale()
    {
        var apiUri = new Uri(ClusterApiUri, "create");
        const string expectedRequest = "{\"cluster_name\":\"autoscaling-cluster\",\"spark_version\":\"7.3.x-scala2.12\",\"node_type_id\":\"Standard_D3_v2\",\"autoscale\":{\"min_workers\":2,\"max_workers\":50}}";
        var expectedResponse = new { cluster_id = "1234-567890-hared123" };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var clusterInfo = ClusterAttributes.GetNewClusterConfiguration("autoscaling-cluster")
            .WithNodeType("Standard_D3_v2")
            .WithAutoScale(2, 50)
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3);

        var clusterId = await client.Create(clusterInfo);
        Assert.AreEqual(expectedResponse.cluster_id, clusterId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestCreateSingleNode()
    {
        var apiUri = new Uri(ClusterApiUri, "create");
        const string expectedRequest = "{\"cluster_name\":\"single-node-cluster\",\"spark_version\":\"7.3.x-scala2.12\",\"node_type_id\":\"Standard_D3_v2\",\"num_workers\":0,\"spark_conf\":{\"spark.databricks.cluster.profile\":\"singleNode\",\"spark.master\":\"local[*]\"},\"custom_tags\":{\"ResourceClass\":\"SingleNode\"}}";
        var expectedResponse = new { cluster_id = "1234-567890-pouch123" };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var clusterInfo = ClusterAttributes.GetNewClusterConfiguration("single-node-cluster")
            .WithNodeType("Standard_D3_v2")
            .WithClusterMode(ClusterMode.SingleNode)
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3);

        string clusterId = await client.Create(clusterInfo);
        Assert.AreEqual(expectedResponse.cluster_id, clusterId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestCreateWithIdempotencyToken()
    {
        const string idempotencyToken = "test_idempotency_token";
        var apiUri = new Uri(ClusterApiUri, "create");
        const string expectedRequest = @"
            {
              ""cluster_name"": ""single-node-cluster"",
              ""spark_version"": ""7.3.x-scala2.12"",
              ""node_type_id"": ""Standard_D3_v2"",
              ""num_workers"": 0,
              ""spark_conf"": {
                ""spark.databricks.cluster.profile"": ""singleNode"",
                ""spark.master"": ""local[*]""
              },
              ""custom_tags"": { ""ResourceClass"": ""SingleNode"" },
              ""idempotency_token"": ""test_idempotency_token""
            }
            ";
        var expectedResponse = new { cluster_id = "1234-567890-pouch123" };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var clusterAttributes = ClusterAttributes.GetNewClusterConfiguration("single-node-cluster")
            .WithNodeType("Standard_D3_v2")
            .WithClusterMode(ClusterMode.SingleNode)
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3);

        var clusterId = await client.Create(clusterAttributes, idempotencyToken);
        Assert.AreEqual(expectedResponse.cluster_id, clusterId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestEdit()
    {
        var apiUri = new Uri(ClusterApiUri, "edit");
        const string expectedRequest = "{\"cluster_id\":\"1202-211320-brick1\",\"num_workers\":10,\"spark_version\":\"7.3.x-scala2.12\",\"node_type_id\":\"Standard_D3_v2\"}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var clusterInfo = new ClusterInfo()
            .WithNumberOfWorkers(10)
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3)
            .WithNodeType("Standard_D3_v2");

        await client.Edit("1202-211320-brick1", clusterInfo);
            
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
        var apiUri = new Uri(ClusterApiUri, "start");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-reef123\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Start("1234-567890-reef123");

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRetart()
    {
        var apiUri = new Uri(ClusterApiUri, "restart");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-reef123\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Restart("1234-567890-reef123");

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestResize()
    {
        var apiUri = new Uri(ClusterApiUri, "resize");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-reef123\",\"num_workers\":30}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Resize("1234-567890-reef123", 30);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestTerminate()
    {
        var apiUri = new Uri(ClusterApiUri, "delete");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-frays123\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Terminate("1234-567890-frays123");

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var apiUri = new Uri(ClusterApiUri, "permanent-delete");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-frays123\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Delete("1234-567890-frays123");

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGet()
    {
        var apiUri = new Uri(ClusterApiUri, "get");
        const string expectedResponse = @"
                {
                    ""cluster_id"": ""1234-567890-reef123"",
                    ""driver"": {
                        ""node_id"": ""dced0ce388954c38abef081f54c18afd"",
                        ""instance_id"": ""c69c0b119a2a499d8a2843c4d256136a"",
                        ""start_timestamp"": 1619718438896,
                        ""host_private_ip"": ""10.0.0.1"",
                        ""private_ip"": ""10.0.0.2""
                    },
                    ""spark_context_id"": 5631707659504820000,
                    ""jdbc_port"": 10000,
                    ""cluster_name"": ""my-cluster"",
                    ""spark_version"": ""8.2.x-scala2.12"",
                    ""node_type_id"": ""Standard_L4s"",
                    ""driver_node_type_id"": ""Standard_L4s"",
                    ""autotermination_minutes"": 0,
                    ""enable_elastic_disk"": true,
                    ""disk_spec"": {},
                    ""cluster_source"": ""UI"",
                    ""enable_local_disk_encryption"": false,
                    ""azure_attributes"": {
                        ""first_on_demand"": 1,
                        ""availability"": ""ON_DEMAND_AZURE"",
                        ""spot_bid_max_price"": -1
                    },
                    ""instance_source"": {
                        ""node_type_id"": ""Standard_L4s""
                    },
                    ""driver_instance_source"": {
                        ""node_type_id"": ""Standard_L4s""
                    },
                    ""state"": ""RUNNING"",
                    ""state_message"": """",
                    ""start_time"": 1610745129764,
                    ""last_state_loss_time"": 1619718513513,
                    ""num_workers"": 0,
                    ""cluster_memory_mb"": 32768,
                    ""cluster_cores"": 4,
                    ""creator_user_name"": ""someone@example.com"",
                    ""pinned_by_user_name"": ""3401478490056118"",
                    ""init_scripts_safe_mode"": false
                }
                ";

        var expected = JsonSerializer.Deserialize<ClusterInfo>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(apiUri, "?cluster_id=1234-567890-reef123"))
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var actual = await client.Get("1234-567890-reef123");

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public async Task TestPin()
    {
        var apiUri = new Uri(ClusterApiUri, "pin");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-reef123\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Pin("1234-567890-reef123");

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestUnPin()
    {
        var apiUri = new Uri(ClusterApiUri, "unpin");
        const string expectedRequest = "{\"cluster_id\":\"1234-567890-reef123\"}";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        await client.Unpin("1234-567890-reef123");

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
        var apiUri = new Uri(ClusterApiUri, "list");
        const string expectedResponse = @"
                {
                    ""clusters"": [{
                        ""cluster_id"": ""1234-567890-reef123"",
                        ""driver"": {
                            ""node_id"": ""dced0ce388954c38abef081f54c18afd"",
                            ""instance_id"": ""c69c0b119a2a499d8a2843c4d256136a"",
                            ""start_timestamp"": 1619718438896,
                            ""host_private_ip"": ""10.0.0.1"",
                            ""private_ip"": ""10.0.0.2""
                        },
                        ""spark_context_id"": 5631707659504820000,
                        ""jdbc_port"": 10000,
                        ""cluster_name"": ""my-cluster"",
                        ""spark_version"": ""8.2.x-scala2.12"",
                        ""node_type_id"": ""Standard_L4s"",
                        ""driver_node_type_id"": ""Standard_L4s"",
                        ""autotermination_minutes"": 0,
                        ""enable_elastic_disk"": true,
                        ""disk_spec"": {},
                        ""cluster_source"": ""UI"",
                        ""enable_local_disk_encryption"": false,
                        ""azure_attributes"": {
                            ""first_on_demand"": 1,
                            ""availability"": ""ON_DEMAND_AZURE"",
                            ""spot_bid_max_price"": -1
                        },
                        ""instance_source"": {
                            ""node_type_id"": ""Standard_L4s""
                        },
                        ""driver_instance_source"": {
                            ""node_type_id"": ""Standard_L4s""
                        },
                        ""state"": ""RUNNING"",
                        ""state_message"": """",
                        ""start_time"": 1610745129764,
                        ""last_state_loss_time"": 1619718513513,
                        ""num_workers"": 0,
                        ""cluster_memory_mb"": 32768,
                        ""cluster_cores"": 4,
                        ""creator_user_name"": ""someone@example.com"",
                        ""pinned_by_user_name"": ""3401478490056118"",
                        ""init_scripts_safe_mode"": false
                    }]
                }
                ";

        var expected = JsonNode.Parse(expectedResponse)?["clusters"].Deserialize<IEnumerable<ClusterInfo>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var actual = await client.List();
        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestListNodeTypes()
    {
        var apiUri = new Uri(ClusterApiUri, "list-node-types");
        const string expectedResponse = @"
                {
                    ""node_types"": [{
                        ""node_type_id"": ""Standard_L80s_v2"",
                        ""memory_mb"": 655360,
                        ""num_cores"": 80,
                        ""description"": ""Standard_L80s_v2"",
                        ""instance_type_id"": ""Standard_L80s_v2"",
                        ""is_deprecated"": false,
                        ""category"": ""Storage Optimized"",
                        ""support_ebs_volumes"": true,
                        ""support_cluster_tags"": true,
                        ""num_gpus"": 0,
                        ""node_instance_type"": {
                            ""instance_type_id"": ""Standard_L80s_v2"",
                            ""local_disks"": 1,
                            ""local_disk_size_gb"": 800,
                            ""instance_family"": ""Standard LSv2 Family vCPUs"",
                            ""local_nvme_disk_size_gb"": 1788,
                            ""local_nvme_disks"": 10,
                            ""swap_size"": ""10g""
                        },
                        ""is_hidden"": false,
                        ""support_port_forwarding"": true,
                        ""display_order"": 0,
                        ""is_io_cache_enabled"": true,
                        ""node_info"": {
                            ""available_core_quota"": 350,
                            ""total_core_quota"": 350
                        }
                    }]
                }
                ";

        var expected = JsonNode.Parse(expectedResponse)?["node_types"].Deserialize<IEnumerable<NodeType>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var actual = await client.ListNodeTypes();
        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestListSparkVersions()
    {
        var apiUri = new Uri(ClusterApiUri, "spark-versions");
        const string expectedResponse = @"
                {
                    ""versions"": [{
                        ""key"": ""8.2.x-scala2.12"",
                        ""name"": ""8.2 (includes Apache Spark 3.1.1, Scala 2.12)""
                    }, {
                        ""key"": ""10.4.x-scala2.12"",
                        ""name"": ""10.4 (includes Apache Spark 3.2.1, Scala 2.12)""
                    }]
                }
                ";

        var expected = JsonNode.Parse(expectedResponse)?["versions"]?.AsArray().ToDictionary(
            e => e?["key"]?.GetValue<string>() ?? "",
            e => e?["name"]?.GetValue<string>() ?? ""
        );

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var actual = await client.ListSparkVersions();
        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestEvents()
    {
        var apiUri = new Uri(ClusterApiUri, "events");
        const string expectedRequest = @"
                {
                    ""cluster_id"": ""1234-567890-reef123"",
                    ""start_time"": 1617238800000,
                    ""end_time"": 1619485200000,
                    ""order"": ""DESC"",
                    ""offset"": 5,
                    ""limit"": 5,
                    ""event_types"": [""RUNNING""]
                }
                ";
        const string expectedResponse = @"
                {
                    ""events"": [{
                        ""cluster_id"": ""1234-567890-reef123"",
                        ""timestamp"": 1619471498409,
                        ""type"": ""RUNNING"",
                        ""details"": {
                            ""current_num_workers"": 2,
                            ""target_num_workers"": 2
                        }
                    }],
                    ""next_page"": {
                        ""cluster_id"": ""1234-567890-reef123"",
                        ""start_time"": 1617238800000,
                        ""end_time"": 1619485200000,
                        ""order"": ""DESC"",
                        ""offset"": 10,
                        ""limit"": 5
                    },
                    ""total_count"": 25
                }
                ";

        var expected = JsonSerializer.Deserialize<EventsResponse>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClustersApiClient(hc);
        var actual = await client.Events(
            "1234-567890-reef123",
            DateTimeOffset.FromUnixTimeMilliseconds(1617238800000),
            DateTimeOffset.FromUnixTimeMilliseconds(1619485200000),
            ListOrder.DESC,
            new[] { ClusterEventType.RUNNING },
            5,
            5
        );

        CollectionAssert.AreEqual(expected?.Events.ToList(), actual.Events.ToList());
        Assert.AreEqual(expected?.TotalCount, actual.TotalCount);
        Assert.AreEqual(expected?.HasNextPage, actual.HasNextPage);
        Assert.AreEqual(expected?.NextPage, actual.NextPage);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }
}
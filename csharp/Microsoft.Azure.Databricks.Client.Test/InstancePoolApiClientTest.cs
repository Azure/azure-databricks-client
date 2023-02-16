// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using Polly;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Policy = Polly.Policy;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class InstancePoolApiClientTest : ApiClientTest
{
    private static readonly Uri InstancePoolApiUri = new(BaseApiUri, "2.0/instance-pools/");

    [TestMethod]
    public async Task TestList()
    {
        var apiUri = new Uri(InstancePoolApiUri, "list");
        const string expectedResponse = @"
                { 
                    ""instance_pools"": [
                        {
                            ""instance_pool_name"": ""Pool1"",
                            ""min_idle_instances"": 0,
                            ""max_capacity"": 5,
                            ""node_type_id"": ""Standard_DS3_v2"",
                            ""idle_instance_autotermination_minutes"": 20,
                            ""enable_elastic_disk"": true,
                            ""preloaded_spark_versions"": [
                                ""9.1.x-scala2.12""
                            ],
                            ""azure_attributes"": {
                                ""availability"": ""ON_DEMAND_AZURE""
                            },
                            ""instance_pool_id"": ""1234-567890-abcd123-pool-12abcde3"",
                            ""default_tags"": {
                                ""Vendor"": ""Databricks"",
                                ""DatabricksInstancePoolCreatorId"": ""1234567890123456"",
                                ""DatabricksInstancePoolId"": ""1234-567890-abcd123-pool-12abcde3"",
                                ""DatabricksInstanceGroupId"": ""-1234567890123456789""
                            },
                            ""state"": ""ACTIVE"",
                            ""stats"": {
                                ""used_count"": 0,
                                ""idle_count"": 0,
                                ""pending_used_count"": 0,
                                ""pending_idle_count"": 0
                            },
                            ""status"": {}
                        }
                    ]
                }
                ";

        var expected = JsonNode.Parse(expectedResponse)?["instance_pools"].Deserialize<IEnumerable<InstancePoolInfo>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new InstancePoolApiClient(hc);
        var actual = (await client.List()).ToList();

        var actualJson = JsonSerializer.Serialize(new { instance_pools = actual }, Options);
        var expectedJson = JsonSerializer.Serialize(new { instance_pools = expected }, Options);

        AssertJsonDeepEquals(expectedJson, actualJson);
    }
}
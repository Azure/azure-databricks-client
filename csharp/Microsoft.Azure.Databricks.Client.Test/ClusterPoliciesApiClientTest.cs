// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class ClusterPoliciesApiClientTest : ApiClientTest
{
    private static readonly Uri ClusterPoliciesApiUri = new(BaseApiUri, "2.0/policies/clusters/");

    [TestMethod]
    public async Task TestGet()
    {
        var apiUri = new Uri(ClusterPoliciesApiUri, "get?policy_id=ABCD000000000000");
        const string expectedResponse = @"
            {
              ""policy_id"": ""ABCD000000000000"",
              ""name"": ""Test policy"",
              ""definition"": ""{\""spark_conf.spark.databricks.cluster.profile\"":{\""type\"":\""forbidden\"",\""hidden\"":true}}"",
              ""created_at_timestamp"": 1600000000000
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        var policy = await client.Get("ABCD000000000000");
        var policyJson = JsonSerializer.Serialize(policy, Options);
        AssertJsonDeepEquals(expectedResponse, policyJson);

        handler.VerifyRequest(HttpMethod.Get, apiUri, Times.Once());
    }

    [TestMethod]
    public async Task TestList()
    {
        var apiUri = new Uri(ClusterPoliciesApiUri, "list?sort_order=ASC&sort_column=POLICY_CREATION_TIME");
        const string expectedResponse = @"
        {
          ""policies"": [
            {
              ""policy_id"": ""ABCD000000000001"",
              ""name"": ""Empty"",
              ""definition"": ""{}"",
              ""created_at_timestamp"": 1600000000002
            },
            {
              ""policy_id"": ""ABCD000000000000"",
              ""name"": ""Test policy"",
              ""definition"": ""{\""spark_conf.spark.databricks.cluster.profile\"":{\""type\"":\""forbidden\"",\""hidden\"":true}}"",
              ""created_at_timestamp"": 1600000000000
            }
          ],
          ""total_count"": 2
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        var policies = (await client.List(ListOrder.ASC)).ToList();
        var policiesJson = JsonSerializer.Serialize(new { policies, total_count = policies.Count }, Options);
        AssertJsonDeepEquals(expectedResponse, policiesJson);

        handler.VerifyRequest(HttpMethod.Get, apiUri, Times.Once());
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var apiUri = new Uri(ClusterPoliciesApiUri, "create");
        const string expectedRequest = @"
            {
              ""name"": ""Test policy"",
              ""definition"": ""{\""spark_conf.spark.databricks.cluster.profile\"":{\""type\"":\""forbidden\"",\""hidden\"":true}}""
            }
        ";
        const string expectedResponse = @"
        { ""policy_id"": ""ABCD000000000000"" }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        var policyId = await client.Create("Test policy", "{\"spark_conf.spark.databricks.cluster.profile\":{\"type\":\"forbidden\",\"hidden\":true}}");
        Assert.AreEqual("ABCD000000000000", policyId);

        handler.VerifyRequest(HttpMethod.Post, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestEdit()
    {
        var apiUri = new Uri(ClusterPoliciesApiUri, "edit");
        const string expectedRequest = @"
            {
              ""policy_id"": ""ABCD000000000000"",
              ""name"": ""Test policy"",
              ""definition"": ""{\""spark_conf.spark.databricks.cluster.profile\"":{\""type\"":\""forbidden\"",\""hidden\"":true}}""
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        await client.Edit("ABCD000000000000", "Test policy", "{\"spark_conf.spark.databricks.cluster.profile\":{\"type\":\"forbidden\",\"hidden\":true}}");
        handler.VerifyRequest(HttpMethod.Post, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var apiUri = new Uri(ClusterPoliciesApiUri, "delete");
        const string expectedRequest = @"
            { ""policy_id"": ""ABCD000000000000"" }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        await client.Delete("ABCD000000000000");
        handler.VerifyRequest(HttpMethod.Post, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestGetPolicyFamily()
    {
        var apiUri = new Uri(BaseApiUri, "2.0/policy-families/personal-vm");
        const string expectedResponse = @"
            {
              ""policy_family_id"": ""personal-vm"",
              ""name"": ""Personal Compute"",
              ""description"":""Use with small-to-medium data or libraries like pandas and scikit-learn. Spark runs in local mode."",
              ""definition"": ""{}"",
              ""version"": 1
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        var policy = await client.GetPolicyFamily("personal-vm");
        var policyJson = JsonSerializer.Serialize(policy, Options);
        AssertJsonDeepEquals(expectedResponse, policyJson);

        handler.VerifyRequest(HttpMethod.Get, apiUri, Times.Once());
    }

    [TestMethod]
    public async Task TestListPolicyFamilies()
    {
        var apiUri = new Uri(BaseApiUri, "2.0/policy-families?max_results=20");
        const string expectedResponse = @"
            {
              ""policy_families"": [
                  {
                    ""policy_family_id"": ""personal-vm"",
                    ""name"": ""Personal Compute"",
                    ""description"":""Use with small-to-medium data or libraries like pandas and scikit-learn. Spark runs in local mode."",
                    ""definition"": ""{}"",
                    ""version"": 1
                  }
              ]
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new ClusterPoliciesApiClient(hc);
        var policyFamilies = (await client.ListPolicyFamily()).Item1;
        var policyJson = JsonSerializer.Serialize(new { policy_families = policyFamilies }, Options);
        AssertJsonDeepEquals(expectedResponse, policyJson);

        handler.VerifyRequest(HttpMethod.Get, apiUri, Times.Once());
    }
}
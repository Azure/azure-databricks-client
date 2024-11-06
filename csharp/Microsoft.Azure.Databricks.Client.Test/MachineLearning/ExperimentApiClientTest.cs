

using Microsoft.Azure.Databricks.Client.MachineLearning;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Azure.Databricks.Client.Models.MachineLearning.Experiment;

namespace Microsoft.Azure.Databricks.Client.Test.MachineLearning;

[TestClass]
public class ExperimentApiClientTests : MachineLearningApiClientTest
{
    [TestMethod]
    public async Task GetRunTest()
    {
        var run_id = "19d13cbfa8134b699f8b41fdab0cdd4c";
        var requestUri = $"{MlflowBaseUri}/runs/get?run_id={run_id}";
        var expectedResponse = @"
        {
          ""run"": {
            ""info"": {
              ""run_id"": ""string"",
              ""run_uuid"": ""string"",
              ""experiment_id"": ""string"",
              ""user_id"": ""string"",
              ""status"": ""RUNNING"",
              ""start_time"": 0,
              ""end_time"": 0,
              ""artifact_uri"": ""string"",
              ""lifecycle_stage"": ""string""
            },
            ""data"": {
              ""metrics"": [
                {
                  ""key"": ""string"",
                  ""value"": 0.1,
                  ""timestamp"": 0,
                  ""step"": 0
                }
              ],
              ""params"": [
                {
                  ""key"": ""string"",
                  ""value"": ""string""
                }
              ],
              ""tags"": [
                {
                  ""key"": ""string"",
                  ""value"": ""string""
                }
              ]
            },
            ""inputs"": {
              ""dataset_inputs"": [
                {
                  ""tags"": [
                    {
                      ""key"": ""string"",
                      ""value"": ""string""
                    }
                  ],
                  ""dataset"": {
                    ""name"": ""string"",
                    ""digest"": ""string"",
                    ""source_type"": ""string"",
                    ""source"": ""string"",
                    ""schema"": ""string"",
                    ""profile"": ""string""
                  }
                }
              ]
            }
          }
        }";

        var handler = CreateMockHandler();
        handler
        .SetupRequest(HttpMethod.Get, requestUri)
        .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new ExperimentApiClient(mockClient);
        var response = await client.GetRun(run_id);

        var expected = JsonNode.Parse(expectedResponse)?["run"].Deserialize<Run>(Options);

        Assert.IsNotNull(expected, "Expected object is null.");
        Assert.IsNotNull(response, "Response object is null.");

        if (response != null && expected != null)
        {
            Assert.AreEqual(expected.ToString(), response.ToString());
        }
    }
}
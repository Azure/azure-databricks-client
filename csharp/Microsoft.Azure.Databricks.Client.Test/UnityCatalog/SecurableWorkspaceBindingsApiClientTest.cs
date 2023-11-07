using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class SecurableWorkspaceBindingsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri SecurableBindingsApiUri = new(BaseApiUri, "bindings");

    [TestMethod]
    public async Task TestGet()
    {
        var securableType = "securable-type";
        var securableName = "securable-kind";

        const string expectedResponse = @"
        {
          ""bindings"": [
            {
              ""workspace_id"": 0,
              ""binding_type"": ""BINDING_TYPE_READ_WRITE""
            }
          ]
        }
";
        var requestUri = $"{SecurableBindingsApiUri}/{securableType}/{securableName}";
        var expected = JsonNode.Parse(expectedResponse)?["bindings"].Deserialize<IEnumerable<SecurableWorkspaceBinding>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SecurableWorkspaceBindingsApiClient(mockClient);
        var actual = await client.Get(
            securableType,
            securableName);

        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var securableType = "securable-type";
        var securableName = "securable-kind";
        var requestUri = $"{SecurableBindingsApiUri}/{securableType}/{securableName}";

        // new values, same as in expected request
        var add = new List<SecurableWorkspaceBinding>()
        {
            new SecurableWorkspaceBinding()
            {
                WorkspaceId = 1,
                BindingType = BindingType.BINDING_TYPE_READ_WRITE
            }
        };

        var remove = new List<SecurableWorkspaceBinding>()
        {
            new SecurableWorkspaceBinding()
            {
                WorkspaceId = 1,
                BindingType = BindingType.BINDING_TYPE_READ_WRITE
            }
        };


        var expectedRequest = @"
        {
          ""add"": [
            {
              ""workspace_id"": 1,
              ""binding_type"": ""BINDING_TYPE_READ_WRITE""
            }
          ],
          ""remove"": [
            {
              ""workspace_id"": 1,
              ""binding_type"": ""BINDING_TYPE_READ_WRITE""
            }
          ]
        }
        ";

        var expectedResponse = @"
        {
          ""bindings"": [
            {
              ""workspace_id"": 1,
              ""binding_type"": ""BINDING_TYPE_READ_WRITE""
            }
          ]
        }
        ";
        var expected = JsonNode.Parse(expectedResponse)?["bindings"].Deserialize<IEnumerable<SecurableWorkspaceBinding>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SecurableWorkspaceBindingsApiClient(mockClient);
        var response = await client.Update
            (securableType,
            securableName,
            add,
            remove);

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }
}

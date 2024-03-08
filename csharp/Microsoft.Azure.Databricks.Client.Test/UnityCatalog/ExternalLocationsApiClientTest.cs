using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class ExternalLocationsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri ExternalLocationsApiUri = new(BaseApiUri, "external-locations");

    [TestMethod]
    public async Task TestList()
    {
        var requestUri = ExternalLocationsApiUri;

        var expectedResponse = @"
        {
            ""external_locations"": [
            {
                ""name"": ""string"",
                ""url"": ""string"",
                ""credential_name"": ""string"",
                ""read_only"": true,
                ""comment"": ""string"",
                ""owner"": ""string"",
                ""metastore_id"": ""string"",
                ""credential_id"": ""string"",
                ""created_at"": 0,
                ""created_by"": ""string"",
                ""updated_at"": 0,
                ""updated_by"": ""string""
            }]
        }
        ";

        var expected = JsonNode.Parse(expectedResponse)?["external_locations"].Deserialize<IEnumerable<ExternalLocation>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ExternalLocationsApiClient(mockClient);
        var response = await client.List();

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var requestUri = ExternalLocationsApiUri;

        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""url"": ""string"",
            ""credential_name"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""skip_validation"": true
        }
";

        var expectedResponse = @"
        {
            ""name"": ""string"",
            ""url"": ""string"",
            ""credential_name"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""owner"": ""string"",
            ""metastore_id"": ""string"",
            ""credential_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string""
        }
";

        var externalLocationAttributes = JsonSerializer.Deserialize<ExternalLocationAttributes>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ExternalLocationsApiClient(mockClient);
        var response = await client.Create(
            externalLocationAttributes,
            true);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Post,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestGet()
    {
        var name = "location1234";
        var requestUri = $"{ExternalLocationsApiUri}/{name}";

        var expectedResponse = @"
        {
            ""name"": ""location1234"",
            ""url"": ""string"",
            ""credential_name"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""owner"": ""string"",
            ""metastore_id"": ""string"",
            ""credential_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string""
        }  
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ExternalLocationsApiClient(mockClient);
        var response = await client.Get(name);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var locationName = "location1234";
        var requestUri = $"{ExternalLocationsApiUri}/{locationName}";

        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""url"": ""string"",
            ""credential_name"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""owner"": ""string"",
            ""force"": true
        }
";

        var expectedResponse = @"
        {
            ""name"": ""string"",
            ""url"": ""string"",
            ""credential_name"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""owner"": ""string"",
            ""metastore_id"": ""string"",
            ""credential_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ExternalLocationsApiClient(mockClient);
        var response = await client.Update(
            locationName,
            "string",
            "string",
            "string",
            true,
            "string",
            "string",
            true);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestDelete()
    {

        var locationName = "location1234";
        var requestUri = $"{ExternalLocationsApiUri}/{locationName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ExternalLocationsApiClient(mockClient);
        await client.Delete(locationName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);

    }
}

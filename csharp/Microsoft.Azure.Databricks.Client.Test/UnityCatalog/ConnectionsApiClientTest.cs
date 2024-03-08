using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class ConnectionsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri ConnectionsApiUri = new(BaseApiUri, "connections");

    [TestMethod]
    public async Task TestList()
    {
        var requestUri = ConnectionsApiUri;

        var expectedResponse = @"
        {
            ""connections"": [
            {
                ""name"": ""string"",
                ""connection_type"": ""MYSQL"",
                ""owner"": ""string"",
                ""read_only"": true,
                ""comment"": ""string"",
                ""full_name"": ""string"",
                ""url"": ""string"",
                ""credential_type"": ""USERNAME_PASSWORD"",
                ""connection_id"": ""string"",
                ""metastore_id"": ""string"",
                ""created_at"": 0,
                ""created_by"": ""string"",
                ""updated_at"": 0,
                ""updated_by"": ""string"",
                ""securable_kind"": ""CONNECTION_BIGQUERY"",
                ""securable_type"": ""CONNECTION"",
                ""provisioning_info"": {
                    ""state"": ""STATE_UNSPECIFIED""
                }
            }]
        }
        ";

        var expected = JsonNode.Parse(expectedResponse)?["connections"].Deserialize<IEnumerable<Connection>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ConnectionsApiClient(mockClient);
        var response = await client.List();

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }

    [TestMethod]
    public async Task TestGet()
    {
        var connectionName = "connection1234";
        var requestUri = $"{ConnectionsApiUri}/{connectionName}";

        var expectedResponse = @"
        {
            ""name"": ""connection1234"",
            ""connection_type"": ""MYSQL"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""owner"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""full_name"": ""string"",
            ""url"": ""string"",
            ""credential_type"": ""USERNAME_PASSWORD"",
            ""connection_id"": ""string"",
            ""metastore_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string"",
            ""securable_kind"": ""CONNECTION_BIGQUERY"",
            ""securable_type"": ""CONNECTION"",
            ""provisioning_info"": {
                ""state"": ""STATE_UNSPECIFIED""
            }
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ConnectionsApiClient(mockClient);
        var response = await client.Get(connectionName);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var requestUri = ConnectionsApiUri;

        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""connection_type"": ""MYSQL"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""read_only"": true,
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            }
        }
        ";

        var expectedResponse = @"
        {
        ""name"": ""string"",
        ""connection_type"": ""MYSQL"",
        ""options"": {
            ""property1"": ""string"",
            ""property2"": ""string""
        },
        ""owner"": ""string"",
        ""read_only"": true,
        ""comment"": ""string"",
        ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
        },
        ""full_name"": ""string"",
        ""url"": ""string"",
        ""credential_type"": ""USERNAME_PASSWORD"",
        ""connection_id"": ""string"",
        ""metastore_id"": ""string"",
        ""created_at"": 0,
        ""created_by"": ""string"",
        ""updated_at"": 0,
        ""updated_by"": ""string"",
        ""securable_kind"": ""CONNECTION_BIGQUERY"",
        ""securable_type"": ""CONNECTION"",
        ""provisioning_info"": {
            ""state"": ""STATE_UNSPECIFIED""
        }}
        ";

        var connectionToCreate = JsonSerializer.Deserialize<ConnectionAttributes>(expectedRequest, Options);
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ConnectionsApiClient(mockClient);
        var response = await client.Create(
            connectionToCreate);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Post,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var connectionName = "connection1234";
        var requestUri = $"{ConnectionsApiUri}/{connectionName}";

        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""owner"": ""string""
        }
";

        var expectedResponse = @"
        {
            ""name"": ""string"",
            ""connection_type"": ""MYSQL"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""owner"": ""string"",
            ""read_only"": true,
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""full_name"": ""string"",
            ""url"": ""string"",
            ""credential_type"": ""USERNAME_PASSWORD"",
            ""connection_id"": ""string"",
            ""metastore_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string"",
            ""securable_kind"": ""CONNECTION_BIGQUERY"",
            ""securable_type"": ""CONNECTION"",
            ""provisioning_info"": {
                ""state"": ""STATE_UNSPECIFIED""
        }}
";

        var optionsDict = new Dictionary<string, string>()
            {
                { "property1", "string" },
                { "property2", "string" }
            };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ConnectionsApiClient(mockClient);
        var response = await client.Update(
            connectionName,
            "string",
            optionsDict,
            "string");

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
        var connectionName = "connection1234";
        var requestUri = $"{ConnectionsApiUri}/{connectionName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ConnectionsApiClient(mockClient);
        await client.Delete(connectionName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

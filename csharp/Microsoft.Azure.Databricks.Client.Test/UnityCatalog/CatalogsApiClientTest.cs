using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class CatalogsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri CatalogsApiUri = new(BaseApiUri, "catalogs");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
        {
          ""catalogs"": [
            {
              ""name"": ""string"",
              ""owner"": ""string"",
              ""comment"": ""string"",
              ""storage_root"": ""string"",
              ""provider_name"": ""string"",
              ""share_name"": ""string"",
              ""metastore_id"": ""string"",
              ""created_at"": 0,
              ""created_by"": ""string"",
              ""updated_at"": 0,
              ""updated_by"": ""string"",
              ""catalog_type"": ""MANAGED_CATALOG"",
              ""storage_location"": ""string"",
              ""isolation_mode"": ""OPEN"",
              ""connection_name"": ""string"",
              ""full_name"": ""string"",
              ""securable_kind"": ""CATALOG_STANDARD"",
              ""securable_type"": ""CATALOG"",
              ""provisioning_info"": {
                ""state"": ""STATE_UNSPECIFIED""
              },
              ""browse_only"": true
            }
          ]
        }
";
        var requestUri = CatalogsApiUri;
        var expected = JsonNode.Parse(expectedResponse)?["catalogs"].Deserialize<IEnumerable<Catalog>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new CatalogsApiClient(mockClient);
        var actual = await client.List();

        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""storage_root"": ""string"",
            ""provider_name"": ""string"",
            ""share_name"": ""string"",
            ""connection_name"": ""string"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            }
        }
        ";

        var expectedResponse = @"
        {
        ""name"": ""string"",
        ""owner"": ""string"",
        ""comment"": ""string"",
        ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
        },
        ""storage_root"": ""string"",
        ""provider_name"": ""string"",
        ""share_name"": ""string"",
        ""metastore_id"": ""string"",
        ""created_at"": 0,
        ""created_by"": ""string"",
        ""updated_at"": 0,
        ""updated_by"": ""string"",
        ""catalog_type"": ""MANAGED_CATALOG"",
        ""storage_location"": ""string"",
        ""isolation_mode"": ""OPEN"",
        ""connection_name"": ""string"",
        ""options"": {
            ""property1"": ""string"",
            ""property2"": ""string""
        },
        ""full_name"": ""string"",
        ""securable_kind"": ""CATALOG_STANDARD"",
        ""securable_type"": ""CATALOG"",
        ""provisioning_info"": {
            ""state"": ""STATE_UNSPECIFIED""
        },
        ""browse_only"": true
        }
";

        var requestUri = CatalogsApiUri;
        var catalogToCreate = JsonSerializer.Deserialize<CatalogAttributes>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new CatalogsApiClient(mockClient);

        var response = await client.Create(catalogToCreate);
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
        var catalogName = "catalog1234";
        var requestUri = $"{CatalogsApiUri}/{catalogName}";

        var expectedReponse = @"
        {
            ""name"": ""string"",
            ""owner"": ""string"",
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""storage_root"": ""string"",
            ""provider_name"": ""string"",
            ""share_name"": ""string"",
            ""metastore_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string"",
            ""catalog_type"": ""MANAGED_CATALOG"",
            ""storage_location"": ""string"",
            ""isolation_mode"": ""OPEN"",
            ""connection_name"": ""string"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""full_name"": ""string"",
            ""securable_kind"": ""CATALOG_STANDARD"",
            ""securable_type"": ""CATALOG"",
            ""provisioning_info"": {
                ""state"": ""STATE_UNSPECIFIED""
            },
            ""browse_only"": true
        }   
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedReponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new CatalogsApiClient(mockClient);
        var actual = await client.Get(catalogName);

        var actualJson = JsonSerializer.Serialize(actual, Options);
        AssertJsonDeepEquals(expectedReponse, actualJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var catalogName = "catalog1234";
        var requestUri = $"{CatalogsApiUri}/{catalogName}";

        // new values, same as in expected request
        var name = "string";
        var owner = "string";
        var comment = "string";
        var properties = new Dictionary<string, string>
        {
            { "property1", "string" },
            { "property2", "string" }
        };
        IsolationMode isolationMode = IsolationMode.OPEN;


        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""owner"": ""string"",
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""isolation_mode"": ""OPEN""
        }
        ";

        var expectedResponse = @"
        {
            ""name"": ""string"",
            ""owner"": ""string"",
            ""comment"": ""string"",
            ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""storage_root"": ""string"",
            ""provider_name"": ""string"",
            ""share_name"": ""string"",
            ""metastore_id"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string"",
            ""catalog_type"": ""MANAGED_CATALOG"",
            ""storage_location"": ""string"",
            ""isolation_mode"": ""OPEN"",
            ""connection_name"": ""string"",
            ""options"": {
                ""property1"": ""string"",
                ""property2"": ""string""
            },
            ""full_name"": ""string"",
            ""securable_kind"": ""CATALOG_STANDARD"",
            ""securable_type"": ""CATALOG"",
            ""provisioning_info"": {
                ""state"": ""STATE_UNSPECIFIED""
            },
            ""browse_only"": true
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new CatalogsApiClient(mockClient);
        var response = await client.Update(catalogName, name, owner, comment, properties, isolationMode);

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
        var catalogName = "catalog1234";
        var requestUri = $"{CatalogsApiUri}/{catalogName}?force=false";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new CatalogsApiClient(mockClient);
        await client.Delete(catalogName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

}

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class SchemasApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri SchemasApiUri = new(BaseApiUri, "schemas");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
        {
          ""schemas"": [
            {
              ""name"": ""string"",
              ""catalog_name"": ""string"",
              ""owner"": ""string"",
              ""comment"": ""string"",
              ""storage_root"": ""string"",
              ""metastore_id"": ""string"",
              ""full_name"": ""string"",
              ""storage_location"": ""string"",
              ""created_at"": 0,
              ""created_by"": ""string"",
              ""updated_at"": 0,
              ""updated_by"": ""string"",
              ""catalog_type"": ""string""
            }
          ]
        }
";
        var requestUri = $"{SchemasApiUri}?catalog_name=string";

        var expected = JsonNode.Parse(expectedResponse)?["schemas"].Deserialize<IEnumerable<Schema>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(System.Net.HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SchemasApiClient(mockClient);
        var actual = await client.List("string");

        CollectionAssert.AreEqual(expected?.ToList(), actual?.ToList());
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""catalog_name"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""storage_root"": ""string""
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""catalog_name"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""storage_root"": ""string"",
          ""metastore_id"": ""string"",
          ""full_name"": ""string"",
          ""storage_location"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""catalog_type"": ""string""
        }
";

        var requestUri = SchemasApiUri;

        var schemasAttributes = JsonSerializer.Deserialize<SchemaAttributes>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SchemasApiClient(mockClient);

        var response = await client.Create(schemasAttributes);

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
        var schemaName = "sample_catalog.samlpe_schema";
        var requestUri = $"{SchemasApiUri}/{schemaName}";

        var expectedReponse = @"
        {
            ""name"": ""string"",
            ""catalog_name"": ""string"",
            ""owner"": ""string"",
            ""comment"": ""string"",
            ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
            },
            ""storage_root"": ""string"",
            ""metastore_id"": ""string"",
            ""full_name"": ""string"",
            ""storage_location"": ""string"",
            ""created_at"": 0,
            ""created_by"": ""string"",
            ""updated_at"": 0,
            ""updated_by"": ""string"",
            ""catalog_type"": ""string""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedReponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SchemasApiClient(mockClient);
        var actual = await client.Get(schemaName);

        var actualJson = JsonSerializer.Serialize(actual, Options);
        AssertJsonDeepEquals(expectedReponse, actualJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var schemaFullName = "sample_catalog.samlpe_schema";
        var requestUri = $"{SchemasApiUri}/{schemaFullName}";

        // new values, same as in expected request
        var name = "string";
        var owner = "string";
        var comment = "string";
        var properties = new Dictionary<string, string>
        {
            { "property1", "string" },
            { "property2", "string" }
        };


        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""owner"": ""string"",
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
          ""catalog_name"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""storage_root"": ""string"",
          ""metastore_id"": ""string"",
          ""full_name"": ""string"",
          ""storage_location"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""catalog_type"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SchemasApiClient(mockClient);
        var response = await client.Update(schemaFullName, name, owner, comment, properties);

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
        var schemaFullName = "sample_catalog.samlpe_schema";
        var requestUri = $"{SchemasApiUri}/{schemaFullName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SchemasApiClient(mockClient);
        await client.Delete(schemaFullName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

}

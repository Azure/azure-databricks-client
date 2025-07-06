using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;

using Moq;
using Moq.Contrib.HttpClient;

using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class RegisteredModelsApiClientTests : UnityCatalogApiClientTest
{
    [TestMethod]
    public async Task ListTest()
    {
        var requestUri = $"{BaseApiUri}models?";
        var expectedResponse = @"
        {
            ""registered_models"": [
                {
                    ""name"": ""revenue_forecasting_model"",
                    ""catalog_name"": ""main"",
                    ""schema_name"": ""default"",
                    ""full_name"": ""main.default.revenue_forecasting_model"",
                    ""owner"": ""Alice@example.com"",
                    ""id"": ""01234567-89ab-cdef-0123-456789abcdef"",
                    ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
                    ""created_at"": 1666369196203,
                    ""created_by"": ""Alice@example.com"",
                    ""updated_at"": 1666369196203,
                    ""updated_by"": ""Alice@example.com"",
                    ""storage_location"": ""s3://my-bucket/hello/world/models/01234567-89ab-cdef-0123-456789abcdef"",
                    ""securable_type"": ""FUNCTION"",
                    ""securable_kind"": ""FUNCTION_REGISTERED_MODEL"",
                    ""comment"": ""This model contains model versions that forecast future revenue, given historical data""
                },
                {
                    ""name"": ""fraud_detection_model"",
                    ""catalog_name"": ""main"",
                    ""schema_name"": ""default"",
                    ""full_name"": ""main.default.fraud_detection_model"",
                    ""owner"": ""Alice@example.com"",
                    ""id"": ""9876543-21zy-abcd-3210-abcdef456789"",
                    ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
                    ""created_at"": 1666369196345,
                    ""created_by"": ""Alice@example.com"",
                    ""updated_at"": 1666369196345,
                    ""updated_by"": ""Alice@example.com"",
                    ""storage_location"": ""s3://my-bucket/hello/world/models/9876543-21zy-abcd-3210-abcdef456789"",
                    ""securable_type"": ""FUNCTION"",
                    ""securable_kind"": ""FUNCTION_REGISTERED_MODEL"",
                    ""comment"": ""This model contains model versions that identify fraudulent transactions""
                }
            ],
            ""next_page_token"": ""some-page-token""
            }";

        var expected = JsonNode.Parse(expectedResponse)?["registered_models"].Deserialize<IEnumerable<RegisteredModel>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new RegisteredModelsApiClient(mockClient);
        var (actual, token) = await client.List();
        CollectionAssert.AreEqual(expected!.ToArray(), actual.ToArray());
        Assert.AreEqual("some-page-token", token);
    }


    [TestMethod]
    public async Task GetTest()
    {
        var expectedResponse = @"
        {
          ""aliases"": [],
          ""name"": ""my_model"",
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""full_name"": ""main.default.my_model"",
          ""owner"": ""Alice@example.com"",
          ""id"": ""01234567-89ab-cdef-0123-456789abcdef"",
          ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
          ""created_at"": 1666369196203,
          ""created_by"": ""Alice@example.com"",
          ""updated_at"": 1666369196203,
          ""updated_by"": ""Alice@example.com"",
          ""storage_location"": ""s3://my-bucket/hello/world/my-model"",
          ""securable_type"": ""FUNCTION"",
          ""securable_kind"": ""FUNCTION_REGISTERED_MODEL"",
          ""comment"": ""This is my first model""
        }
        ";

        var full_name = "main.default.my_model";
        var requestUri = $"{BaseApiUri}models/{full_name}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new RegisteredModelsApiClient(mockClient);
        var response = await client.Get(full_name);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task SetTest()
    {
        var full_name = "main.default.revenue_forecasting_model";
        var alias = "champion";
        var version_num = 2;
        var requestUri = $"{BaseApiUri}models/{full_name}/aliases/{alias}";


        var expectedRequest = @"
        {
            ""version_num"": 2
        }";

        var expectedResponse = @"
        {
            ""alias_name"": ""champion"",
            ""version_num"": 2
        }";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");


        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new RegisteredModelsApiClient(mockClient);
        var response = await client.SetAlias(full_name, alias, version_num);
        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Put,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }
}

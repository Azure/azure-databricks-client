using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class ModelVersionApiClientTests : UnityCatalogApiClientTest
{
    [TestMethod]
    public async Task ListTest()
    {
        var full_name = "main.default.revenue_forecasting";
        var requestUri = $"{BaseApiUri}models/{full_name}/versions?";

        var expectedResponse = @"
        {
             ""model_versions"": [
            {
              ""model_name"": ""revenue_forecasting_model"",
              ""catalog_name"": ""main"",
              ""schema_name"": ""default"",
              ""comment"": ""This model version forecasts future revenue given historical data, using classic ML techniques"",
              ""source"": ""dbfs:/databricks/mlflow-tracking/1234567890/abcdef/artifacts/model"",
              ""run_id"": ""abcdef"",
              ""run_workspace_id"": 6051234418418567,
              ""version"": 1,
              ""status"": ""READY"",
              ""id"": ""9876543-21zy-abcd-3210-abcdef456789"",
              ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
              ""created_at"": 1666369196203,
              ""created_by"": ""Alice@example.com"",
              ""updated_at"": 1666369196203,
              ""updated_by"": ""Alice@example.com"",
              ""storage_location"": ""s3://my-bucket/hello/world/models/2222-2222/versions/9876543-21zy-abcd-3210-abcdef456789""
            },
            {
              ""model_name"": ""revenue_forecasting_model"",
              ""catalog_name"": ""main"",
              ""schema_name"": ""default"",
              ""comment"": ""This model version forecasts future revenue given historical data, using deep learning"",
              ""source"": ""dbfs:/databricks/mlflow-tracking/1234567890/abcdef/artifacts/model"",
              ""run_id"": ""abcdef"",
              ""run_workspace_id"": 6051234418418567,
              ""version"": 2,
              ""status"": ""READY"",
              ""id"": ""01234567-89ab-cdef-0123-456789abcdef"",
              ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
              ""created_at"": 1666369196907,
              ""created_by"": ""Alice@example.com"",
              ""updated_at"": 1666369196907,
              ""updated_by"": ""Alice@example.com"",
              ""storage_location"": ""s3://my-bucket/hello/world/models/2222-2222/versions/01234567-89ab-cdef-0123-456789abcdef""
            }
          ],
          ""next_page_token"": ""some-page-token""
        }";
        var expected = JsonNode.Parse(expectedResponse)?["model_versions"].Deserialize<IEnumerable<ModelVersion>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ModelVersionApiClient(mockClient);

        var (actual, token) = await client.List(full_name);

        CollectionAssert.AreEqual(expected!.ToArray(), actual.ToArray());
        Assert.AreEqual("some-page-token", token);
    }


    [TestMethod]
    public async Task GetTest()
    {
        var expectedResponse = @"
        {
          ""model_name"": ""revenue_forecasting_model"",
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""comment"": ""This model version forecasts future revenue given historical data, using classic ML techniques"",
          ""source"": ""dbfs:/databricks/mlflow-tracking/1234567890/abcdef/artifacts/model"",
          ""run_id"": ""abcdef"",
          ""run_workspace_id"": 6051234418418567,
          ""version"": 1,
          ""status"": ""READY"",
          ""id"": ""01234567-89ab-cdef-0123-456789abcdef"",
          ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
          ""created_at"": 1666369196203,
          ""created_by"": ""Alice@example.com"",
          ""updated_at"": 1666369196203,
          ""updated_by"": ""Alice@example.com"",
          ""storage_location"": ""s3://my-bucket/hello/world/models/2222-2222/versions/01234567-89ab-cdef-0123-456789abcdef""
        }
        ";

        var full_name = "main.default.revenue_forecasting_model";
        var version = 2;
        var requestUri = $"{BaseApiUri}models/{full_name}/versions/{version}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ModelVersionApiClient(mockClient);
        var response = await client.Get(full_name, version);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task GetByAliasTest()
    {
        var full_name = "main.default.revenue_forecasting_model";
        var alias = "champion";
        var requestUri = $"{BaseApiUri}models/{full_name}/aliases/{alias}";


        var expectedResponse = @"
       {
          ""model_name"": ""revenue_forecasting_model"",
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""comment"": ""This model version forecasts future revenue, given historical data"",
          ""source"": ""dbfs:/databricks/mlflow-tracking/1234567890/abcdef/artifacts/model"",
          ""run_id"": ""abcdef"",
          ""run_workspace_id"": 6051234418418567,
          ""version"": 1,
          ""status"": ""PENDING_REGISTRATION"",
          ""id"": ""01234567-89ab-cdef-0123-456789abcdef"",
          ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
          ""created_at"": 1666369196203,
          ""created_by"": ""Alice@example.com"",
          ""updated_at"": 1666369196203,
          ""updated_by"": ""Alice@example.com"",
          ""storage_location"": ""s3://my-bucket/hello/world/models/2222-2222/versions/01234567-89ab-cdef-0123-456789abcdef"",
          ""aliases"": [
            {
              ""alias_name"": ""champion"",
              ""version_num"": 1
            }
          ]
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");


        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new ModelVersionApiClient(mockClient);

        var response = await client.GetByAlias(full_name, alias);
        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }
}
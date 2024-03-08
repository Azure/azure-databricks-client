using Microsoft.Azure.Databricks.Client.Models;
using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class VolumesApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri VolumesApiUri = new(BaseApiUri, "volumes");

    [TestMethod]
    public async Task TestCreate()
    {
        var expectedRequest = @"
        {
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""name"": ""my_volume"",
          ""volume_type"": ""EXTERNAL"",
          ""storage_location"": ""s3://my-bucket/hello/world/my-volume"",
          ""comment"": ""This is my first volume""
        }
        ";

        var expectedResponse = @"
        {
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""name"": ""my_volume"",
          ""full_name"": ""main.default.my_volume"",
          ""volume_type"": ""EXTERNAL"",
          ""owner"": ""Alice@example.com"",
          ""volume_id"": ""01234567-89ab-cdef-0123-456789abcdef"",
          ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
          ""created_at"": 1666369196203,
          ""created_by"": ""Alice@example.com"",
          ""updated_at"": 1666369196203,
          ""updated_by"": ""Alice@example.com"",
          ""storage_location"": ""s3://my-bucket/hello/world/my-volume"",
          ""comment"": ""This is my first volume""
        }
";

        var requestUri = VolumesApiUri;
        var newVolumeAttributes = JsonSerializer.Deserialize<VolumeAttributes>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new VolumesApiClient(mockClient);

        var response = await client.Create(newVolumeAttributes);
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
        var volumeName = "main.default.my_volume";
        var requestUri = $"{VolumesApiUri}/{volumeName}";

        var expectedReponse = @"
        {
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""name"": ""my_volume"",
          ""full_name"": ""main.default.my_volume"",
          ""volume_type"": ""EXTERNAL"",
          ""owner"": ""Alice@example.com"",
          ""volume_id"": ""01234567-89ab-cdef-0123-456789abcdef"",
          ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
          ""created_at"": 1666369196203,
          ""created_by"": ""Alice@example.com"",
          ""updated_at"": 1666369196203,
          ""updated_by"": ""Alice@example.com"",
          ""storage_location"": ""s3://my-bucket/hello/world/my-volume"",
          ""comment"": ""This is my first volume""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedReponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new VolumesApiClient(mockClient);
        var actual = await client.Get(volumeName);

        var actualJson = JsonSerializer.Serialize(actual, Options);
        AssertJsonDeepEquals(expectedReponse, actualJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var fullVolumeName = "main.default.my_volume";
        var requestUri = $"{VolumesApiUri}/{fullVolumeName}";

        // new values, same as in expected request
        var name = "my_new_volume";
        var owner = "Bob@example.com";
        var comment = "This is my new volume";

        var expectedRequest = @"
        {
            ""name"": ""my_new_volume"",
            ""owner"": ""Bob@example.com"",
            ""comment"": ""This is my new volume""
        }
        ";

        var expectedResponse = @"
        {
          ""catalog_name"": ""main"",
          ""schema_name"": ""default"",
          ""name"": ""my_new_volume"",
          ""full_name"": ""main.default.my_new_volume"",
          ""volume_type"": ""EXTERNAL"",
          ""owner"": ""Bob@example.com"",
          ""volume_id"": ""01234567-89ab-cdef-0123-456789abcdef"",
          ""metastore_id"": ""11111111-1111-1111-1111-111111111111"",
          ""created_at"": 1666369196203,
          ""created_by"": ""Alice@example.com"",
          ""updated_at"": 1666369207415,
          ""updated_by"": ""Alice@example.com"",
          ""storage_location"": ""s3://my-bucket/hello/world/my-volume"",
          ""comment"": ""This is my new volume""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new VolumesApiClient(mockClient);
        var response = await client.Update(fullVolumeName, name, owner, comment);

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
        var volumeName = "main.default.my_volume";
        var requestUri = $"{VolumesApiUri}/{volumeName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new VolumesApiClient(mockClient);
        await client.Delete(volumeName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

}

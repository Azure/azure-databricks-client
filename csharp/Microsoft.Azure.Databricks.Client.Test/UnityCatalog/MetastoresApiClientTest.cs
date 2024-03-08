using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class MetastoresApiClientTest : UnityCatalogApiClientTest
{
    [TestMethod]
    public async Task TestList()
    {
        var requestUri = $"{BaseApiUri}metastores";

        var expectedResponse = @"
        {
            ""metastores"": [
            {
                ""name"": ""string"",
                ""storage_root"": ""string"",
                ""default_data_access_config_id"": ""string"",
                ""storage_root_credential_id"": ""string"",
                ""delta_sharing_scope"": ""INTERNAL"",
                ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
                ""delta_sharing_organization_name"": ""string"",
                ""owner"": ""string"",
                ""privilege_model_version"": ""string"",
                ""region"": ""string"",
                ""metastore_id"": ""string"",
                ""created_at"": 0,
                ""created_by"": ""string"",
                ""updated_at"": 0,
                ""updated_by"": ""string"",
                ""storage_root_credential_name"": ""string"",
                ""cloud"": ""string"",
                ""global_metastore_id"": ""string""
            }
            ]
        }
        ";

        var expected = JsonNode.Parse(expectedResponse)?["metastores"].Deserialize<IEnumerable<Metastore>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.List();

        var responseJson = JsonSerializer.Serialize(response, Options);
        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }

    [TestMethod]
    public async Task TestGetSummary()
    {
        var requestUri = $"{BaseApiUri}metastore_summary";

        var expectedResponse = @"
        {
          ""metastore_id"": ""string"",
          ""name"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""cloud"": ""string"",
          ""region"": ""string"",
          ""global_metastore_id"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""privilege_model_version"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""storage_root"": ""string"",
          ""owner"": ""string"",
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

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.GetSummary();

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestGet()
    {
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}";

        var expectedResponse = @"
        {
          ""metastore_id"": ""string"",
          ""name"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""cloud"": ""string"",
          ""region"": ""string"",
          ""global_metastore_id"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""privilege_model_version"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""storage_root"": ""string"",
          ""owner"": ""string"",
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

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.Get(metastoreId);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var requestUri = $"{BaseApiUri}metastores";

        var expectedRequest = @"
        {
            ""name"": ""string"",
            ""storage_root"": ""string"",
            ""region"": ""string""
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""storage_root"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""owner"": ""string"",
          ""privilege_model_version"": ""string"",
          ""region"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""cloud"": ""string"",
          ""global_metastore_id"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.Create(
            "string",
            "string",
            "string");

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
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}";

        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""owner"": ""string"",
          ""privilege_model_version"": ""string""
        }
";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""storage_root"": ""string"",
          ""default_data_access_config_id"": ""string"",
          ""storage_root_credential_id"": ""string"",
          ""delta_sharing_scope"": ""INTERNAL"",
          ""delta_sharing_recipient_token_lifetime_in_seconds"": 1,
          ""delta_sharing_organization_name"": ""string"",
          ""owner"": ""string"",
          ""privilege_model_version"": ""string"",
          ""region"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_root_credential_name"": ""string"",
          ""cloud"": ""string"",
          ""global_metastore_id"": ""string""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.Update(
            metastoreId,
            "string",
            "string",
            DeltaSharingScope.INTERNAL,
            1,
            "string",
            "string",
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
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}?force=false";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.Delete(metastoreId);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

    [TestMethod]
    public async Task TestGetAssignment()
    {
        var requestUri = $"{BaseApiUri}current-metastore-assignment";

        var expectedResponse = @"
        {
          ""metastore_id"": ""string"",
          ""workspace_id"": 1,
          ""default_catalog_name"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        var response = await client.GetAssignment();

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreateAssignment()
    {
        var workspaceId = 1232412;
        var requestUri = $"{BaseApiUri}workspaces/{workspaceId}/metastore";

        var expectedRequest = @"
        {
          ""metastore_id"": ""string"",
          ""default_catalog_name"": ""string""
        }
        ";


        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.CreateAssignment(
            workspaceId,
            "string",
            "string");

        handler.VerifyRequest(
            HttpMethod.Put,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestUpdateAssignment()
    {
        var workspaceId = 1232412;
        var requestUri = $"{BaseApiUri}workspaces/{workspaceId}/metastore";

        var expectedRequest = @"
        {
          ""metastore_id"": ""string"",
          ""default_catalog_name"": ""string""
        }
        ";


        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.UpdateAssignment(
            workspaceId,
            "string",
            "string");

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestDeleteAssignment()
    {
        var metastoreId = "2422B3C8-664B-4EDC-9E65-F164F2B2F2BA";
        var workspaceId = 1232412;
        var requestUri = $"{BaseApiUri}workspaces/{workspaceId}/metastore?metastore_id={metastoreId}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new MetastoresApiClient(mockClient);
        await client.DeleteAssignment(
            workspaceId,
            metastoreId);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

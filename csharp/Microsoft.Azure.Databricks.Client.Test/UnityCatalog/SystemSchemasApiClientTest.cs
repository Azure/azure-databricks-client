using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class SystemSchemasApiClientTest : UnityCatalogApiClientTest
{
    [TestMethod]
    public async Task TestList()
    {
        var metastoreId = "metastore1234";

        var requestUri = $"{BaseApiUri}metastores/{metastoreId}/systemschemas";

        var expectedResponse = @"
        {
          ""schemas"": [
            {
              ""schema"": ""string"",
              ""state"": ""AVAILABLE""
            }
          ]
        }
";

        var expected = JsonNode.Parse(expectedResponse)?["schemas"].Deserialize<IEnumerable<SystemSchema>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SystemSchemasApiClient(mockClient);
        var response = await client.List(metastoreId);

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }

    [TestMethod]
    public async Task TestEnable()
    {
        var metastoreId = "metastore1234";
        var schemaName = SystemSchemaName.lineage;
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}/systemschemas/{schemaName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SystemSchemasApiClient(mockClient);
        await client.Enable(metastoreId, schemaName);

        handler.VerifyRequest(
            HttpMethod.Put,
            requestUri);
    }

    [TestMethod]
    public async Task TestDisable()
    {
        var metastoreId = "metastore1234";
        var schemaName = SystemSchemaName.lineage;
        var requestUri = $"{BaseApiUri}metastores/{metastoreId}/systemschemas/{schemaName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SystemSchemasApiClient(mockClient);
        await client.Disable(metastoreId, schemaName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

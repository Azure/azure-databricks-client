using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class UnityCatalogPermissionsApiClientTest : UnityCatalogApiClientTest
{
    [TestMethod]
    public async Task TestGet()
    {
        var securableType = SecurableType.catalog;
        var securableName = "catalog123";
        var requestUri = $"{BaseApiUri}permissions/{securableType}/{securableName}";

        var expectedResponse = @"
        {
          ""privilege_assignments"": [
            {
              ""principal"": ""string"",
              ""privileges"": [
                ""READ_PRIVATE_FILES""
              ]
            }
          ]
        }
";
        var expected = JsonNode.Parse(expectedResponse)?["privilege_assignments"].Deserialize<IEnumerable<Permission>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new UnityCatalogPermissionsApiClient(mockClient);
        var response = await client.Get(securableType, securableName);

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var securableType = SecurableType.catalog;
        var securableName = "catalog123";
        var requestUri = $"{BaseApiUri}permissions/{securableType}/{securableName}";

        var expectedRequest = @"
        {
          ""changes"": [
            {
              ""principal"": ""string"",
              ""add"": [
                ""READ_PRIVATE_FILES""
              ],
              ""remove"": [
                ""READ_PRIVATE_FILES""
              ]
            }
          ]
        }
";

        var expectedResponse = @"
        {
          ""privilege_assignments"": [
            {
              ""principal"": ""string"",
              ""privileges"": [
                ""READ_PRIVATE_FILES""
              ]
            }
          ]
        }
";

        var principal = "string";
        IEnumerable<Privilege> addPrivileges = new List<Privilege>()
        {
            Privilege.READ_PRIVATE_FILES
        };

        IEnumerable<Privilege> removePrivileges = new List<Privilege>()
        {
            Privilege.READ_PRIVATE_FILES
        };

        var permissionUpdate = new PermissionsUpdate()
        {
            Principal = principal,
            Add = addPrivileges,
            Remove = removePrivileges,
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new UnityCatalogPermissionsApiClient(mockClient);
        var response = await client.Update(
            securableType,
            securableName,
            new PermissionsUpdate[] { permissionUpdate });

        var expected = JsonNode.Parse(expectedResponse)?["privilege_assignments"].Deserialize<IEnumerable<Permission>>(Options);

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestGetEffective()
    {
        var securableType = SecurableType.catalog;
        var securableName = "catalog123";
        var requestUri = $"{BaseApiUri}effective-permissions/{securableType}/{securableName}";

        var expectedResponse = @"
        {
            ""privilege_assignments"": [
            {
                ""principal"": ""string"",
                ""privileges"": [
                {
                    ""privilege"": ""READ_PRIVATE_FILES"",
                    ""inherited_from_type"": ""catalog"",
                    ""inherited_from_name"": ""string""
                }]
            }]
        }
        ";

        var expected = JsonNode.Parse(expectedResponse)?["privilege_assignments"].Deserialize<IEnumerable<EffectivePermission>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new UnityCatalogPermissionsApiClient(mockClient);
        var response = await client.GetEffective(securableType, securableName);

        CollectionAssert.AreEqual(expected?.ToList(), response?.ToList());
    }
}

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class StorageCredentialsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri StorageCredentialsApiUri = new(BaseApiUri, "storage-credentials\r\n");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
        {
          ""storage_credentials"": [
            {
              ""name"": ""string"",
              ""azure_service_principal"": {
                ""directory_id"": ""string"",
                ""application_id"": ""string"",
                ""client_secret"": ""string""
              },
              ""azure_managed_identity"": {
                ""access_connector_id"": ""string"",
                ""managed_identity_id"": ""string"",
                ""credential_id"": ""string""
              },
              ""comment"": ""string"",
              ""read_only"": true,
              ""owner"": ""string"",
              ""id"": ""string"",
              ""metastore_id"": ""string"",
              ""created_at"": 0,
              ""created_by"": ""string"",
              ""updated_at"": 0,
              ""updated_by"": ""string"",
              ""used_for_managed_storage"": true
            }
          ]
        }
";
        var requestUri = StorageCredentialsApiUri;

        var expected = JsonNode.Parse(expectedResponse)?["storage_credentials"].Deserialize<IEnumerable<StorageCredential>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new StorageCredentialsApiClient(mockClient);
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
          ""read_only"": true,
          ""azure_service_principal"": {
            ""directory_id"": ""string"",
            ""application_id"": ""string"",
            ""client_secret"": ""string""
          },
          ""azure_managed_identity"": {
            ""access_connector_id"": ""string"",
            ""managed_identity_id"": ""string"",
            ""credential_id"": ""string""
          },
          ""skip_validation"": false
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""azure_service_principal"": {
            ""directory_id"": ""string"",
            ""application_id"": ""string"",
            ""client_secret"": ""string""
          },
          ""azure_managed_identity"": {
            ""access_connector_id"": ""string"",
            ""managed_identity_id"": ""string"",
            ""credential_id"": ""string""
          },
          ""comment"": ""string"",
          ""read_only"": true,
          ""owner"": ""string"",
          ""id"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""used_for_managed_storage"": true
        }
";
        // new values, same as in expected request
        var name = "string";
        var comment = "string";
        var readOnly = true;
        var skipValidation = false;

        var azureServicePrincipal = new AzureServicePrincipal()
        {
            DirectoryId = "string",
            ApplicationId = "string",
            ClientSecret = "string"
        };

        var azureManagedIdentity = new AzureManagedIdentity()
        {
            AccessConnectorId = "string",
            ManagedIdentityId = "string",
            CredentialId = "string"
        };

        var credentialsAttributes = new StorageCredentialAttributes()
        {
            Name = name,
            Comment = comment,
            ReadOnly = readOnly,
            AzureServicePrincipal = azureServicePrincipal,
            AzureManagedIdentity = azureManagedIdentity
        };

        var requestUri = StorageCredentialsApiUri;

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new StorageCredentialsApiClient(mockClient);

        var response = await client.Create(
            credentialsAttributes,
            skipValidation);

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
        var credentialName = "sample_credential";
        var requestUri = $"{StorageCredentialsApiUri}/{credentialName}";

        var expectedReponse = @"
        {
          ""name"": ""string"",
          ""azure_service_principal"": {
            ""directory_id"": ""string"",
            ""application_id"": ""string"",
            ""client_secret"": ""string""
          },
          ""azure_managed_identity"": {
            ""access_connector_id"": ""string"",
            ""managed_identity_id"": ""string"",
            ""credential_id"": ""string""
          },
          ""comment"": ""string"",
          ""read_only"": true,
          ""owner"": ""string"",
          ""id"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""used_for_managed_storage"": true
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedReponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new StorageCredentialsApiClient(mockClient);
        var actual = await client.Get(credentialName);

        var actualJson = JsonSerializer.Serialize(actual, Options);
        AssertJsonDeepEquals(expectedReponse, actualJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var credentialName = "sample_credential";
        var requestUri = $"{StorageCredentialsApiUri}/{credentialName}";

        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""comment"": ""string"",
          ""read_only"": true,
          ""owner"": ""string"",
          ""azure_service_principal"": {
            ""directory_id"": ""string"",
            ""application_id"": ""string"",
            ""client_secret"": ""string""
          },
          ""azure_managed_identity"": {
            ""access_connector_id"": ""string"",
            ""managed_identity_id"": ""string"",
            ""credential_id"": ""string""
          },
          ""skip_validation"": false,
          ""force"": true
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""azure_service_principal"": {
            ""directory_id"": ""string"",
            ""application_id"": ""string"",
            ""client_secret"": ""string""
          },
          ""azure_managed_identity"": {
            ""access_connector_id"": ""string"",
            ""managed_identity_id"": ""string"",
            ""credential_id"": ""string""
          },
          ""comment"": ""string"",
          ""read_only"": true,
          ""owner"": ""string"",
          ""id"": ""string"",
          ""metastore_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""used_for_managed_storage"": true
        }
        ";

        // new values, same as in expected request
        var name = "string";
        var comment = "string";
        var readOnly = true;
        var owner = "string";
        var skipValidation = false;
        var force = true;

        var azureServicePrincipal = new AzureServicePrincipal()
        {
            DirectoryId = "string",
            ApplicationId = "string",
            ClientSecret = "string"
        };

        var azureManagedIdentity = new AzureManagedIdentity()
        {
            AccessConnectorId = "string",
            ManagedIdentityId = "string",
            CredentialId = "string"
        };

        var credentialsAttributes = new StorageCredentialAttributes()
        {
            Name = name,
            Comment = comment,
            ReadOnly = readOnly,
            Owner = owner,
            AzureServicePrincipal = azureServicePrincipal,
            AzureManagedIdentity = azureManagedIdentity
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new StorageCredentialsApiClient(mockClient);
        var response = await client.Update(
            credentialName,
            credentialsAttributes,
            skipValidation,
            force);

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
        var credentialName = "sample_credential";
        var requestUri = $"{StorageCredentialsApiUri}/{credentialName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new StorageCredentialsApiClient(mockClient);
        await client.Delete(credentialName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

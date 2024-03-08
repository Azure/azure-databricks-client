using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq.Contrib.HttpClient;
using Moq;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class TableConstraintsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri TableConstraintsApiUri = new(BaseApiUri, "constraints");

    [TestMethod]
    public async Task TestCreatePrimaryKeyConstraint()
    {
        var expectedRequest = @"
        {
          ""full_name_arg"": ""string"",
          ""constraint"": {
            ""primary_key_constraint"": {
              ""name"": ""string"",
              ""child_columns"": [
                ""string""
              ]
            },
            ""foreign_key_constraint"": {
              ""name"": ""string"",
              ""child_columns"": [
                ""string""
              ],
              ""parent_table"": ""string"",
              ""parent_columns"": [
                ""string""
              ]
            },
            ""named_table_constraint"": {
              ""name"": ""string""
            }
          }
        }
";

        var expectedResponse = @"
        {
          ""primary_key_constraint"": {
            ""name"": ""string"",
            ""child_columns"": [
              ""string""
            ]
          },
          ""foreign_key_constraint"": {
            ""name"": ""string"",
            ""child_columns"": [
              ""string""
            ],
            ""parent_table"": ""string"",
            ""parent_columns"": [
              ""string""
            ]
          },
          ""named_table_constraint"": {
            ""name"": ""string""
          }
        }
";

        var tableConstraint = JsonSerializer.Deserialize<TableConstraintAttributes>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, TableConstraintsApiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new TableConstraintsApiClient(mockClient);
        var response = await client.Create(tableConstraint);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);

        handler.VerifyRequest(
            HttpMethod.Post,
            TableConstraintsApiUri,
            GetMatcher(expectedRequest),
            Times.Once());

    }

    [TestMethod]
    public async Task TestDelete()
    {
        var fullTableName = "catalog.schema.table";
        var constraintName = "PK_table";
        var requestUri = $"{TableConstraintsApiUri}/{fullTableName}?constraint_name={constraintName}&cascade=false";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new TableConstraintsApiClient(mockClient);
        await client.Delete(fullTableName, constraintName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

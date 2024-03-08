using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class FunctionsApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri FunctionsApiUri = new(BaseApiUri, "functions");

    [TestMethod]
    public async Task TestList()
    {
        var requestUri = FunctionsApiUri;

        var expectedResponse = @"
        {
            ""functions"": [
            {
                ""name"": ""string"",
                ""catalog_name"": ""string"",
                ""schema_name"": ""string"",
                ""input_params"": [
                {
                    ""name"": ""string"",
                    ""type_text"": ""string"",
                    ""type_json"": ""string"",
                    ""type_name"": ""BOOLEAN"",
                    ""type_precision"": 1,
                    ""type_scale"": 1,
                    ""type_interval_type"": ""string"",
                    ""position"": 1,
                    ""parameter_mode"": ""IN"",
                    ""parameter_type"": ""PARAM"",
                    ""parameter_default"": ""string"",
                    ""comment"": ""string""
                }],
                ""data_type"": ""BOOLEAN"",
                ""full_data_type"": ""string"",
                ""return_params"": [
                {
                    ""name"": ""string"",
                    ""type_text"": ""string"",
                    ""type_json"": ""string"",
                    ""type_name"": ""BOOLEAN"",
                    ""type_precision"": 1,
                    ""type_scale"": 1,
                    ""type_interval_type"": ""string"",
                    ""position"": 1,
                    ""parameter_mode"": ""IN"",
                    ""parameter_type"": ""PARAM"",
                    ""parameter_default"": ""string"",
                    ""comment"": ""string""
                }],
                ""routine_body"": ""SQL"",
                ""routine_definition"": ""string"",
                ""routine_dependencies"": [
                {
                  ""table"": {
                    ""table_full_name"": ""string""
                    }
                },
                {
                  ""function"": {
                    ""function_full_name"": ""string""
                    }   
                }],
                ""parameter_style"": ""S"",
                ""is_deterministic"": true,
                ""sql_data_access"": ""CONTAINS_SQL"",
                ""is_null_call"": true,
                ""security_type"": ""DEFINER"",
                ""specific_name"": ""string"",
                ""external_name"": ""string"",
                ""external_language"": ""string"",
                ""sql_path"": ""string"",
                ""owner"": ""string"",
                ""comment"": ""string"",
                ""metastore_id"": ""string"",
                ""full_name"": ""string"",
                ""created_at"": 0,
                ""created_by"": ""string"",
                ""updated_at"": 0,
                ""updated_by"": ""string"",
                ""function_id"": ""string""
        }]}
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new FunctionsApiClient(mockClient);
        var response = await client.List();

        // adding layer of serialization as simple Assert will fail because of arrays in Json response
        var responseDict = new Dictionary<string, IEnumerable<Function>>()
        {
            { "functions", response }
        };
        var responseJson = JsonSerializer.Serialize(responseDict, Options);

        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var requestUri = FunctionsApiUri;

        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""catalog_name"": ""string"",
          ""schema_name"": ""string"",
          ""input_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""data_type"": ""BOOLEAN"",
          ""full_data_type"": ""string"",
          ""return_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""routine_body"": ""SQL"",
          ""routine_definition"": ""string"",
          ""routine_dependencies"": [
            {
                ""table"": {
                ""table_full_name"": ""string""
                }
            },
            {
            ""function"": {
                ""function_full_name"": ""string""
            }
            }
            ],
          ""parameter_style"": ""S"",
          ""is_deterministic"": true,
          ""sql_data_access"": ""CONTAINS_SQL"",
          ""is_null_call"": true,
          ""security_type"": ""DEFINER"",
          ""specific_name"": ""string"",
          ""external_name"": ""string"",
          ""external_language"": ""string"",
          ""sql_path"": ""string"",
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
          ""schema_name"": ""string"",
          ""input_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""data_type"": ""BOOLEAN"",
          ""full_data_type"": ""string"",
          ""return_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""routine_body"": ""SQL"",
          ""routine_definition"": ""string"",
          ""routine_dependencies"": [
            {
              ""table"": {
                ""table_full_name"": ""string""
              }
            },
            {
            ""function"": {
                ""function_full_name"": ""string""
            }
            }
          ],
          ""parameter_style"": ""S"",
          ""is_deterministic"": true,
          ""sql_data_access"": ""CONTAINS_SQL"",
          ""is_null_call"": true,
          ""security_type"": ""DEFINER"",
          ""specific_name"": ""string"",
          ""external_name"": ""string"",
          ""external_language"": ""string"",
          ""sql_path"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""metastore_id"": ""string"",
          ""full_name"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""function_id"": ""string""
        }
";

        var createFunction = JsonSerializer.Deserialize<Function>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new FunctionsApiClient(mockClient);
        var response = await client.Create(createFunction);

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
        var name = "function1235";
        var requestUri = $"{FunctionsApiUri}/{name}";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""catalog_name"": ""string"",
          ""schema_name"": ""string"",
          ""input_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""data_type"": ""BOOLEAN"",
          ""full_data_type"": ""string"",
          ""return_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""routine_body"": ""SQL"",
          ""routine_definition"": ""string"",
          ""routine_dependencies"": [
            {
              ""table"": {
                ""table_full_name"": ""string""
              }
            },
            {
            ""function"": {
                ""function_full_name"": ""string""
            }
            }
          ],
          ""parameter_style"": ""S"",
          ""is_deterministic"": true,
          ""sql_data_access"": ""CONTAINS_SQL"",
          ""is_null_call"": true,
          ""security_type"": ""DEFINER"",
          ""specific_name"": ""string"",
          ""external_name"": ""string"",
          ""external_language"": ""string"",
          ""sql_path"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""metastore_id"": ""string"",
          ""full_name"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""function_id"": ""string""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new FunctionsApiClient(mockClient);
        var response = await client.Get(name);

        var responseJson = JsonSerializer.Serialize(response, Options);
        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var functionName = "function12345";
        var requestUri = $"{FunctionsApiUri}/{functionName}";

        var expectedRequest = @"
        {
          ""owner"": ""string""
        }
";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""catalog_name"": ""string"",
          ""schema_name"": ""string"",
          ""input_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""data_type"": ""BOOLEAN"",
          ""full_data_type"": ""string"",
          ""return_params"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 1,
              ""type_scale"": 1,
              ""type_interval_type"": ""string"",
              ""position"": 1,
              ""parameter_mode"": ""IN"",
              ""parameter_type"": ""PARAM"",
              ""parameter_default"": ""string"",
              ""comment"": ""string""
            }
          ],
          ""routine_body"": ""SQL"",
          ""routine_definition"": ""string"",
          ""routine_dependencies"": [
            {
              ""table"": {
                ""table_full_name"": ""string""
              }
            },
            {
            ""function"": {
                ""function_full_name"": ""string""
            }
            }
          ],
          ""parameter_style"": ""S"",
          ""is_deterministic"": true,
          ""sql_data_access"": ""CONTAINS_SQL"",
          ""is_null_call"": true,
          ""security_type"": ""DEFINER"",
          ""specific_name"": ""string"",
          ""external_name"": ""string"",
          ""external_language"": ""string"",
          ""sql_path"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""metastore_id"": ""string"",
          ""full_name"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""function_id"": ""string""
        }
";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new FunctionsApiClient(mockClient);
        var response = await client.Update(functionName, "string");

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

        var functionName = "function12345";
        var requestUri = $"{FunctionsApiUri}/{functionName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new FunctionsApiClient(mockClient);
        await client.Delete(functionName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);

    }
}

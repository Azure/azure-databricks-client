using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class TablesApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri TablesApiUri = new(BaseApiUri, "tables");

    [TestMethod]
    public async Task TestListSummaries()
    {
        const string expectedResponse = @"
        {
          ""tables"": [
            {
              ""full_name"": ""string"",
              ""table_type"": ""MANAGED""
            }
          ],
          ""next_page_token"": ""string""
        }
";

        var testOptions = Options;
        testOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        var catalogName = "catalog";

        var requestUri = $"{BaseApiUri}table-summaries?catalog_name={catalogName}&max_results=10000";
        var expectedTablesSummaries = JsonNode.Parse(expectedResponse)?["tables"].Deserialize<IEnumerable<Table>>(testOptions);
        var expectedNextPageToken = JsonNode.Parse(expectedResponse)?["next_page_token"].Deserialize<string>(testOptions);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new TablesApiClient(mockClient);
        var response = await client.ListSummaries(
            catalogName);

        // adding layer of serialization as simple Assert will fail because of arrays in Json response
        var responseDict = new Dictionary<string, IEnumerable<TableSummary>>()
        {
            { "tables", response.Item1 }
        };
        var responseListObject = JsonSerializer.SerializeToNode(responseDict, testOptions)!.AsObject();
        responseListObject.Add("next_page_token", response.Item2);

        var responseListJson = JsonSerializer.Serialize(responseListObject, testOptions);

        AssertJsonDeepEquals(expectedResponse, responseListJson);

    }

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
        {
          ""tables"": [
            {
              ""name"": ""string"",
              ""catalog_name"": ""string"",
              ""schema_name"": ""string"",
              ""table_type"": ""MANAGED"",
              ""data_source_format"": ""DELTA"",
              ""columns"": [
                {
                  ""name"": ""string"",
                  ""type_text"": ""string"",
                  ""type_json"": ""string"",
                  ""type_name"": ""BOOLEAN"",
                  ""type_precision"": 0,
                  ""type_scale"": 0,
                  ""type_interval_type"": ""string"",
                  ""position"": 0,
                  ""comment"": ""string"",
                  ""nullable"": true,
                  ""partition_index"": 0,
                  ""mask"": {
                    ""function_name"": ""string"",
                    ""using_column_names"": [
                      ""string""
                    ]
                  }
                }
              ],
              ""storage_location"": ""string"",
              ""view_definition"": ""string"",
              ""view_dependencies"": [
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
              ""sql_path"": ""string"",
              ""owner"": ""string"",
              ""comment"": ""string"",
              ""properties"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""storage_credential_name"": ""string"",
              ""table_constraints"": {
                ""table_constraints"": [
                  {
                    ""primary_key_constraint"": {
                      ""name"": ""string"",
                      ""child_columns"": [
                        ""string""
                      ]
                    }
                  },
                  {
                    ""foreign_key_constraint"": {
                      ""name"": ""string"",
                      ""child_columns"": [
                        ""string""
                      ],
                      ""parent_table"": ""string"",
                      ""parent_columns"": [
                        ""string""
                      ]
                    }
                   },
                   {
                    ""named_table_constraint"": {
                      ""name"": ""string""
                    }
                  }
                ]
              },
              ""row_filter"": {
                ""name"": ""string"",
                ""input_column_names"": [
                  ""string""
                ]
              },
              ""metastore_id"": ""string"",
              ""full_name"": ""string"",
              ""data_access_configuration_id"": ""string"",
              ""created_at"": 0,
              ""created_by"": ""string"",
              ""updated_at"": 0,
              ""updated_by"": ""string"",
              ""deleted_at"": 0,
              ""table_id"": ""string"",
              ""delta_runtime_properties_kvpairs"": {
                ""delta_runtime_properties"": {
                  ""property1"": ""string"",
                  ""property2"": ""string""
                }
              }
            }
          ],
          ""next_page_token"": ""string""
        }
";
        var testOptions = Options;
        testOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        var catalogName = "catalog";
        var schemaName = "schema";

        var requestUri = $"{TablesApiUri}?catalog_name={catalogName}&schema_name={schemaName}";
        var expectedTables = JsonNode.Parse(expectedResponse)?["tables"].Deserialize<IEnumerable<Table>>(testOptions);
        var expectedNextPageToken = JsonNode.Parse(expectedResponse)?["next_page_token"].Deserialize<string>(testOptions);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new TablesApiClient(mockClient);
        var response = await client.List(
            catalogName,
            schemaName);

        // adding layer of serialization as simple Assert will fail because of arrays in Json response
        var responseDict = new Dictionary<string, IEnumerable<Table>>()
        {
            { "tables", response.Item1 }
        };
        var responseListObject = JsonSerializer.SerializeToNode(responseDict, testOptions)!.AsObject();
        responseListObject.Add("next_page_token", response.Item2);

        var responseListJson = JsonSerializer.Serialize(responseListObject, testOptions);

        AssertJsonDeepEquals(expectedResponse, responseListJson);
    }

    [TestMethod]
    public async Task TestGet()
    {
        const string expectedResponse = @"
        {
          ""name"": ""string"",
          ""catalog_name"": ""string"",
          ""schema_name"": ""string"",
          ""table_type"": ""MANAGED"",
          ""data_source_format"": ""DELTA"",
          ""columns"": [
            {
              ""name"": ""string"",
              ""type_text"": ""string"",
              ""type_json"": ""string"",
              ""type_name"": ""BOOLEAN"",
              ""type_precision"": 0,
              ""type_scale"": 0,
              ""type_interval_type"": ""string"",
              ""position"": 0,
              ""comment"": ""string"",
              ""nullable"": true,
              ""partition_index"": 0,
              ""mask"": {
                ""function_name"": ""string"",
                ""using_column_names"": [
                  ""string""
                ]
              }
            }
          ],
          ""storage_location"": ""string"",
          ""view_definition"": ""string"",
          ""view_dependencies"": [
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
          ""sql_path"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""properties"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""storage_credential_name"": ""string"",
          ""table_constraints"": {
            ""table_constraints"": [
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
            ]
          },
          ""row_filter"": {
            ""name"": ""string"",
            ""input_column_names"": [
              ""string""
            ]
          },
          ""metastore_id"": ""string"",
          ""full_name"": ""string"",
          ""data_access_configuration_id"": ""string"",
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""deleted_at"": 0,
          ""table_id"": ""string"",
          ""delta_runtime_properties_kvpairs"": {
            ""delta_runtime_properties"": {
              ""property1"": ""string"",
              ""property2"": ""string""
            }
          }
}
";

        var testOptions = Options;
        testOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        var fullTableName = $"catalog.schema.table";

        var requestUri = $"{TablesApiUri}/{fullTableName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new TablesApiClient(mockClient);
        var response = await client.Get(fullTableName);

        var responseJson = JsonSerializer.Serialize(response, testOptions);

        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var fullTableName = $"catalog.schema.table";
        var requestUri = $"{TablesApiUri}/{fullTableName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new TablesApiClient(mockClient);
        await client.Delete(fullTableName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }
}

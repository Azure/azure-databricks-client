using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog;

using Moq;
using Moq.Contrib.HttpClient;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class SharesApiClientTest : UnityCatalogApiClientTest
{
    private static readonly Uri SharesApiUri = new(BaseApiUri, "shares");

    [TestMethod]
    public async Task TestList()
    {
        const string expectedResponse = @"
        {
          ""shares"": [
            {
              ""name"": ""string"",
              ""owner"": ""string"",
              ""comment"": ""string"",
              ""storage_root"": ""string"",
              ""objects"": [
                {
                  ""name"": ""string"",
                  ""data_object_type"": ""TABLE"",
                  ""added_at"": 0,
                  ""added_by"": ""string"",
                  ""comment"": ""string"",
                  ""shared_as"": ""string"",
                  ""partitions"": [
                    {
                      ""values"": [
                        {
                          ""name"": ""string"",
                          ""value"": ""string"",
                          ""recipient_property_key"": ""string"",
                          ""op"": ""EQUAL""
                        }
                      ]
                    }
                  ],
                  ""cdf_enabled"": true,
                  ""history_data_sharing_status"": ""DISABLED"",
                  ""start_version"": 0,
                  ""status"": ""ACTIVE"",
                  ""content"": ""string"",
                  ""string_shared_as"": ""string""
                }
              ],
              ""created_at"": 0,
              ""created_by"": ""string"",
              ""updated_at"": 0,
              ""updated_by"": ""string"",
              ""storage_location"": ""string""
            }
          ]
        }
";
        var requestUri = SharesApiUri;

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);
        var actual = await client.List();
        var responseObj = new { shares = actual };
        var responseJson = JsonSerializer.Serialize(responseObj, Options);

        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        var expectedRequest = @"
        {
          ""name"": ""string"",
          ""comment"": ""string"",
          ""storage_root"": ""string""
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""storage_root"": ""string"",
          ""objects"": [
            {
              ""name"": ""string"",
              ""data_object_type"": ""TABLE"",
              ""added_at"": 0,
              ""added_by"": ""string"",
              ""comment"": ""string"",
              ""shared_as"": ""string"",
              ""partitions"": [
                {
                  ""values"": [
                    {
                      ""name"": ""string"",
                      ""value"": ""string"",
                      ""recipient_property_key"": ""string"",
                      ""op"": ""EQUAL""
                    }
                  ]
                }
              ],
              ""cdf_enabled"": true,
              ""history_data_sharing_status"": ""DISABLED"",
              ""start_version"": 0,
              ""status"": ""ACTIVE"",
              ""content"": ""string"",
              ""string_shared_as"": ""string""
            }
          ],
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_location"": ""string""
        }
        ";

        var requestUri = SharesApiUri;
        var shareToCreate = JsonSerializer.Deserialize<ShareAttributes>(expectedRequest, Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);

        var response = await client.Create(shareToCreate);
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
        var shareName = "share123";
        var includeSharedData = true;
        var requestUri = $"{SharesApiUri}/{shareName}?include_shared_data={includeSharedData.ToString().ToLower()}";

        var expectedReponse = @"
        {
          ""name"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""storage_root"": ""string"",
          ""objects"": [
            {
              ""name"": ""string"",
              ""data_object_type"": ""TABLE"",
              ""added_at"": 0,
              ""added_by"": ""string"",
              ""comment"": ""string"",
              ""shared_as"": ""string"",
              ""partitions"": [
                {
                  ""values"": [
                    {
                      ""name"": ""string"",
                      ""value"": ""string"",
                      ""recipient_property_key"": ""string"",
                      ""op"": ""EQUAL""
                    }
                  ]
                }
              ],
              ""cdf_enabled"": true,
              ""history_data_sharing_status"": ""DISABLED"",
              ""start_version"": 0,
              ""status"": ""ACTIVE"",
              ""content"": ""string"",
              ""string_shared_as"": ""string""
            }
          ],
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_location"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedReponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);

        var actual = await client.Get(shareName, includeSharedData: includeSharedData);
        var actualJson = JsonSerializer.Serialize(actual, Options);
        AssertJsonDeepEquals(expectedReponse, actualJson);
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var shareName = "share123";
        var requestUri = $"{SharesApiUri}/{shareName}";

        // new values, same as in expected request
        var newName = "string";
        var owner = "string";
        var comment = "string";
        var storage_root = "string";
        var updates = new []
        {
            new ShareObjectUpdate
            {
                Action = ShareObjectUpdateAction.ADD,
                Object = new ShareObject
                {
                    Name = "string",
                    DataObjectType = DataObjectType.TABLE,
                    AddedAt = 0,
                    AddedBy = "string",
                    Comment = "string",
                    SharedAs = "string",
                    Partitions =
                    [
                        new SharePartition
                        {
                            Values =
                            [
                                new SharePartitionValue
                                {
                                    Name = "string",
                                    Value = "string",
                                    RecipientPropertyKey = "string",
                                    Op = "EQUAL"
                                }
                            ]
                        }
                    ],
                    CdfEnabled = true,
                    HistoryDataSharingStatus = HistoryDataSharingStatus.DISABLED,
                    StartVersion = 0,
                    Status = Status.ACTIVE,
                    Content = "string",
                    StringSharedAs = "string"
                }
            }
        };

        var expectedRequest = @"
        {
          ""new_name"": ""string"",
          ""updates"": [
            {
              ""action"": ""ADD"",
              ""data_object"": {
                ""name"": ""string"",
                ""data_object_type"": ""TABLE"",
                ""added_at"": 0,
                ""added_by"": ""string"",
                ""comment"": ""string"",
                ""shared_as"": ""string"",
                ""partitions"": [
                  {
                    ""values"": [
                      {
                        ""name"": ""string"",
                        ""value"": ""string"",
                        ""recipient_property_key"": ""string"",
                        ""op"": ""EQUAL""
                      }
                    ]
                  }
                ],
                ""cdf_enabled"": true,
                ""history_data_sharing_status"": ""DISABLED"",
                ""start_version"": 0,
                ""status"": ""ACTIVE"",
                ""content"": ""string"",
                ""string_shared_as"": ""string""
              }
            }
          ],
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""storage_root"": ""string""
        }
        ";

        var expectedResponse = @"
        {
          ""name"": ""string"",
          ""owner"": ""string"",
          ""comment"": ""string"",
          ""storage_root"": ""string"",
          ""objects"": [
            {
              ""name"": ""string"",
              ""data_object_type"": ""TABLE"",
              ""added_at"": 0,
              ""added_by"": ""string"",
              ""comment"": ""string"",
              ""shared_as"": ""string"",
              ""partitions"": [
                {
                  ""values"": [
                    {
                      ""name"": ""string"",
                      ""value"": ""string"",
                      ""recipient_property_key"": ""string"",
                      ""op"": ""EQUAL""
                    }
                  ]
                }
              ],
              ""cdf_enabled"": true,
              ""history_data_sharing_status"": ""DISABLED"",
              ""start_version"": 0,
              ""status"": ""ACTIVE"",
              ""content"": ""string"",
              ""string_shared_as"": ""string""
            }
          ],
          ""created_at"": 0,
          ""created_by"": ""string"",
          ""updated_at"": 0,
          ""updated_by"": ""string"",
          ""storage_location"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);
        var response = await client.Update(shareName, newName, owner, comment, storage_root, updates);

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
        var shareName = "share123";
        var requestUri = $"{SharesApiUri}/{shareName}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, requestUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);
        await client.Delete(shareName);

        handler.VerifyRequest(
            HttpMethod.Delete,
            requestUri);
    }

    [TestMethod]
    public async Task TestGetPermissions()
    {
        var shareName = "share123";
        var requestUri = $"{SharesApiUri}/{shareName}/permissions";

        var expectedReponse = @"
        {
          ""privilege_assignments"": [
            {
              ""principal"": ""string"",
              ""privileges"": [
                ""SELECT""
              ]
            }
          ]
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedReponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);

        var actual = await client.GetPermissions(shareName);
        var responseObj = new { privilege_assignments = actual };
        var actualJson = JsonSerializer.Serialize(responseObj, Options);

        AssertJsonDeepEquals(expectedReponse, actualJson);
    }

    [TestMethod]
    public async Task TestUpdatePermissions()
    {
        var shareName = "share123";
        var requestUri = $"{SharesApiUri}/{shareName}/permissions";

        // new values, same as in expected request
        var changes = new []
        {
            new PermissionsUpdate
            {
                Principal = "string",
                Add = [Privilege.SELECT],
                Remove = [Privilege.USAGE],
            }
        };

        var expectedRequest = @"
        {
          ""changes"": [
            {
              ""principal"": ""string"",
              ""add"": [
                ""SELECT""
              ],
              ""remove"": [
                ""USAGE""
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
                ""SELECT""
              ]
            }
          ]
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Patch, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = ApiClientTest.BaseApiUri;

        using var client = new SharesApiClient(mockClient);
        var actual = await client.UpdatePermissions(shareName, changes);
        var responseObj = new { privilege_assignments = actual };
        var actualJson = JsonSerializer.Serialize(responseObj, Options);

        AssertJsonDeepEquals(expectedResponse, actualJson);

        handler.VerifyRequest(
            HttpMethod.Patch,
            requestUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }
}

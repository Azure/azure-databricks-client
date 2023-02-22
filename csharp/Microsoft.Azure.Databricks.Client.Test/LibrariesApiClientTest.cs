// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class LibrariesApiClientTest : ApiClientTest
{
    private static readonly Uri LibrariesApiUri = new(BaseApiUri, "2.0/libraries/");

    [TestMethod]
    public async Task TestClusterStatus()
    {
        var apiUri = new Uri(LibrariesApiUri, "cluster-status");
        const string expectedResponse = @"
                {
                    ""cluster_id"": ""1234-567890-reef123"",
                    ""library_statuses"": [
                        {
                            ""library"": {
                                ""egg"": ""dbfs:/libraries/some-library.egg""
                            },
                            ""status"": ""PENDING"",
                            ""is_library_for_all_clusters"": false
                        },
                        {
                            ""library"": {
                                ""maven"": {
                                    ""coordinates"": ""com.microsoft.azure:azure-sqldb-spark:1.0.2""
                                }
                            },
                            ""status"": ""PENDING"",
                            ""is_library_for_all_clusters"": false
                        },
                        {
                            ""library"": {
                                ""pypi"": {
                                    ""package"": ""adal~=1.2.0""
                                }
                            },
                            ""status"": ""PENDING"",
                            ""is_library_for_all_clusters"": false
                        },
                        {
                            ""library"": {
                                ""pypi"": {
                                    ""package"": ""matplotlib~=3.5.0""
                                }
                            },
                            ""status"": ""PENDING"",
                            ""is_library_for_all_clusters"": false
                        }
                    ]
                }
                ";

        var libraryStatuses = JsonSerializer.Deserialize<JsonObject>(expectedResponse)!["library_statuses"];
        var expected = libraryStatuses.Deserialize<IEnumerable<LibraryFullStatus>>(Options);

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(apiUri, "?cluster_id=1234-567890-reef123"))
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new LibrariesApiClient(hc);
        var actual = await client.ClusterStatus("1234-567890-reef123");

        AssertJsonDeepEquals(
            JsonSerializer.Serialize(expected, Options),
            JsonSerializer.Serialize(actual, Options)
        );

        handler.VerifyRequest(
            HttpMethod.Get,
            new Uri(apiUri, "?cluster_id=1234-567890-reef123"),
            Times.Once()
        );
    }
}
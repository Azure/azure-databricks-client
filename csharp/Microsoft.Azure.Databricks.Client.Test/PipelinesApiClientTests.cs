using Microsoft.Azure.Databricks.Client.Models;
using Moq;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class PipelinesApiClientTest : ApiClientTest
{
    private static readonly Uri PipelineApiUri = new(BaseApiUri, "2.0/pipelines");

    [TestMethod]
    public async Task TestList()
    {
        var apiUri = $"{PipelineApiUri}?max_results=25";

        const string expectedResponse = @"
        {
          ""statuses"": [
            {
              ""pipeline_id"": ""string"",
              ""state"": ""DEPLOYING"",
              ""cluster_id"": ""string"",
              ""name"": ""string"",
              ""latest_updates"": [
                {
                  ""update_id"": ""string"",
                  ""state"": ""QUEUED"",
                  ""creation_time"": ""string""
                }
              ],
              ""creator_user_name"": ""string"",
              ""run_as_user_name"": ""string""
            }
          ],
          ""next_page_token"": ""string""
        }
        ";
        var expected = JsonSerializer.Deserialize<PipelinesList>(expectedResponse, Options);

        var handler = CreateMockHandler();
        handler.SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var actual = await client.List();

        var actualJson = JsonSerializer.Serialize(actual, Options);

        AssertJsonDeepEquals(expectedResponse, actualJson);
    }

    [TestMethod]
    public async Task TestGet()
    {
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}";

        const string expectedResponse = @"
        {
          ""pipeline_id"": ""string"",
          ""spec"": {
            ""id"": ""string"",
            ""name"": ""string"",
            ""storage"": ""string"",
            ""configuration"": {
              ""property1"": ""string"",
              ""property2"": ""string""
            },
            ""clusters"": [
              {
                ""label"": ""string"",
                ""spark_conf"": {
                  ""property1"": ""string"",
                  ""property2"": ""string""
                },
                ""azure_attributes"": {
                  ""log_analytics_info"": {
                    ""log_analytics_workspace_id"": ""string"",
                    ""log_analytics_primary_key"": ""string""
                  },
                  ""first_on_demand"": 1,
                  ""availability"": ""SPOT_AZURE"",
                  ""spot_bid_max_price"": -1
                },
                ""node_type_id"": ""string"",
                ""driver_node_type_id"": ""string"",
                ""ssh_public_keys"": [
                  ""string""
                ],
                ""custom_tags"": {
                  ""property1"": ""string"",
                  ""property2"": ""string""
                },
                ""cluster_log_conf"": {
                  ""dbfs"": {
                    ""destination"": ""string""
                  }
                },
                ""spark_env_vars"": {
                  ""property1"": ""string"",
                  ""property2"": ""string""
                },
                ""instance_pool_id"": ""string"",
                ""driver_instance_pool_id"": ""string"",
                ""policy_id"": ""string"",
                ""num_workers"": 0,
                ""autoscale"": {
                  ""min_workers"": 1,
                  ""max_workers"": 1
                },
                ""apply_policy_default_values"": true
              }
            ],
            ""libraries"": [
              {
                ""notebook"": {
                  ""path"": ""string""
                }
              },
              {
                ""file"": {
                  ""path"": ""string""
                }
              }
            ],
            ""target"": ""string"",
            ""filters"": {
              ""include"": [
                ""string""
              ],
              ""exclude"": [
                ""string""
              ]
            },
            ""continuous"": true,
            ""development"": true,
            ""photon"": true,
            ""edition"": ""string"",
            ""channel"": ""string"",
            ""catalog"": ""string"",
            ""notifications"": [
              {
                ""email_recipients"": [
                  ""string""
                ],
                ""alerts"": [
                  ""string""
                ]
              }
            ]
          },
          ""state"": ""DEPLOYING"",
          ""cause"": ""string"",
          ""cluster_id"": ""string"",
          ""name"": ""string"",
          ""health"": ""HEALTHY"",
          ""creator_user_name"": ""string"",
          ""latest_updates"": [
            {
              ""update_id"": ""string"",
              ""state"": ""QUEUED"",
              ""creation_time"": ""string""
            }
          ],
          ""last_modified"": 0,
          ""run_as_user_name"": ""string""
        }
        ";

        var expected = JsonSerializer.Deserialize<Pipeline>(expectedResponse, Options);
        var handler = CreateMockHandler();
        handler.SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var actual = await client.Get(pipelineId);

        var actualJson = JsonSerializer.Serialize<Pipeline>(actual, Options);
        AssertJsonDeepEquals(expectedResponse, actualJson);
    }

    [TestMethod]
    public async Task TestCreate()
    {
        const string expectedRequest = @"
        {
          ""id"": ""string"",
          ""name"": ""string"",
          ""storage"": ""string"",
          ""configuration"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""clusters"": [
            {
                ""label"": ""string"",
                ""spark_conf"": {
                ""property1"": ""string"",
                ""property2"": ""string""
                },
                ""azure_attributes"": {
                ""log_analytics_info"": {
                    ""log_analytics_workspace_id"": ""string"",
                    ""log_analytics_primary_key"": ""string""
                },
                ""first_on_demand"": 1,
                ""availability"": ""SPOT_AZURE"",
                ""spot_bid_max_price"": -1
                },
                ""node_type_id"": ""string"",
                ""driver_node_type_id"": ""string"",
                ""ssh_public_keys"": [
                ""string""
                ],
                ""custom_tags"": {
                ""property1"": ""string"",
                ""property2"": ""string""
                },
                ""cluster_log_conf"": {
                ""dbfs"": {
                    ""destination"": ""string""
                }
                },
                ""spark_env_vars"": {
                ""property1"": ""string"",
                ""property2"": ""string""
                },
                ""instance_pool_id"": ""string"",
                ""driver_instance_pool_id"": ""string"",
                ""policy_id"": ""string"",
                ""num_workers"": 0,
                ""autoscale"": {},
                ""apply_policy_default_values"": true
            }
          ],                    
          ""libraries"": [
            {
              ""notebook"": {
                ""path"": ""string""
              }
            }
          ],
          ""target"": ""string"",
          ""filters"": {
            ""include"": [
              ""string""
            ],
            ""exclude"": [
              ""string""
            ]
          },
          ""continuous"": true,
          ""development"": true,
          ""photon"": true,
          ""edition"": ""string"",
          ""channel"": ""string"",
          ""catalog"": ""string"",
          ""notifications"": [
            {
              ""email_recipients"": [
                ""string""
              ],
              ""alerts"": [
                ""on-update-success""
              ]
            }
          ],
          ""allow_duplicate_names"": false,
          ""dry_run"": true
        }
        ";

        var expectedResponse = new
        {
            pipeline_id = "1234-567890-cited123"
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, PipelineApiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse), "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var createdPipeline = JsonSerializer.Deserialize<PipelineSpecification>(expectedRequest, Options);

        var response = await client.Create(createdPipeline);
        var pipelineId = response.Item1;

        Assert.AreEqual(
            expectedResponse.pipeline_id,
            pipelineId);

        handler.VerifyRequest(
            HttpMethod.Post,
            PipelineApiUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestEdit()
    {
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}";
        const string expectedRequest = @"
        {
          ""id"": ""string"",
          ""name"": ""string"",
          ""storage"": ""string"",
          ""configuration"": {
            ""property1"": ""string"",
            ""property2"": ""string""
          },
          ""clusters"": [
            {
              ""label"": ""string"",
              ""spark_conf"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""azure_attributes"": {
                ""log_analytics_info"": {
                  ""log_analytics_workspace_id"": ""string"",
                  ""log_analytics_primary_key"": ""string""
                },
                ""first_on_demand"": 1,
                ""availability"": ""SPOT_AZURE"",
                ""spot_bid_max_price"": -1
              },
              ""node_type_id"": ""string"",
              ""driver_node_type_id"": ""string"",
              ""ssh_public_keys"": [
                ""string""
              ],
              ""custom_tags"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""cluster_log_conf"": {
                ""dbfs"": {
                  ""destination"": ""string""
                }
              },
              ""spark_env_vars"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""instance_pool_id"": ""string"",
              ""driver_instance_pool_id"": ""string"",
              ""policy_id"": ""string"",
              ""num_workers"": 0,
              ""autoscale"": {},
              ""apply_policy_default_values"": true
            }
          ],
          ""libraries"": [
            {
              ""notebook"": {
                ""path"": ""string""
              }   
            }
          ],
          ""target"": ""string"",
          ""filters"": {
            ""include"": [
              ""string""
            ],
            ""exclude"": [
              ""string""
            ]
          },
          ""continuous"": true,
          ""development"": true,
          ""photon"": true,
          ""edition"": ""string"",
          ""channel"": ""string"",
          ""catalog"": ""string"",
          ""notifications"": [
            {
              ""email_recipients"": [
                ""string""
              ],
              ""alerts"": [
                ""string""
              ]
            }
          ],
          ""allow_duplicate_names"": false
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Put, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var editPipeline = JsonSerializer.Deserialize<PipelineSpecification>(expectedRequest, Options);

        await client.Edit(pipelineId, editPipeline);

        handler.VerifyRequest(
            HttpMethod.Put,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestDelete()
    {
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Delete, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        await client.Delete(pipelineId);

        handler.VerifyRequest(
            HttpMethod.Delete,
            apiUri);
    }

    [TestMethod]
    public async Task TestReset()
    {
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}/reset";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        await client.Reset(pipelineId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri);
    }

    [TestMethod]
    public async Task TestStop()
    {
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}/stop";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK);

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        await client.Stop(pipelineId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri);
    }

    [TestMethod]
    public async Task TestGetUpdate()
    {
        var pipelineId = "1234-567890-cited123";
        var updateId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}/updates/{updateId}";

        // Two responses as get action will return only part of response from API

        const string mockResponse = @"
        {
          ""update"": {
            ""pipeline_id"": ""string"",
            ""update_id"": ""string"",
            ""config"": {
              ""id"": ""string"",
              ""name"": ""string"",
              ""storage"": ""string"",
              ""configuration"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""clusters"": [
                {
                  ""label"": ""string"",
                  ""spark_conf"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""azure_attributes"": {
                    ""log_analytics_info"": {
                      ""log_analytics_workspace_id"": ""string"",
                      ""log_analytics_primary_key"": ""string""
                    },
                    ""first_on_demand"": 1,
                    ""availability"": ""SPOT_AZURE"",
                    ""spot_bid_max_price"": -1
                  },
                  ""node_type_id"": ""string"",
                  ""driver_node_type_id"": ""string"",
                  ""ssh_public_keys"": [
                    ""string""
                  ],
                  ""custom_tags"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""cluster_log_conf"": {
                    ""dbfs"": {
                      ""destination"": ""string""
                    }
                  },
                  ""spark_env_vars"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""instance_pool_id"": ""string"",
                  ""driver_instance_pool_id"": ""string"",
                  ""policy_id"": ""string"",
                  ""num_workers"": 0,
                  ""autoscale"": {
                    ""min_workers"": 1,
                    ""max_workers"": 1
                  },
                  ""apply_policy_default_values"": true
                }
              ],
              ""libraries"": [
                {
                  ""notebook"": {
                    ""path"": ""string""
                  }
                }
              ],
              ""target"": ""string"",
              ""filters"": {
                ""include"": [
                  ""string""
                ],
                ""exclude"": [
                  ""string""
                ]
              },
              ""continuous"": true,
              ""development"": true,
              ""photon"": true,
              ""edition"": ""string"",
              ""channel"": ""string"",
              ""catalog"": ""string"",
              ""notifications"": [
                {
                  ""email_recipients"": [
                    ""string""
                  ],
                  ""alerts"": [
                    ""string""
                  ]
                }
              ]
            },
            ""cause"": ""API_CALL"",
            ""state"": ""QUEUED"",
            ""cluster_id"": ""string"",
            ""creation_time"": 0,
            ""full_refresh"": true,
            ""refresh_selection"": [
              ""string""
            ],
            ""full_refresh_selection"": [
              ""string""
            ]
          }
        }
        ";

        const string expectedResponse = @"
        {
            ""pipeline_id"": ""string"",
            ""update_id"": ""string"",
            ""config"": {
              ""id"": ""string"",
              ""name"": ""string"",
              ""storage"": ""string"",
              ""configuration"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""clusters"": [
                {
                  ""label"": ""string"",
                  ""spark_conf"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""azure_attributes"": {
                    ""log_analytics_info"": {
                      ""log_analytics_workspace_id"": ""string"",
                      ""log_analytics_primary_key"": ""string""
                    },
                    ""first_on_demand"": 1,
                    ""availability"": ""SPOT_AZURE"",
                    ""spot_bid_max_price"": -1
                  },
                  ""node_type_id"": ""string"",
                  ""driver_node_type_id"": ""string"",
                  ""ssh_public_keys"": [
                    ""string""
                  ],
                  ""custom_tags"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""cluster_log_conf"": {
                    ""dbfs"": {
                      ""destination"": ""string""
                    }
                  },
                  ""spark_env_vars"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""instance_pool_id"": ""string"",
                  ""driver_instance_pool_id"": ""string"",
                  ""policy_id"": ""string"",
                  ""num_workers"": 0,
                  ""autoscale"": {
                    ""min_workers"": 1,
                    ""max_workers"": 1
                  },
                  ""apply_policy_default_values"": true
                }
              ],
              ""libraries"": [
                {
                  ""notebook"": {
                    ""path"": ""string""
                  }
                }
              ],
              ""target"": ""string"",
              ""filters"": {
                ""include"": [
                  ""string""
                ],
                ""exclude"": [
                  ""string""
                ]
              },
              ""continuous"": true,
              ""development"": true,
              ""photon"": true,
              ""edition"": ""string"",
              ""channel"": ""string"",
              ""catalog"": ""string"",
              ""notifications"": [
                {
                  ""email_recipients"": [
                    ""string""
                  ],
                  ""alerts"": [
                    ""string""
                  ]
                }
              ]
            },
            ""cause"": ""API_CALL"",
            ""state"": ""QUEUED"",
            ""cluster_id"": ""string"",
            ""creation_time"": 0,
            ""full_refresh"": true,
            ""refresh_selection"": [
              ""string""
            ],
            ""full_refresh_selection"": [
              ""string""
            ]
          }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, mockResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var response = await client.GetUpdate(pipelineId, updateId);

        var responseJson = JsonSerializer.Serialize<PipelineUpdate>(response, Options);

        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestListUpdates()
    {
        var maxResults = 25;
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}/updates?max_results={maxResults}";
        var expectedResponse = @"
        {
          ""updates"": [
            {
            ""pipeline_id"": ""string"",
            ""update_id"": ""string"",
            ""config"": {
              ""id"": ""string"",
              ""name"": ""string"",
              ""storage"": ""string"",
              ""configuration"": {
                ""property1"": ""string"",
                ""property2"": ""string""
              },
              ""clusters"": [
                {
                  ""label"": ""string"",
                  ""spark_conf"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""azure_attributes"": {
                    ""log_analytics_info"": {
                      ""log_analytics_workspace_id"": ""string"",
                      ""log_analytics_primary_key"": ""string""
                    },
                    ""first_on_demand"": 1,
                    ""availability"": ""SPOT_AZURE"",
                    ""spot_bid_max_price"": -1
                  },
                  ""node_type_id"": ""string"",
                  ""driver_node_type_id"": ""string"",
                  ""ssh_public_keys"": [
                    ""string""
                  ],
                  ""custom_tags"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""cluster_log_conf"": {
                    ""dbfs"": {
                      ""destination"": ""string""
                    }
                  },
                  ""spark_env_vars"": {
                    ""property1"": ""string"",
                    ""property2"": ""string""
                  },
                  ""instance_pool_id"": ""string"",
                  ""driver_instance_pool_id"": ""string"",
                  ""policy_id"": ""string"",
                  ""num_workers"": 1,
                  ""autoscale"": {
                    ""min_workers"": 1,
                    ""max_workers"": 1
                  },
                  ""apply_policy_default_values"": true
                }
              ],
              ""libraries"": [
                {
                  ""notebook"": {
                    ""path"": ""string""
                  }
                }
              ],
              ""target"": ""string"",
              ""filters"": {
                ""include"": [
                  ""string""
                ],
                ""exclude"": [
                  ""string""
                ]
              },
              ""continuous"": true,
              ""development"": true,
              ""photon"": true,
              ""edition"": ""string"",
              ""channel"": ""string"",
              ""catalog"": ""string"",
              ""notifications"": [
                {
                  ""email_recipients"": [
                    ""string""
                  ],
                  ""alerts"": [
                    ""string""
                  ]
                }
              ]
            },
            ""cause"": ""API_CALL"",
            ""state"": ""QUEUED"",
            ""cluster_id"": ""string"",
            ""creation_time"": 0,
            ""full_refresh"": true,
            ""refresh_selection"": [
              ""string""
            ],
            ""full_refresh_selection"": [
              ""string""
            ]
          }
          ],
          ""next_page_token"": ""string"",
          ""prev_page_token"": ""string""
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var response = await client.ListUpdates(pipelineId, maxResults);

        var responseJson = JsonSerializer.Serialize<PipelineUpdatesList>(response, Options);

        AssertJsonDeepEquals(expectedResponse, responseJson);
    }

    [TestMethod]
    public async Task TestStart()
    {
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}/updates";

        var expectedRequest = @"
        {
          ""full_refresh"": ""true"",
          ""cause"": ""API_CALL"",
          ""refresh_selection"": [
            ""string1"", ""string2""
          ],
          ""full_refresh_selection"": [
             ""string1"", ""string2""
          ]

        }
        ";

        var expectedResponse = new
        {
            update_id = "1234-567890-cited123"
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse), "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        using var client = new PipelinesApiClient(mockClient);
        var response = await client.Start(
            pipelineId,
            fullRefresh: true,
            refreshSelection: new[] { "string1", "string2" },
            fullRefreshSelection: new[] { "string1", "string2" });

        Assert.AreEqual(expectedResponse.update_id, response);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once());
    }

    [TestMethod]
    public async Task TestListEvents()
    {
        var maxResults = 25;
        var pipelineId = "1234-567890-cited123";
        var apiUri = $"{PipelineApiUri}/{pipelineId}/events?max_results={maxResults}";

        const string expectedResponse = @"
        {
          ""next_page_token"": ""string"",
          ""prev_page_token"": ""string"",
          ""events"": [
            {
              ""id"": ""string"",
              ""sequence"": {
                ""data_plane_id"": {
                  ""instance"": ""string"",
                  ""seq_no"": {}
                },
                ""control_plane_seq_no"": 1
              },
              ""origin"": {
                ""cloud"": ""string"",
                ""region"": ""string"",
                ""org_id"": 1,
                ""pipeline_id"": ""string"",
                ""pipeline_name"": ""string"",
                ""cluster_id"": ""string"",
                ""update_id"": ""string"",
                ""maintenance_id"": ""string"",
                ""table_id"": ""string"",
                ""dataset_name"": ""string"",
                ""flow_id"": ""string"",
                ""flow_name"": ""string"",
                ""batch_id"": 1,
                ""request_id"": ""string"",
                ""uc_resource_id"": ""string"",
                ""host"": ""string"",
                ""materialization_name"": ""string""
              },
              ""timestamp"": ""string"",
              ""message"": ""string"",
              ""level"": ""INFO"",
              ""error"": {
                ""fatal"": true,
                ""exceptions"": [
                  {
                    ""class_name"": ""string"",
                    ""message"": ""string"",
                    ""stack"": [
                      {
                        ""declaring_class"": ""string"",
                        ""method_name"": ""string"",
                        ""file_name"": ""string"",
                        ""line_number"": 1
                      }
                    ]
                  }
                ]
              },
              ""event_type"": ""string"",
              ""maturity_level"": ""STABLE""
            }
          ]
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json");

        var mockClient = handler.CreateClient();
        mockClient.BaseAddress = BaseApiUri;

        var client = new PipelinesApiClient(mockClient);
        var response = await client.ListEvents(pipelineId, maxResults);

        var responseJson = JsonSerializer.Serialize<PipelineEventsList>(response, Options);

        AssertJsonDeepEquals(expectedResponse, responseJson);
    }
}

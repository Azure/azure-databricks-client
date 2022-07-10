using Microsoft.Azure.Databricks.Client.Models;
using Moq.Contrib.HttpClient;
using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;
using Castle.Core.Internal;
using Moq;

namespace Microsoft.Azure.Databricks.Client.Test;

[TestClass]
public class JobApiClientTest : ApiClientTest
{
    private static readonly Uri JobsApiUri = new(BaseApiUri, "2.1/jobs/");

    #region Test resource
    private const string MultiTaskJobJson = @"
            {
                ""name"": ""A multitask job"",
                ""tags"": {
                    ""cost-center"": ""engineering"",
                    ""team"": ""jobs""
                },
                ""tasks"": [{
                        ""task_key"": ""Sessionize"",
                        ""description"": ""Extracts session data from events"",
                        ""existing_cluster_id"": ""0923-164208-meows279"",
                        ""spark_jar_task"": {
                            ""main_class_name"": ""com.databricks.Sessionize"",
                            ""parameters"": [""--data"", ""dbfs:/path/to/data.json""]
                        },
                        ""libraries"": [{
                            ""jar"": ""dbfs:/mnt/databricks/Sessionize.jar""
                        }],
                        ""timeout_seconds"": 86400,
                        ""max_retries"": 3,
                        ""min_retry_interval_millis"": 2000,
                        ""retry_on_timeout"": false
                    },
                    {
                        ""task_key"": ""Orders_Ingest"",
                        ""description"": ""Ingests order data"",
                        ""job_cluster_key"": ""auto_scaling_cluster"",
                        ""spark_jar_task"": {
                            ""main_class_name"": ""com.databricks.OrdersIngest"",
                            ""parameters"": [""--data"", ""dbfs:/path/to/order-data.json""]
                        },
                        ""libraries"": [{
                            ""jar"": ""dbfs:/mnt/databricks/OrderIngest.jar""
                        }],
                        ""timeout_seconds"": 86400,
                        ""max_retries"": 3,
                        ""min_retry_interval_millis"": 2000,
                        ""retry_on_timeout"": false
                    },
                    {
                        ""task_key"": ""Match"",
                        ""description"": ""Matches orders with user sessions"",
                        ""depends_on"": [{
                                ""task_key"": ""Orders_Ingest""
                            },
                            {
                                ""task_key"": ""Sessionize""
                            }
                        ],
                        ""new_cluster"": {
                            ""spark_version"": ""7.3.x-scala2.12"",
                            ""node_type_id"": ""Standard_D3_v2"",
                            ""spark_conf"": {
                                ""spark.speculation"": ""true""
                            },
                            ""azure_attributes"": {
                                ""availability"": ""ON_DEMAND_AZURE"",
                                ""spot_bid_max_price"": -1,
                                ""first_on_demand"": 0
                            },
                            ""autoscale"": {
                                ""min_workers"": 2,
                                ""max_workers"": 16
                            }
                        },
                        ""notebook_task"": {
                            ""notebook_path"": ""/Users/user.name@databricks.com/Match"",
                            ""base_parameters"": {
                                ""name"": ""John Doe"",
                                ""age"": ""35""
                            }
                        },
                        ""timeout_seconds"": 86400,
                        ""max_retries"": 3,
                        ""min_retry_interval_millis"": 2000,
                        ""retry_on_timeout"": false
                    }
                ],
                ""job_clusters"": [{
                    ""job_cluster_key"": ""auto_scaling_cluster"",
                    ""new_cluster"": {
                        ""spark_version"": ""7.3.x-scala2.12"",
                        ""node_type_id"": ""Standard_D3_v2"",
                        ""spark_conf"": {
                            ""spark.speculation"": ""true""
                        },
                        ""azure_attributes"": {
                            ""availability"": ""ON_DEMAND_AZURE"",
                            ""spot_bid_max_price"": -1,
                            ""first_on_demand"": 0
                        },
                        ""autoscale"": {
                            ""min_workers"": 2,
                            ""max_workers"": 16
                        }
                    }
                }],
                ""email_notifications"": {
                    ""on_start"": [""user.name@databricks.com""],
                    ""on_success"": [""user.name@databricks.com""],
                    ""on_failure"": [""user.name@databricks.com""],
                    ""no_alert_for_skipped_runs"": false
                },
                ""timeout_seconds"": 86400,
                ""schedule"": {
                    ""quartz_cron_expression"": ""20 30 * * * ?"",
                    ""timezone_id"": ""Europe/London"",
                    ""pause_status"": ""PAUSED""
                },
                ""max_concurrent_runs"": 10,
                ""git_source"": null,
                ""format"": ""MULTI_TASK"",
                ""access_control_list"": [{
                        ""user_name"": ""jsmith@example.com"",
                        ""permission_level"": ""CAN_MANAGE""
                    },
                    {
                        ""group_name"": ""readonly-group@example.com"",
                        ""permission_level"": ""CAN_VIEW""
                    }
                ]
            }
            ";

    private const string MultiTaskRunJson = @"
            {
                ""run_name"": ""A multitask job run"",
                ""tasks"": [{
                        ""task_key"": ""Sessionize"",
                        ""existing_cluster_id"": ""0923-164208-meows279"",
                        ""spark_jar_task"": {
                            ""main_class_name"": ""com.databricks.Sessionize"",
                            ""parameters"": [""--data"", ""dbfs:/path/to/data.json""]
                        },
                        ""libraries"": [{
                            ""jar"": ""dbfs:/mnt/databricks/Sessionize.jar""
                        }],
                        ""timeout_seconds"": 86400
                    },
                    {
                        ""task_key"": ""Orders_Ingest"",
                        ""existing_cluster_id"": ""0923-164208-meows279"",
                        ""spark_jar_task"": {
                            ""main_class_name"": ""com.databricks.OrdersIngest"",
                            ""parameters"": [""--data"", ""dbfs:/path/to/order-data.json""]
                        },
                        ""libraries"": [{
                            ""jar"": ""dbfs:/mnt/databricks/OrderIngest.jar""
                        }],
                        ""timeout_seconds"": 86400
                    },
                    {
                        ""task_key"": ""Match"",
                        ""depends_on"": [{
                                ""task_key"": ""Orders_Ingest""
                            },
                            {
                                ""task_key"": ""Sessionize""
                            }
                        ],
                        ""new_cluster"": {
                            ""spark_version"": ""7.3.x-scala2.12"",
                            ""node_type_id"": ""Standard_D3_v2"",
                            ""spark_conf"": {
                                ""spark.speculation"": ""true""
                            },
                            ""azure_attributes"": {
                                ""availability"": ""ON_DEMAND_AZURE"",
                                ""spot_bid_max_price"": -1,
                                ""first_on_demand"": 0
                            },
                            ""autoscale"": {
                                ""min_workers"": 2,
                                ""max_workers"": 16
                            }
                        },
                        ""notebook_task"": {
                            ""notebook_path"": ""/Users/user.name@databricks.com/Match"",
                            ""base_parameters"": {
                                ""name"": ""John Doe"",
                                ""age"": ""35""
                            }
                        },
                        ""timeout_seconds"": 86400
                    }
                ],
                ""timeout_seconds"": 86400,
                ""git_source"": null,
                ""idempotency_token"": ""8f018174-4792-40d5-bcbc-3e6a527352c8"",
                ""access_control_list"": [{
                        ""user_name"": ""jsmith@example.com"",
                        ""permission_level"": ""CAN_MANAGE""
                    },
                    {
                        ""group_name"": ""readonly-group@example.com"",
                        ""permission_level"": ""CAN_VIEW""
                    }
                ]
            }
            ";

    private static IEnumerable<AccessControlRequest> CreateDefaultAccessControlRequest()
    {
        return new AccessControlRequest[]
        {
            new AccessControlRequestForUser
                {PermissionLevel = JobPermissionLevel.CAN_MANAGE, UserName = "jsmith@example.com"},
            new AccessControlRequestForGroup
                {PermissionLevel = JobPermissionLevel.CAN_VIEW, GroupName = "readonly-group@example.com"}
        };
    }

    private static ClusterAttributes CreateDefaultJobCluster()
    {
        var cluster = new ClusterAttributes()
            .WithRuntimeVersion(RuntimeVersions.Runtime_7_3)
            .WithNodeType(NodeTypes.Standard_D3_v2)
            .WithAutoScale(2, 16);
        cluster.SparkConfiguration = new Dictionary<string, string> { { "spark.speculation", "true" } };
        cluster.AzureAttributes = new AzureAttributes
            { Availability = AzureAvailability.ON_DEMAND_AZURE, FirstOnDemand = 0, SpotBidMaxPrice = -1 };
        return cluster;
    }

    private static RunSubmitSettings CreateDefaultRunSubmitSettings()
    {
        var cluster = CreateDefaultJobCluster();
        var runSubmit = new RunSubmitSettings
        {
            RunName = "A multitask job run",
            TimeoutSeconds = 86400
        };

        var sessionizeTask = new SparkJarTask
        {
            MainClassName = "com.databricks.Sessionize",
            Parameters = new List<string> {"--data", "dbfs:/path/to/data.json"}
        };

        var task1 = runSubmit.AddTask("Sessionize", sessionizeTask, timeoutSeconds: 86400)
            .AttachLibrary(new JarLibrary {Jar = "dbfs:/mnt/databricks/Sessionize.jar"})
            .WithExistingClusterId("0923-164208-meows279");

        var ingestTask = new SparkJarTask
        {
            MainClassName = "com.databricks.OrdersIngest",
            Parameters = new List<string> {"--data", "dbfs:/path/to/order-data.json"}
        };


        var task2 = runSubmit.AddTask("Orders_Ingest", ingestTask, timeoutSeconds: 86400)
            .AttachLibrary(new JarLibrary {Jar = "dbfs:/mnt/databricks/OrderIngest.jar"})
            .WithExistingClusterId("0923-164208-meows279");

        var matchTask = new NotebookTask
        {
            NotebookPath = "/Users/user.name@databricks.com/Match",
            BaseParameters = new Dictionary<string, string>
            {
                {"name", "John Doe"}, {"age", "35"}
            }
        };

        runSubmit.AddTask("Match", matchTask, new[] {task2, task1}, timeoutSeconds: 86400)
            .WithNewCluster(cluster);

        return runSubmit;
    }

    private static JobSettings CreateDefaultJobSettings()
    {
        var cluster = CreateDefaultJobCluster();

        var job = new JobSettings
        {
            Name = "A multitask job",
            Tags = new Dictionary<string, string> {{"cost-center", "engineering"}, {"team", "jobs"}},
            JobClusters = new List<JobCluster>
            {
                new()
                {
                    JobClusterKey = "auto_scaling_cluster",
                    NewCluster = cluster
                }
            },
            EmailNotifications = new JobEmailNotifications
            {
                NoAlertForSkippedRuns = false,
                OnStart = new[] {"user.name@databricks.com"},
                OnFailure = new[] {"user.name@databricks.com"},
                OnSuccess = new[] {"user.name@databricks.com"}
            },
            TimeoutSeconds = 86400,
            Schedule = new CronSchedule
            {
                QuartzCronExpression = "20 30 * * * ?",
                PauseStatus = PauseStatus.PAUSED,
                TimezoneId = "Europe/London"
            },
            MaxConcurrentRuns = 10,
            Format = JobFormat.MULTI_TASK
        };

        var sessionizeTask = new SparkJarTask
        {
            MainClassName = "com.databricks.Sessionize",
            Parameters = new List<string> { "--data", "dbfs:/path/to/data.json" }
        };

        var task1 = job.AddTask("Sessionize", sessionizeTask, timeoutSeconds: 86400)
            .WithRetry(3, 2000, false)
            .AttachLibrary(new JarLibrary {Jar = "dbfs:/mnt/databricks/Sessionize.jar"})
            .WithExistingClusterId("0923-164208-meows279")
            .WithDescription("Extracts session data from events");

        var ingestTask = new SparkJarTask
        {
            MainClassName = "com.databricks.OrdersIngest",
            Parameters = new List<string> { "--data", "dbfs:/path/to/order-data.json" }
        };

        var task2 = job.AddTask("Orders_Ingest", ingestTask, timeoutSeconds: 86400)
            .WithJobClusterKey("auto_scaling_cluster")
            .WithRetry(3, 2000, false)
            .AttachLibrary(new JarLibrary { Jar = "dbfs:/mnt/databricks/OrderIngest.jar" })
            .WithDescription("Ingests order data");

        var matchTask = new NotebookTask
        {
            NotebookPath = "/Users/user.name@databricks.com/Match",
            BaseParameters = new Dictionary<string, string>
            {
                {"name", "John Doe"}, {"age", "35"}
            }
        };

        job.AddTask("Match", matchTask, new[] { task2, task1 }, timeoutSeconds: 86400)
            .WithRetry(3, 2000, false)
            .WithNewCluster(cluster)
            .WithDescription("Matches orders with user sessions");

        return job;
    }
    
    #endregion

    [TestMethod]
    public async Task TestCreate()
    {
        var apiUri = new Uri(JobsApiUri, "create");
        var expectedResponse = new { job_id = 11223344 };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options),
                "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var job = CreateDefaultJobSettings();
        var acr = CreateDefaultAccessControlRequest();
        var jobId = await client.Create(job, acr);
        Assert.AreEqual(expectedResponse.job_id, jobId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(MultiTaskJobJson),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestReset()
    {
        var apiUri = new Uri(JobsApiUri, "reset");

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        
        var job = CreateDefaultJobSettings();

        await client.Reset(11223344, job);

        var jobReset = JsonNode.Parse(MultiTaskJobJson)!.AsObject();
        jobReset.Remove("access_control_list");
        var request = new JsonObject {new("job_id", JsonValue.Create(11223344)), new("new_settings", jobReset)};
            
        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(request.ToJsonString(Options)),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestUpdate()
    {
        var apiUri = new Uri(JobsApiUri, "update");

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var job = CreateDefaultJobSettings();

        await client.Update(11223344, job, new []{ "libraries", "schedule" });

        var jobReset = JsonNode.Parse(MultiTaskJobJson)!.AsObject();
        jobReset.Remove("access_control_list");
        var request = new JsonObject
        {
            new("job_id", JsonValue.Create(11223344)),
            new("new_settings", jobReset),
            new("fields_to_remove", new JsonArray("libraries", "schedule"))
        };

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(request.ToJsonString(Options)),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestDelete()
    {
        const string expectedRequest = @"
            {
              ""job_id"": 11223344
            }
        ";

        var apiUri = new Uri(JobsApiUri, "delete");
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        await client.Delete(11223344);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestGet()
    {
        var apiUri = new Uri(JobsApiUri, "get");

        var expectedResponse = new Job
        {
            CreatedTime = DateTimeOffset.FromUnixTimeMilliseconds(0),
            CreatorUserName = "user.name@databricks.com",
            JobId = 11223344,
            RunAsUserName = "user.name@databricks.com",
            Settings = CreateDefaultJobSettings()
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(apiUri, "?job_id=11223344"))
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var job = await client.Get(11223344);

        AssertJsonDeepEquals(
            JsonSerializer.Serialize(expectedResponse, Options),
            JsonSerializer.Serialize(job, Options)
        );

        handler.VerifyRequest(
            HttpMethod.Get,
            new Uri(apiUri, "?job_id=11223344"),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestList()
    {
        var apiUri = new Uri(JobsApiUri, "list");

        var expectedResponse = new JobList
            {
                Jobs = new[]
                {
                    new Job
                    {
                        CreatedTime = DateTimeOffset.FromUnixTimeMilliseconds(0),
                        CreatorUserName = "user.name@databricks.com",
                        JobId = 11223344,
                        RunAsUserName = "user.name@databricks.com",
                        Settings = CreateDefaultJobSettings()
                    }
                },
                HasMore = false
            };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, JsonSerializer.Serialize(expectedResponse, Options), "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var job = await client.List();

        AssertJsonDeepEquals(
            JsonSerializer.Serialize(expectedResponse, Options),
            JsonSerializer.Serialize(job, Options)
        );

        handler.VerifyRequest(
            HttpMethod.Get,
            apiUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunNow()
    {
        var apiUri = new Uri(JobsApiUri, "run-now");

        const string expectedRequest = @"
            {
              ""job_id"": 11223344,
              ""idempotency_token"": ""8f018174-4792-40d5-bcbc-3e6a527352c8"",
              ""jar_params"": [
                ""john"",
                ""doe"",
                ""35""
              ],
              ""notebook_params"": {
                ""name"": ""john doe"",
                ""age"": ""35""
              },
              ""python_params"": [
                ""john doe"",
                ""35""
              ],
              ""spark_submit_params"": [
                ""--class"",
                ""org.apache.spark.examples.SparkPi""
              ],
              ""python_named_params"": {
                ""name"": ""task"",
                ""data"": ""dbfs:/path/to/data.json""
              }
            }
        ";

        const string expectedResponse = @"
            {
              ""run_id"": 455644833,
              ""number_in_job"": 455644833
            }
        ";

        var runParams = new RunParameters
        {
            JarParams = new List<string> {"john", "doe", "35"},
            NotebookParams = new Dictionary<string, string> {{"name", "john doe"}, {"age", "35"}},
            PythonParams = new List<string> {"john doe", "35"},
            SparkSubmitParams = new List<string> {"--class", "org.apache.spark.examples.SparkPi"},
            PythonNamedParams = new Dictionary<string, string> {{"name", "task"}, {"data", "dbfs:/path/to/data.json"}}
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        var runId = await client.RunNow(11223344, runParams, "8f018174-4792-40d5-bcbc-3e6a527352c8");

        Assert.AreEqual(455644833, runId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunNowNoParam()
    {
        var apiUri = new Uri(JobsApiUri, "run-now");

        const string expectedRequest = @"
            {
              ""job_id"": 11223344,
              ""idempotency_token"": ""8f018174-4792-40d5-bcbc-3e6a527352c8""
            }
        ";

        const string expectedResponse = @"
            {
              ""run_id"": 455644833,
              ""number_in_job"": 455644833
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        var runId = await client.RunNow(11223344, idempotencyToken: "8f018174-4792-40d5-bcbc-3e6a527352c8");

        Assert.AreEqual(455644833, runId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(expectedRequest),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunSubmit()
    {
        var apiUri = new Uri(JobsApiUri, "runs/submit");

        const string expectedResponse = @"
            {
              ""run_id"": 455644833
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var run = CreateDefaultRunSubmitSettings();
        var acr = CreateDefaultAccessControlRequest();
        var runId = await client.RunSubmit(run, acr, "8f018174-4792-40d5-bcbc-3e6a527352c8");

        Assert.AreEqual(455644833, runId);

        handler.VerifyRequest(
            HttpMethod.Post,
            apiUri,
            GetMatcher(MultiTaskRunJson),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunsGet()
    {
        var apiUri = new Uri(JobsApiUri, "runs/get");

        const string response = @"
                {
                  ""job_id"": 11223344,
                  ""run_id"": 455644833,
                  ""creator_user_name"": ""user.name@databricks.com"",
                  ""original_attempt_run_id"": 455644833,
                  ""state"": {
                    ""life_cycle_state"": ""PENDING"",
                    ""result_state"": ""SUCCESS"",
                    ""user_cancelled_or_timedout"": false,
                    ""state_message"": """"
                  },
                  ""schedule"": {
                    ""quartz_cron_expression"": ""20 30 * * * ?"",
                    ""timezone_id"": ""Europe/London"",
                    ""pause_status"": ""PAUSED""
                  },
                  ""tasks"": [
                    {
                      ""run_id"": 2112892,
                      ""task_key"": ""Orders_Ingest"",
                      ""description"": ""Ingests order data"",
                      ""existing_cluster_id"": ""0923-164208-meows279"",
                      ""spark_jar_task"": {
                        ""main_class_name"": ""com.databricks.OrdersIngest""
                      },
                      ""state"": {
                        ""life_cycle_state"": ""INTERNAL_ERROR"",
                        ""result_state"": ""FAILED"",
                        ""state_message"": """",
                        ""user_cancelled_or_timedout"": false
                      },
                      ""cluster_instance"": {
                        ""cluster_id"": ""0923-164208-meows279"",
                        ""spark_context_id"": ""4348585301701786933""
                      },
                      ""attempt_number"": 0
                    }],
                  ""cluster_spec"": {
                    ""existing_cluster_id"": ""0923-164208-meows279""
                  },
                  ""cluster_instance"": {
                    ""cluster_id"": ""0923-164208-meows279"",
                    ""spark_context_id"": ""4348585301701786933""
                  },
                  ""git_source"": null,
                  ""trigger"": ""PERIODIC"",
                  ""run_name"": ""A multitask job run"",
                  ""run_page_url"": ""https://my-workspace.cloud.databricks.com/#job/39832/run/20"",
                  ""run_type"": ""JOB_RUN"",
                  ""attempt_number"": 0,
                  ""repair_history"": [
                    {
                      ""type"": ""ORIGINAL"",
                      ""start_time"": 1625060460483,
                      ""end_time"": 1625060863413,
                      ""state"": {
                        ""life_cycle_state"": ""TERMINATED"",
                        ""result_state"": ""SUCCESS"",
                        ""user_cancelled_or_timedout"": false,
                        ""state_message"": """"
                      },
                      ""id"": 734650698524280,
                      ""task_run_ids"": [
                        1106460542112844,
                        988297789683452
                      ]
                    }
                  ]
                }
        ";

        var expectedRun = new Run
        {
            JobId = 11223344,
            RunId = 455644833,
            CreatorUserName = "user.name@databricks.com",
            OriginalAttemptRunId = 455644833,
            State = new RunState
            {
                LifeCycleState = RunLifeCycleState.PENDING,
                ResultState = RunResultState.SUCCESS,
                UserCancelledOrTimedOut = false,
                StateMessage = ""
            },
            Schedule = new CronSchedule
            {
                QuartzCronExpression = "20 30 * * * ?",
                TimezoneId = "Europe/London",
                PauseStatus = PauseStatus.PAUSED
            },
            Tasks = new[]
            {
                new RunTask
                {
                    RunId = 2112892,
                    TaskKey = "Orders_Ingest",
                    Description = "Ingests order data",
                    ExistingClusterId = "0923-164208-meows279",
                    SparkJarTask = new SparkJarTask
                    {
                        MainClassName = "com.databricks.OrdersIngest"
                    },
                    State = new RunState
                    {
                        LifeCycleState = RunLifeCycleState.INTERNAL_ERROR,
                        ResultState = RunResultState.FAILED,
                        UserCancelledOrTimedOut = false,
                        StateMessage = ""
                    },
                    ClusterInstance = new ClusterInstance
                    {
                        ClusterId = "0923-164208-meows279",
                        SparkContextId = "4348585301701786933"
                    },
                    AttemptNumber = 0
                }
            },
            ClusterSpec = new ClusterSpec
            {
                ExistingClusterId = "0923-164208-meows279"
            },
            ClusterInstance = new ClusterInstance
            {
                ClusterId = "0923-164208-meows279",
                SparkContextId = "4348585301701786933"
            },
            Trigger = TriggerType.PERIODIC,
            RunName = "A multitask job run",
            RunPageUrl = "https://my-workspace.cloud.databricks.com/#job/39832/run/20",
            RunType = RunType.JOB_RUN,
            AttemptNumber = 0
        };

        var expectedRepair = new RepairHistory
        {
            Items = new List<RepairHistoryItem>
            {
                new()
                {
                    Type = RepairHistoryItemType.ORIGINAL,
                    StartTime = DateTimeOffset.FromUnixTimeMilliseconds(1625060460483),
                    EndTime = DateTimeOffset.FromUnixTimeMilliseconds(1625060863413),
                    State = new RunState
                    {
                        LifeCycleState = RunLifeCycleState.TERMINATED,
                        ResultState = RunResultState.SUCCESS,
                        UserCancelledOrTimedOut = false,
                        StateMessage = ""
                    },
                    Id = 734650698524280,
                    TaskRunIds = new []{1106460542112844, 988297789683452}
                }
            }
        };

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, new Uri(apiUri, "?run_id=455644833&include_history=true"))
            .ReturnsResponse(HttpStatusCode.OK, response, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var (run, repair) = await client.RunsGet(455644833, true);

        AssertJsonDeepEquals(
            JsonSerializer.Serialize(expectedRun, Options),
            JsonSerializer.Serialize(run, Options)
        );

        AssertJsonDeepEquals(
            JsonSerializer.Serialize(expectedRepair, Options),
            JsonSerializer.Serialize(repair, Options)
        );

        handler.VerifyRequest(
            HttpMethod.Get,
            new Uri(apiUri, "?run_id=455644833&include_history=true"),
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunsList()
    {
        var apiUri = new Uri(JobsApiUri, "runs/list");
        
        var expectedRequestUrl = new Uri(apiUri,
            "?limit=25&offset=0&job_id=11223344&active_only=true&run_type=JOB_RUN&start_time_from=1642521600000&start_time_to=1642608000000");
        const string response = @"
            {
                ""runs"":[],
                ""has_more"": false
            }
        ";
        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, expectedRequestUrl)
            .ReturnsResponse(HttpStatusCode.OK, response, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        var runsList = await client.RunsList(
            jobId: 11223344, offset: 0, limit: 25, activeOnly: true, completedOnly: false, runType: RunType.JOB_RUN,
            expandTasks: false,
            startTimeFrom: DateTimeOffset.FromUnixTimeMilliseconds(1642521600000),
            startTimeTo: DateTimeOffset.FromUnixTimeMilliseconds(1642608000000)
        );

        Assert.IsTrue(runsList.Runs.IsNullOrEmpty());
        Assert.IsFalse(runsList.HasMore);

        handler.VerifyRequest(
            HttpMethod.Get,
            expectedRequestUrl,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunsExport()
    {
        var apiUri = new Uri(JobsApiUri, "runs/export");

        var expectedRequestUri = new Uri(apiUri, "?run_id=455644833&views_to_export=DASHBOARDS");

        const string response = @"
        {
          ""views"": [
            {
              ""content"": ""notebook content"",
              ""name"": ""notebook name"",
              ""type"": ""NOTEBOOK""
            }
          ]
        }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, expectedRequestUri)
            .ReturnsResponse(HttpStatusCode.OK, response, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        var views = await client.RunsExport(455644833, ViewsToExport.DASHBOARDS);
        var wrapped = new {views};
        AssertJsonDeepEquals(response, JsonSerializer.Serialize(wrapped, Options));
        handler.VerifyRequest(HttpMethod.Get, expectedRequestUri, Times.Once());
    }

    [TestMethod]
    public async Task TestRunsDelete()
    {
        var apiUri = new Uri(JobsApiUri, "runs/delete");

        const string expectedRequest = @"
            {
                ""run_id"": 455644833
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        await client.RunsDelete(455644833);
        handler.VerifyRequest(HttpMethod.Post, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestRunsCancel()
    {
        var apiUri = new Uri(JobsApiUri, "runs/cancel");

        const string expectedRequest = @"
            {
                ""run_id"": 455644833
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        await client.RunsCancel(455644833);
        handler.VerifyRequest(HttpMethod.Post, apiUri, GetMatcher(expectedRequest), Times.Once());
    }

    [TestMethod]
    public async Task TestRunsGetOutput()
    {
        var apiUri = new Uri(JobsApiUri, "runs/get-output");
        var requestUri = new Uri(apiUri, "?run_id=455644833");
        const string response = @"
            {
                ""notebook_output"": {
                    ""result"": ""An arbitrary string passed by calling dbutils.notebook.exit(...)"",
                    ""truncated"": false
                },
                ""logs"": ""Hello World!"",
                ""logs_truncated"": true,
                ""error"": ""ZeroDivisionError: integer division or modulo by zero"",
                ""error_trace"": ""---------------------------------------------------------------------------\nException Traceback (most recent call last)\n 1 numerator = 42\n 2 denominator = 0\n----> 3 return numerator / denominator\n\nZeroDivisionError: integer division or modulo by zero"",
                ""metadata"": {
                    ""git_source"": null
                }
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Get, requestUri)
            .ReturnsResponse(HttpStatusCode.OK, response, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);

        var runOutput = await client.RunsGetOutput(455644833);
        AssertJsonDeepEquals(response, JsonSerializer.Serialize(runOutput, Options));

        handler.VerifyRequest(
            HttpMethod.Get,
            requestUri,
            Times.Once()
        );
    }

    [TestMethod]
    public async Task TestRunsRepair()
    {
        var apiUri = new Uri(JobsApiUri, "runs/repair");

        const string expectedRequest = @"
            {
              ""run_id"": 455644833,
              ""rerun_tasks"": [
                ""task0"",
                ""task1""
              ],
              ""latest_repair_id"": 734650698524280,
              ""jar_params"": [
                ""john"",
                ""doe"",
                ""35""
              ]
            }
        ";

        const string expectedResponse = @"
            {
              ""repair_id"": 734650698524281
            }
        ";

        var handler = CreateMockHandler();
        handler
            .SetupRequest(HttpMethod.Post, apiUri)
            .ReturnsResponse(HttpStatusCode.OK, expectedResponse, "application/json")
            .Verifiable();

        var hc = handler.CreateClient();
        hc.BaseAddress = BaseApiUri;

        using var client = new JobsApiClient(hc);
        var repairInput = new RepairRunInput
        {
            LatestRepairId = 734650698524280,
            RunId = 455644833,
            RerunTasks = new[] {"task0", "task1"}
        };

        var repairParam = new RunParameters
        {
            JarParams = new List<string> {"john", "doe", "35"}
        };

        var repairId = await client.RunsRepair(repairInput, repairParam);

        Assert.AreEqual(734650698524281, repairId);
        handler.VerifyRequest(HttpMethod.Post, apiUri, GetMatcher(expectedRequest), Times.Once());
    }
}
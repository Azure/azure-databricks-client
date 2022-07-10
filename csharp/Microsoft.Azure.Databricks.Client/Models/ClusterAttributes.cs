using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public enum ClusterMode
    {
        /// <summary>
        /// The standard cluster mode. Recommended for single-user clusters. Can run SQL, Python, R, and Scala workloads.
        /// </summary>
        Standard,

        /// <summary>
        /// High concurrency cluster mode. Optimized to run concurrent SQL, Python, and R workloads. Does not support Scala. Previously known as Serverless. <see href="https://docs.microsoft.com/en-us/azure/databricks/clusters/configure#high-concurrency"/>
        /// </summary>
        HighConcurrency,

        /// <summary>
        /// A Single Node cluster is a cluster consisting of a Spark driver and no Spark workers. <see href="https://docs.microsoft.com/en-us/azure/databricks/clusters/single-node"/>
        /// </summary>
        SingleNode
    }

    public record ClusterAttributes: ClusterSize
    {
        /// <summary>
        /// Cluster name requested by the user. This doesn’t have to be unique. If not specified at creation, the cluster name will be an empty string.
        /// </summary>
        [JsonPropertyName("cluster_name")]
        public string ClusterName { get; set; }

        /// <summary>
        /// The Spark version of the cluster. A list of available Spark versions can be retrieved by using the Spark Versions API call.
        /// </summary>
        [JsonPropertyName("spark_version")]
        public string RuntimeVersion { get; set; }

        /// <summary>
        /// An object containing a set of optional, user-specified Spark configuration key-value pairs. Users can also pass in a string of extra JVM options to the driver and the executors via spark.driver.extraJavaOptions and spark.executor.extraJavaOptions respectively.
        /// Example Spark confs: {"spark.speculation": true, "spark.streaming.ui.retainedBatches": 5} or {"spark.driver.extraJavaOptions": "-verbose:gc -XX:+PrintGCDetails"}
        /// </summary>
        [JsonPropertyName("spark_conf")]
        public Dictionary<string, string> SparkConfiguration { get; set; }

        /// <summary>
        /// This field encodes, through a single value, the resources available to each of the Spark nodes in this cluster. For example, the Spark nodes can be provisioned and optimized for memory or compute intensive workloads A list of available node types can be retrieved by using the List Node Types API call. This field is required.
        /// </summary>
        [JsonPropertyName("node_type_id")]
        public string NodeTypeId { get; set; }

        /// <summary>
        /// The node type of the Spark driver. Note that this field is optional; if unset, the driver node type will be set as the same value as node_type_id defined above.
        /// </summary>
        [JsonPropertyName("driver_node_type_id")]
        public string DriverNodeTypeId { get; set; }

        /// <summary>
        /// SSH public key contents that will be added to each Spark node in this cluster. The corresponding private keys can be used to login with the user name ubuntu on port 2200. Up to 10 keys can be specified.
        /// </summary>
        [JsonPropertyName("ssh_public_keys")]
        public IEnumerable<string> SshPublicKeys { get; set; }

        /// <summary>
        /// Additional tags for cluster resources. Databricks will tag all cluster resources (e.g., VMs disk volumes) with these tags in addition to default_tags.
        /// Azure Databricks allows at most 8 custom tags.
        /// </summary>
        [JsonPropertyName("custom_tags")]
        public Dictionary<string, string> CustomTags { get; set; }

        /// <summary>
        /// The configuration for delivering spark logs to a long-term storage destination. Only one destination can be specified for one cluster. If the conf is given, the logs will be delivered to the destination every 5 mins. The destination of driver logs is $destination/$clusterId/driver, while the destination of executor logs is $destination/$clusterId/executor.
        /// </summary>
        [JsonPropertyName("cluster_log_conf")]
        public ClusterLogConf ClusterLogConfiguration { get; set; }

        /// <summary>
        /// The configuration for storing init scripts. 
        /// </summary>
        /// <remarks>
        /// Any number of destinations can be specified. The scripts are executed sequentially in the order provided.
        /// If cluster_log_conf is specified, init script logs are sent to &lt;destination&gt;/&lt;cluster-id&gt;/init_scripts.
        /// </remarks>
        [JsonPropertyName("init_scripts")]
        public IEnumerable<InitScriptInfo> InitScripts { get; set; }

        /// <summary>
        /// An object containing a set of optional, user-specified environment variable key-value pairs. Please note that key-value pair of the form (X,Y) will be exported as is (i.e., export X='Y') while launching the driver and workers.
        ///In order to specify an additional set of SPARK_DAEMON_JAVA_OPTS, we recommend appending them to $SPARK_DAEMON_JAVA_OPTS as shown in the example below.This ensures that all default databricks managed environmental variables are included as well.
        ///Example Spark environment variables: {"SPARK_WORKER_MEMORY": "28000m", "SPARK_LOCAL_DIRS": "/local_disk0"}
        ///or {"SPARK_DAEMON_JAVA_OPTS": "$SPARK_DAEMON_JAVA_OPTS -Dspark.shuffle.service.enabled=true"}
        /// </summary>
        [JsonPropertyName("spark_env_vars")]
        public Dictionary<string, string> SparkEnvironmentVariables { get; set; }

        /// <summary>
        /// Automatically terminates the cluster after it is inactive for this time in minutes. If not set, this cluster will not be automatically terminated. If specified, the threshold must be between 10 and 10000 minutes. Users can also set this value to 0 to explicitly disable automatic termination.
        /// </summary>
        [JsonPropertyName("autotermination_minutes")]
        public int AutoTerminationMinutes { get; set; }

        /// <summary>
        /// Autoscaling Local Storage: when enabled, this cluster will dynamically acquire additional disk space when its Spark workers are running low on disk space.
        /// </summary>
        [JsonPropertyName("enable_elastic_disk")]
        public bool EnableElasticDisk { get; set; }

        /// <summary>
        /// The optional ID of the instance pool to which the cluster belongs. Refer to Pools for details.
        /// </summary>
        [JsonPropertyName("instance_pool_id")]
        public string InstancePoolId { get; set; }

        /// <summary>
        /// The ID of the instance pool to use for drivers. You must also specify instance_pool_id. Refer to Instance Pools API 2.0 for details.
        /// </summary>
        [JsonPropertyName("driver_instance_pool_id")]
        public string DriverInstancePoolId { get; set; }

        /// <summary>
        /// Docker image for a custom container.
        /// </summary>
        [JsonPropertyName("docker_image")]
        public DockerImage DockerImage { get; set; }

        /// <summary>
        /// When enabled, local disk data will be encrypted at-rest. Contact your Microsoft or Databricks account representative to enable for your subscription.
        /// </summary>
        [JsonPropertyName("enable_local_disk_encryption")]
        public bool LocalDiskEncryption { get; set; }

        /// <summary>
        /// A cluster policy ID.
        /// </summary>
        [JsonPropertyName("policy_id")]
        public string PolicyId { get; set; }

        /// <summary>
        /// Whether to use policy default values for missing cluster attributes. Default value: false.
        /// </summary>
        [JsonPropertyName("apply_policy_default_values")]
        public bool? ApplyPolicyDefaultValues { get; set; }

        /// <summary>
        /// Defines attributes such as the instance availability type, node placement, and max bid price. If not specified during cluster creation, a set of default values is used.
        /// </summary>
        [JsonPropertyName("azure_attributes")]
        public AzureAttributes AzureAttributes { get; set; }

        /// <summary>
        /// The type of runtime engine to use. If not specified, the runtime engine type is inferred based on the spark_version value.Allowed values include:
        /// * PHOTON: Use the Photon runtime engine type.
        /// * STANDARD: Use the standard runtime engine type.
        /// </summary>
        [JsonPropertyName("runtime_engine")]
        public RuntimeEngine RuntimeEngine { get; set; }

        public static ClusterAttributes GetNewClusterConfiguration(string clusterName = null)
        {
            return new ClusterAttributes
            {
                ClusterName = clusterName
            };
        }

        public ClusterAttributes WithAutoScale(int minWorkers, int maxWorkers)
        {
            AutoScale = new AutoScale { MinWorkers = minWorkers, MaxWorkers = maxWorkers };
            NumberOfWorkers = null;
            return this;
        }

        public ClusterAttributes WithNumberOfWorkers(int numWorkers)
        {
            NumberOfWorkers = numWorkers;
            AutoScale = null;
            return this;
        }

        private static string DatabricksAllowedReplLang(bool enableTableAccessControl, ClusterMode clusterMode) =>
            enableTableAccessControl ? "python,sql" : clusterMode == ClusterMode.HighConcurrency ? "sql,python,r" : null;

        /// <summary>
        /// When enabled:
        ///     Allows users to run SQL, Python, and PySpark commands. Users are restricted to the SparkSQL API and DataFrame API, and therefore cannot use Scala, R, RDD APIs, or clients that directly read the data from cloud storage, such as DBUtils.
        ///     Cannot acquire direct access to data in the cloud via DBFS or by reading credentials from the cloud provider’s metadata service.
        ///     Requires that clusters run Databricks Runtime 3.5 or above.
        ///     Must run their commands on cluster nodes as a low-privilege user forbidden from accessing sensitive parts of the filesystem or creating network connections to ports other than 80 and 443.
        /// </summary>
        private bool _enableTableAccessControl;
        public ClusterAttributes WithTableAccessControl(bool enableTableAccessControl)
        {
            _enableTableAccessControl = enableTableAccessControl;

            SparkConfiguration ??= new Dictionary<string, string>();

            if (enableTableAccessControl)
            {
                SparkConfiguration["spark.databricks.acl.dfAclsEnabled"] = "true";
            }
            else
            {
                SparkConfiguration.Remove("spark.databricks.acl.dfAclsEnabled");
            }

            var allowedReplLang = DatabricksAllowedReplLang(enableTableAccessControl, _clusterMode);

            if (string.IsNullOrEmpty(allowedReplLang))
            {
                SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }
            else
            {
                SparkConfiguration["spark.databricks.repl.allowedLanguages"] = allowedReplLang;
            }

            return this;
        }

        private ClusterMode _clusterMode = ClusterMode.Standard;

        public ClusterAttributes WithClusterMode(ClusterMode clusterMode)
        {
            _clusterMode = clusterMode;

            CustomTags ??= new Dictionary<string, string>();

            SparkConfiguration ??= new Dictionary<string, string>();

            switch (clusterMode)
            {
                case ClusterMode.HighConcurrency:
                    CustomTags["ResourceClass"] = "Serverless";
                    SparkConfiguration["spark.databricks.cluster.profile"] = "serverless";
                    SparkConfiguration.Remove("spark.master");
                    break;
                case ClusterMode.SingleNode:
                    CustomTags["ResourceClass"] = "SingleNode";
                    SparkConfiguration["spark.databricks.cluster.profile"] = "singleNode";
                    SparkConfiguration["spark.master"] = "local[*]";
                    NumberOfWorkers = 0;
                    break;
                case ClusterMode.Standard:
                default: // Standard mode
                    CustomTags.Remove("ResourceClass");
                    SparkConfiguration.Remove("spark.databricks.cluster.profile");
                    SparkConfiguration.Remove("spark.master");
                    break;
            }

            var allowedReplLang = DatabricksAllowedReplLang(_enableTableAccessControl, clusterMode);

            if (string.IsNullOrEmpty(allowedReplLang))
            {
                SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }
            else
            {
                SparkConfiguration["spark.databricks.repl.allowedLanguages"] = allowedReplLang;
            }

            return this;
        }

        public ClusterAttributes WithAutoTermination(int? autoTerminationMinutes)
        {
            AutoTerminationMinutes = autoTerminationMinutes.GetValueOrDefault();
            return this;
        }

        public ClusterAttributes WithRuntimeVersion(string runtimeVersion)
        {
            RuntimeVersion = runtimeVersion;
            return this;
        }

        /// <summary>
        /// This enables Photon engine on AWS Graviton-enabled clusters.
        /// For Azure Databricks, this setting has no effect. Specify Photon-specific runtimes instead.
        /// </summary>
        /// <see href="https://docs.databricks.com/clusters/graviton.html#databricks-rest-api" />
        public ClusterAttributes WithRuntimeEngine(RuntimeEngine engine)
        {
            RuntimeEngine = engine;
            return this;
        }

        public ClusterAttributes WithNodeType(string workerNodeType, string driverNodeType = null)
        {
            NodeTypeId = workerNodeType;
            DriverNodeTypeId = driverNodeType;
            return this;
        }

        public ClusterAttributes WithClusterLogConf(string dbfsDestination)
        {
            ClusterLogConfiguration =
                new ClusterLogConf { Dbfs = new DbfsStorageInfo { Destination = dbfsDestination } };
            return this;
        }

        public ClusterAttributes WithInstancePool(string instancePoolId, string driverInstancePoolId)
        {
            InstancePoolId = instancePoolId;
            DriverInstancePoolId = driverInstancePoolId;
            return this;
        }

        public ClusterAttributes WithInstancePool(string instancePoolId)
        {
            return WithInstancePool(instancePoolId, instancePoolId);
        }

        public ClusterAttributes WithPolicyId(string policyId, bool applyPolicyDefaultValues = true)
        {
            PolicyId = policyId;
            ApplyPolicyDefaultValues = applyPolicyDefaultValues;
            return this;
        }

        public ClusterAttributes WithDockerImage(string url, (string, string)? basicAuth = default)
        {
            DockerImage = basicAuth == null
                ? new DockerImage {Url = url}
                : new DockerImage
                {
                    Url = url,
                    BasicAuth = new DockerBasicAuth {UserName = basicAuth.Value.Item1, Password = basicAuth.Value.Item2}
                };

            return this;
        }
    }
}

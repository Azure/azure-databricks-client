using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClusterAttributes
    {
        /// <summary>
        /// Cluster name requested by the user. This doesn’t have to be unique. If not specified at creation, the cluster name will be an empty string.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_name")]
        public string ClusterName { get; set; }

        /// <summary>
        /// The Spark version of the cluster. A list of available Spark versions can be retrieved by using the Spark Versions API call.
        /// </summary>
        [JsonProperty(PropertyName = "spark_version")]
        public string RuntimeVersion { get; set; }

        /// <summary>
        /// An object containing a set of optional, user-specified Spark configuration key-value pairs. Users can also pass in a string of extra JVM options to the driver and the executors via spark.driver.extraJavaOptions and spark.executor.extraJavaOptions respectively.
        ///Example Spark confs: {"spark.speculation": true, "spark.streaming.ui.retainedBatches": 5} or {"spark.driver.extraJavaOptions": "-verbose:gc -XX:+PrintGCDetails"}
        /// </summary>
        [JsonProperty(PropertyName = "spark_conf")]
        public Dictionary<string, string> SparkConfiguration { get; set; }

        /// <summary>
        /// This field encodes, through a single value, the resources available to each of the Spark nodes in this cluster. For example, the Spark nodes can be provisioned and optimized for memory or compute intensive workloads A list of available node types can be retrieved by using the List Node Types API call. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "node_type_id")]
        public string NodeTypeId { get; set; }

        /// <summary>
        /// The node type of the Spark driver. Note that this field is optional; if unset, the driver node type will be set as the same value as node_type_id defined above.
        /// </summary>
        [JsonProperty(PropertyName = "driver_node_type_id")]
        public string DriverNodeTypeId { get; set; }

        /// <summary>
        /// SSH public key contents that will be added to each Spark node in this cluster. The corresponding private keys can be used to login with the user name ubuntu on port 2200. Up to 10 keys can be specified.
        /// </summary>
        [JsonProperty(PropertyName = "ssh_public_keys")]
        public IEnumerable<string> SshPublicKeys { get; set; }

        /// <summary>
        /// Additional tags for cluster resources. Databricks will tag all cluster resources (e.g., VMs disk volumes) with these tags in addition to default_tags.
        /// Azure Databricks allows at most 8 custom tags.
        /// </summary>
        [JsonProperty(PropertyName = "custom_tags")]
        public Dictionary<string, string> CustomTags { get; set; }

        /// <summary>
        /// The configuration for delivering spark logs to a long-term storage destination. Only one destination can be specified for one cluster. If the conf is given, the logs will be delivered to the destination every 5 mins. The destination of driver logs is $destination/$clusterId/driver, while the destination of executor logs is $destination/$clusterId/executor.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_log_conf")]
        public ClusterLogConf ClusterLogConfiguration { get; set; }

        /// <summary>
        /// The configuration for storing init scripts. 
        /// </summary>
        /// <remarks>
        /// Any number of destinations can be specified. The scripts are executed sequentially in the order provided.
        /// If cluster_log_conf is specified, init script logs are sent to &lt;destination&gt;/&lt;cluster-id&gt;/init_scripts.
        /// </remarks>
        [JsonProperty(PropertyName = "init_scripts")]
        public IEnumerable<InitScriptInfo> InitScripts { get; set; }

        /// <summary>
        /// An object containing a set of optional, user-specified environment variable key-value pairs. Please note that key-value pair of the form (X,Y) will be exported as is (i.e., export X='Y') while launching the driver and workers.
        ///In order to specify an additional set of SPARK_DAEMON_JAVA_OPTS, we recommend appending them to $SPARK_DAEMON_JAVA_OPTS as shown in the example below.This ensures that all default databricks managed environmental variables are included as well.
        ///Example Spark environment variables: {"SPARK_WORKER_MEMORY": "28000m", "SPARK_LOCAL_DIRS": "/local_disk0"}
        ///or {"SPARK_DAEMON_JAVA_OPTS": "$SPARK_DAEMON_JAVA_OPTS -Dspark.shuffle.service.enabled=true"}
        /// </summary>
        [JsonProperty(PropertyName = "spark_env_vars")]
        public Dictionary<string, string> SparkEnvironmentVariables { get; set; }

        /// <summary>
        /// Automatically terminates the cluster after it is inactive for this time in minutes. If not set, this cluster will not be automatically terminated. If specified, the threshold must be between 10 and 10000 minutes. Users can also set this value to 0 to explicitly disable automatic termination.
        /// </summary>
        [JsonProperty(PropertyName = "autotermination_minutes")]
        public int AutoTerminationMinutes { get; set; }

        /// <summary>
        /// Autoscaling Local Storage: when enabled, this cluster will dynamically acquire additional disk space when its Spark workers are running low on disk space.
        /// </summary>
        [JsonProperty(PropertyName = "enable_elastic_disk")]
        public bool EnableElasticDisk { get; set; }

        /// <summary>
        /// Determines whether the cluster was created by a user through the UI, by the Databricks Jobs Scheduler, or through an API request.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClusterSource? ClusterSource { get; set; }

        /// <summary>
        /// The optional ID of the instance pool to which the cluster belongs. Refer to Pools for details.
        /// </summary>
        [JsonProperty(PropertyName = "instance_pool_id")]
        public string InstancePoolId { get; set; }

        /// <summary>
        /// Docker image for a custom container.
        /// </summary>
        [JsonProperty(PropertyName = "docker_image")]
        public DockerImage DockerImage { get; set; }

        /// <summary>
        /// When enabled, local disk data will be encrypted at-rest. Contact your Microsoft or Databricks account representative to enable for your subscription.
        /// </summary>
        [JsonProperty(PropertyName = "enable_local_disk_encryption")]
        public bool LocalDiskEncryption { get; set; }

        /// <summary>
        /// A cluster policy ID.
        /// </summary>
        [JsonProperty(PropertyName = "policy_id")]
        public string PolicyId { get; set; }

        /// <summary>
        /// Whether to use policy default values for missing cluster attributes. Default value: false.
        /// </summary>
        [JsonProperty(PropertyName = "apply_policy_default_values")]
        public bool ApplyPolicyDefaultValues { get; set; }

        /// <summary>
        /// Defines attributes such as the instance availability type, node placement, and max bid price. If not specified during cluster creation, a set of default values is used.
        /// </summary>
        [JsonProperty(PropertyName = "azure_attributes")]
        public AzureAttributes AzureAttributes { get; set; }
    }
}

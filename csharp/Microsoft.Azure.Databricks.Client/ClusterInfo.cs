using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <inheritdoc />
    /// <summary>
    /// Describes all of the metadata about a single Spark cluster in Databricks.
    /// </summary>
    /// <seealso cref="T:Microsoft.Azure.Databricks.Client.ClusterInstance" />
    public class ClusterInfo : ClusterInstance
    {
        public static ClusterInfo GetNewClusterConfiguration(string clusterName = null)
        {
            return new ClusterInfo
            {
                ClusterName = clusterName
            };
        }

        /// <summary>
        /// If num_workers, number of worker nodes that this cluster should have. A cluster has one Spark Driver and num_workers Executors for a total of num_workers + 1 Spark nodes.
        /// If autoscale, parameters needed in order to automatically scale clusters up and down based on load.Note: autoscaling works best with DB runtime versions 3.0 or later.
        /// </summary>
        /// <remarks>
        /// Note: When reading the properties of a cluster, this field reflects the desired number of workers rather than the actual current number of workers.
        /// For instance, if a cluster is resized from 5 to 10 workers, this field will immediately be updated to reflect the target size of 10 workers, whereas the workers listed in spark_info will gradually increase from 5 to 10 as the new nodes are provisioned.
        /// </remarks>
        [JsonProperty(PropertyName = "num_workers")]
        public int? NumberOfWorkers { get; set; }

        /// <summary>
        /// If num_workers, number of worker nodes that this cluster should have. A cluster has one Spark Driver and num_workers Executors for a total of num_workers + 1 Spark nodes.
        /// If autoscale, parameters needed in order to automatically scale clusters up and down based on load.Note: autoscaling works best with DB runtime versions 3.0 or later.
        /// </summary>
        /// <remarks>
        /// Note: When reading the properties of a cluster, this field reflects the desired number of workers rather than the actual current number of workers.
        /// For instance, if a cluster is resized from 5 to 10 workers, this field will immediately be updated to reflect the target size of 10 workers, whereas the workers listed in spark_info will gradually increase from 5 to 10 as the new nodes are provisioned.
        /// </remarks>
        [JsonProperty(PropertyName = "autoscale")]
        public AutoScale AutoScale { get; set; }

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
        ///Currently Databricks allows up to 9 custom tags.
        /// </summary>
        [JsonProperty(PropertyName = "custom_tags")]
        public Dictionary<string, string> CustomTags { get; set; }

        /// <summary>
        /// The configuration for delivering spark logs to a long-term storage destination. Only one destination can be specified for one cluster. If the conf is given, the logs will be delivered to the destination every 5 mins. The destination of driver logs is $destination/$clusterId/driver, while the destination of executor logs is $destination/$clusterId/executor.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_log_conf")]
        public ClusterLogConf ClusterLogConfiguration { get; set; }

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
        /// Determines whether the cluster was created by a user through the UI, by the Databricks Jobs Scheduler, or through an API request.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_source")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClusterSource? ClusterSource { get; set; }

        /// <summary>
        /// Current state of the cluster.
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClusterState? State { get; set; }

        /// <summary>
        /// A message associated with the most recent state transition (e.g., the reason why the cluster entered a TERMINATED state).
        /// </summary>
        [JsonProperty(PropertyName = "state_message")]
        public string StateMessage { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster creation request was received (when the cluster entered a PENDING state).
        /// </summary>
        [JsonProperty(PropertyName = "start_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? StartTime { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster was terminated, if applicable.
        /// </summary>
        [JsonProperty(PropertyName = "terminated_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? TerminatedTime { get; set; }

        /// <summary>
        /// Time when the cluster driver last lost its state (due to a restart or driver failure).
        /// </summary>
        [JsonProperty(PropertyName = "last_state_loss_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? LastStateLossTime { get; set; }

        /// <summary>
        /// Time (in epoch milliseconds) when the cluster was last active. A cluster is active if there is at least one command that has not finished on the cluster. This field is available after the cluster has reached a RUNNING state. Updates to this field are made as best-effort attempts. Certain versions of Spark do not support reporting of cluster activity. Refer to Automatic termination for details.
        /// </summary>
        [JsonProperty(PropertyName = "last_activity_time")]
        [JsonConverter(typeof(MillisecondEpochDateTimeConverter))]
        public DateTimeOffset? LastActivityTime { get; set; }

        /// <summary>
        /// Total amount of cluster memory, in megabytes
        /// </summary>
        [JsonProperty(PropertyName = "cluster_memory_mb")]
        public long ClusterMemoryMb { get; set; }

        /// <summary>
        /// Number of CPU cores available for this cluster. Note that this can be fractional, e.g. 7.5 cores, since certain node types are configured to share cores between Spark nodes on the same instance.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_cores")]
        public float ClusterCores { get; set; }

        /// <summary>
        /// Tags that are added by Databricks regardless of any custom_tags, including:
        ///     Vendor: Databricks
        ///     Creator: username_of_creator
        ///     ClusterName: name_of_cluster
        ///     ClusterId: id_of_cluster
        ///     Name: Databricks internal use
        /// </summary>
        [JsonProperty(PropertyName = "default_tags")]
        public Dictionary<string, string> DefaultTags { get; set; }

        /// <summary>
        /// Cluster log delivery status.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_log_status")]
        public LogSyncStatus ClusterLogSyncStatus { get; set; }

        /// <summary>
        /// Information about why the cluster was terminated. This field only appears when the cluster is in a TERMINATING or TERMINATED state.
        /// </summary>
        [JsonProperty(PropertyName = "termination_reason")]
        public TerminationReason TerminationReason { get; set; }

        [JsonProperty(PropertyName = "pinned_by_user_name")]
        public string PinnedByUserName { get; set; }

        /// <summary>
        /// Specifies whether the cluster supports Python version 3.
        /// </summary>
        public ClusterInfo WithPython3(bool enablePython3)
        {
            if (this.SparkEnvironmentVariables == null)
            {
                this.SparkEnvironmentVariables = new Dictionary<string, string>();
            }

            if (enablePython3)
            {
                this.SparkEnvironmentVariables["PYSPARK_PYTHON"] = "/databricks/python3/bin/python3";
            }
            else
            {
                this.SparkEnvironmentVariables.Remove("PYSPARK_PYTHON");
            }

            return this;
        }

        /// <summary>
        /// When enabled:
        ///     Allows users to run SQL, Python, and PySpark commands. Users are restricted to the SparkSQL API and DataFrame API, and therefore cannot use Scala, R, RDD APIs, or clients that directly read the data from cloud storage, such as DBUtils.
        ///     Cannot acquire direct access to data in the cloud via DBFS or by reading credentials from the cloud provider’s metadata service.
        ///     Requires that clusters run Databricks Runtime 3.5 or above.
        ///     Must run their commands on cluster nodes as a low-privilege user forbidden from accessing sensitive parts of the filesystem or creating network connections to ports other than 80 and 443.
        /// </summary>
        public ClusterInfo WithTableAccessControl(bool enableTableAccessControl)
        {
            if (this.SparkConfiguration == null)
            {
                this.SparkConfiguration = new Dictionary<string, string>();
            }

            if (enableTableAccessControl)
            {
                this.SparkConfiguration["spark.databricks.acl.dfAclsEnabled"] = "true";
                this.SparkConfiguration["spark.databricks.repl.allowedLanguages"] = "python,sql";
            }
            else
            {
                this.SparkConfiguration.Remove("spark.databricks.acl.dfAclsEnabled");
                this.SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }

            return this;
        }

        public ClusterInfo WithAutoScale(int minWorkers, int maxWorkers)
        {
            this.AutoScale = new AutoScale(minWorkers, maxWorkers);
            this.NumberOfWorkers = null;
            return this;
        }

        public ClusterInfo WithNumberOfWorkers(int numWorkers)
        {
            this.NumberOfWorkers = numWorkers;
            this.AutoScale = null;
            return this;
        }

        public ClusterInfo WithAutoTermination(int? autoTerminationMinutes)
        {
            this.AutoTerminationMinutes = autoTerminationMinutes.GetValueOrDefault();
            return this;
        }

        public ClusterInfo WithRuntimeVersion(string runtimeVersion)
        {
            this.RuntimeVersion = runtimeVersion;
            return this;
        }

        public ClusterInfo WithNodeType(string workerNodeType, string driverNodeType = null)
        {
            this.NodeTypeId = workerNodeType;
            this.DriverNodeTypeId = driverNodeType;
            return this;
        }

        public ClusterInfo WithClusterLogConf(string dbfsDestination)
        {
            this.ClusterLogConfiguration =
                new ClusterLogConf {Dbfs = new DbfsStorageInfo {Destination = dbfsDestination}};
            return this;
        }
    }
}
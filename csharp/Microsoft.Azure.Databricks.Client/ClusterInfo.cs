using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
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

    /// <summary>
    /// Describes all of the metadata about a single Spark cluster in Databricks.
    /// </summary>
    /// <seealso cref="T:Microsoft.Azure.Databricks.DatabricksClient.ClusterInstance" />
    public class ClusterInfo : ClusterAttributes
    {
        public ClusterInfo()
        {
            // Use python3 by default.
            this.SparkEnvironmentVariables = new Dictionary<string, string>
            {
                {"PYSPARK_PYTHON", "/databricks/python3/bin/python3"}
            };
        }

        public static ClusterInfo GetNewClusterConfiguration(string clusterName = null)
        {
            return new ClusterInfo
            {
                ClusterName = clusterName
            };
        }

        /// <summary>
        /// The canonical identifier for the cluster used by a run. This field is always available for runs on existing clusters. For runs on new clusters, it becomes available once the cluster is created. This value can be used to view logs by browsing to /#setting/sparkui/$cluster_id/driver-logs. The logs will continue to be available after the run completes.
        /// If this identifier is not yet available, the response won’t include this field.
        /// </summary>
        [JsonProperty(PropertyName = "cluster_id")]
        public string ClusterId { get; set; }

        /// <summary>
        /// The canonical identifier for the Spark context used by a run. This field will be filled in once the run begins execution. This value can be used to view the Spark UI by browsing to /#setting/sparkui/$cluster_id/$spark_context_id. The Spark UI will continue to be available after the run has completed.
        /// If this identifier is not yet available, the response won’t include this field.
        /// </summary>
        [JsonProperty(PropertyName = "spark_context_id")]
        public string SparkContextId { get; set; }

        /// <summary>
        /// Number of worker nodes that this cluster should have. A cluster has one Spark Driver and num_workers Executors for a total of num_workers + 1 Spark nodes.
        /// </summary>
        /// <remarks>
        /// Note: When reading the properties of a cluster, this field reflects the desired number of workers rather than the actual current number of workers.
        /// For instance, if a cluster is resized from 5 to 10 workers, this field will immediately be updated to reflect the target size of 10 workers, whereas the workers listed in spark_info will gradually increase from 5 to 10 as the new nodes are provisioned.
        /// </remarks>
        [JsonProperty(PropertyName = "num_workers")]
        public int? NumberOfWorkers { get; set; }

        /// <summary>
        /// Parameters needed in order to automatically scale clusters up and down based on load.Note: autoscaling works best with DB runtime versions 3.0 or later.
        /// </summary>
        [JsonProperty(PropertyName = "autoscale")]
        public AutoScale AutoScale { get; set; }

        /// <summary>
        /// Creator user name. The field won’t be included in the response if the user has already been deleted.
        /// </summary>
        [JsonProperty(PropertyName = "creator_user_name")]
        public string CreatorUserName { get; set; }

        /// <summary>
        /// Node on which the Spark driver resides. The driver node contains the Spark master and the Databricks application that manages the per-notebook Spark REPLs.
        /// </summary>
        [JsonProperty(PropertyName = "driver")]
        public SparkNode Driver { get; set; }

        /// <summary>
        /// Nodes on which the Spark executors reside.
        /// </summary>
        [JsonProperty(PropertyName = "executors")]
        public IEnumerable<SparkNode> Executors { get; set; }

        /// <summary>
        /// Port on which Spark JDBC server is listening, in the driver nod. No service will be listening on on this port in executor nodes.
        /// </summary>
        [JsonProperty(PropertyName = "jdbc_port")]
        public int JdbcPort { get; set; }

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

        /// <summary>
        /// Specifies whether the cluster uses Python version 2.
        /// Python 2 reached its end of life on January 1, 2020. Python 2 is not supported in Databricks Runtime 6.0 and above. Databricks Runtime 5.5 and below continue to support Python 2.
        /// </summary>
        public ClusterInfo WithPython2()
        {
            SparkEnvironmentVariables?.Remove("PYSPARK_PYTHON");
            return this;
        }

        /// <summary>
        /// Specifies whether the cluster supports Python version 3.
        /// </summary>
        [Obsolete("Python3 is enabled by default. If you want to use Python2, call \"WithPython2\".")]
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
        private bool _enableTableAccessControl;
        public ClusterInfo WithTableAccessControl(bool enableTableAccessControl)
        {
            _enableTableAccessControl = enableTableAccessControl;

            if (this.SparkConfiguration == null)
            {
                this.SparkConfiguration = new Dictionary<string, string>();
            }

            if (enableTableAccessControl)
            {
                this.SparkConfiguration["spark.databricks.acl.dfAclsEnabled"] = "true";
            }
            else
            {
                this.SparkConfiguration.Remove("spark.databricks.acl.dfAclsEnabled");
            }

            var allowedReplLang = DatabricksAllowedReplLang(enableTableAccessControl, _clusterMode);

            if (string.IsNullOrEmpty(allowedReplLang))
            {
                this.SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }
            else
            {
                this.SparkConfiguration["spark.databricks.repl.allowedLanguages"] = allowedReplLang;
            }

            return this;
        }

        private ClusterMode _clusterMode = ClusterMode.Standard;

        public ClusterInfo WithClusterMode(ClusterMode clusterMode)
        {
            this._clusterMode = clusterMode;

            if (this.CustomTags == null)
            {
                this.CustomTags = new Dictionary<string, string>();
            }

            if (this.SparkConfiguration == null)
            {
                this.SparkConfiguration = new Dictionary<string, string>();
            }

            switch (clusterMode)
            {
                case ClusterMode.HighConcurrency:
                    this.CustomTags["ResourceClass"] = "Serverless";
                    this.SparkConfiguration["spark.databricks.cluster.profile"] = "serverless";
                    this.SparkConfiguration.Remove("spark.master");
                    break;
                case ClusterMode.SingleNode:
                    this.CustomTags["ResourceClass"] = "SingleNode";
                    this.SparkConfiguration["spark.databricks.cluster.profile"] = "singleNode";
                    this.SparkConfiguration["spark.master"] = "local[*]";
                    this.NumberOfWorkers = 0;
                    break;
                default: // Standard mode
                    this.CustomTags.Remove("ResourceClass");
                    this.SparkConfiguration.Remove("spark.databricks.cluster.profile");
                    this.SparkConfiguration.Remove("spark.master");
                    break;
            }

            var allowedReplLang = DatabricksAllowedReplLang(_enableTableAccessControl, clusterMode);

            if (string.IsNullOrEmpty(allowedReplLang))
            {
                this.SparkConfiguration.Remove("spark.databricks.repl.allowedLanguages");
            }
            else
            {
                this.SparkConfiguration["spark.databricks.repl.allowedLanguages"] = allowedReplLang;
            }

            return this;
        }

        private static string DatabricksAllowedReplLang(bool enableTableAccessControl, ClusterMode clusterMode) => 
            enableTableAccessControl ? "python,sql" : (clusterMode == ClusterMode.HighConcurrency ? "sql,python,r" : null);

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
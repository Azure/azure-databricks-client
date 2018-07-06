using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunSettings<T> : ClusterSpec where T:RunSettings<T>
    {
        public T WithExistingCluster(string clusterId)
        {
            this.ExistingClusterId = clusterId;
            this.NewCluster = null;
            return (T)this;
        }

        public T WithNewCluster(ClusterInfo newClusterConfig)
        {
            this.NewCluster = newClusterConfig;
            this.ExistingClusterId = null;
            return (T)this;
        }

        /// <summary>
        /// indicates that this job should run a notebook. This field may not be specified in conjunction with spark_jar_task.
        /// </summary>
        [JsonProperty(PropertyName = "notebook_task", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NotebookTask NotebookTask { get; set; }

        /// <summary>
        /// indicates that this job should run a jar.
        /// </summary>
        [JsonProperty(PropertyName = "spark_jar_task", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SparkJarTask SparkJarTask { get; set; }

        /// <summary>
        /// indicates that this job should run a python file.
        /// </summary>
        [JsonProperty(PropertyName = "spark_python_task", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SparkPythonTask SparkPythonTask { get; set; }

        /// <summary>
        /// indicates that this job should run spark submit script.
        /// </summary>
        [JsonProperty(PropertyName = "spark_submit_task", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SparkSubmitTask SparkSubmitTask { get; set; }

        /// <summary>
        /// An optional timeout applied to each run of this job. The default behavior is to have no timeout.
        /// </summary>
        [JsonProperty(PropertyName = "timeout_seconds", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TimeoutSeconds { get; set; }
    }
}
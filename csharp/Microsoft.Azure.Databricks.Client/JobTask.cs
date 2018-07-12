using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class JobTask
    {
        /// <summary>
        /// indicates that this job should run a notebook. This field may not be specified in conjunction with spark_jar_task.
        /// </summary>
        [JsonProperty(PropertyName = "notebook_task")]
        public NotebookTask NotebookTask { get; set; }

        /// <summary>
        /// indicates that this job should run a jar.
        /// </summary>
        [JsonProperty(PropertyName = "spark_jar_task")]
        public SparkJarTask SparkJarTask { get; set; }

        /// <summary>
        /// indicates that this job should run a python file.
        /// </summary>
        [JsonProperty(PropertyName = "spark_python_task")]
        public SparkPythonTask SparkPythonTask { get; set; }

        /// <summary>
        /// indicates that this job should run spark submit script.
        /// </summary>
        [JsonProperty(PropertyName = "spark_submit_task")]
        public SparkSubmitTask SparkSubmitTask { get; set; }
    }
}
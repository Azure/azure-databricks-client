using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record JobTask
    {
        /// <summary>
        /// indicates that this job should run a notebook. This field may not be specified in conjunction with spark_jar_task.
        /// </summary>
        [JsonPropertyName("notebook_task")]
        public NotebookTask NotebookTask { get; set; }

        /// <summary>
        /// indicates that this job should run a jar.
        /// </summary>
        [JsonPropertyName("spark_jar_task")]
        public SparkJarTask SparkJarTask { get; set; }

        /// <summary>
        /// indicates that this job should run a python file.
        /// </summary>
        [JsonPropertyName("spark_python_task")]
        public SparkPythonTask SparkPythonTask { get; set; }

        /// <summary>
        /// indicates that this job should run spark submit script.
        /// </summary>
        [JsonPropertyName("spark_submit_task")]
        public SparkSubmitTask SparkSubmitTask { get; set; }
    }
}
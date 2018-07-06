using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class NotebookTask
    {
        /// <summary>
        /// The absolute path of the notebook to be run in the Databricks Workspace. This path must begin with a slash. Relative paths will be supported in the future. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "notebook_path")]
        public string NotebookPath { get; set; }

        /// <summary>
        /// Base parameters to be used for each run of this job. If the run is initiated by a call to run-now with parameters specified, the two parameters maps will be merged. If the same key is specified in base_parameters and in run-now, the value from run-now will be used.
        /// If the notebook takes a parameter that is not specified in the job’s base_parameters or the run-now override parameters, the default value from the notebook will be used.
        /// These parameters can be retrieved in a notebook by using dbutils.widgets.get().
        /// </summary>
        [JsonProperty(PropertyName = "base_parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, string> BaseParameters { get; set; }
    }

    public class SparkJarTask
    {
        /// <summary>
        /// The full name of the class containing the main method to be executed. This class must be contained in a JAR provided as a library.
        /// Note that the code should use SparkContext.getOrCreate to obtain a Spark context; otherwise, runs of the job will fail.
        /// </summary>
        [JsonProperty(PropertyName = "main_class_name")]
        public string MainClassName { get; set; }

        /// <summary>
        /// Parameters that will be passed to the main method.
        /// </summary>
        [JsonProperty(PropertyName = "parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Parameters { get; set; }
    }

    public class SparkPythonTask
    {
        /// <summary>
        /// The URI of the Python file to be executed. DBFS and S3 paths are supported. This field is required.
        /// </summary>
        [JsonProperty(PropertyName = "python_file")]
        public string PythonFile { get; set; }

        /// <summary>
        /// Command line parameters that will be passed to the Python file.
        /// </summary>
        [JsonProperty(PropertyName = "parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Parameters { get; set; }
    }

    /// <remarks>
    /// Here are some important things to know.
    ///     Spark submit tasks can only be run on new clusters.
    ///     master, deploy-mode, and executor-cores are automatically configured by Databricks; you cannot specify them in parameters.
    ///     By default, the Spark submit job would use all available memory(excluding reserved memory for Databricks services). You can also set --driver-memory, and --executor-memory to a smaller value to leave some room for off-heap usage.
    ///     libraries and spark_conf in the new_cluster specification are not supported. Use --jars and --pyFiles to add Java and Python libraries and use --conf to set spark conf.S3 and DBFS paths are supported in --jars, --pyFiles, --files arguments.
    ///     Requires Spark 2.1.1-db5 (for example, 2.1.1-db5-scala2.10) or above.
    /// </remarks>
    /// <example>
    /// For example, you can run SparkPi by setting the following parameters, assuming the JAR is already uploaded to DBFS.
    /// <c>
    ///     {
    ///         "parameters": [
    ///         "--class",
    ///         "org.apache.spark.examples.SparkPi",
    ///         "dbfs:/path/to/examples.jar",
    ///         "10"
    ///         ]
    ///     }
    /// </c>
    /// </example>
    public class SparkSubmitTask
    {
        /// <summary>
        /// Command line parameters that will be passed to spark submit.
        /// </summary>
        [JsonProperty(PropertyName = "parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string> Parameters { get; set; }
    }
}

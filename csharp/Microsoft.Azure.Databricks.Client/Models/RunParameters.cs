using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// Parameters for this run. Only one of jar_params, python_params or notebook_params should be specified in the run-now request, depending on the type of job task. Jobs with jar task or python task take a list of position-based parameters, and jobs with notebook tasks take a key value map.
/// </summary>
public record RunParameters
{
    public static RunParameters CreateJarParams(IEnumerable<string> jarParams)
    {
        return new RunParameters {JarParams = jarParams.ToList()};
    }

    public static RunParameters CreateNotebookParams(Dictionary<string, string> notebookParams)
    {
        return new RunParameters {NotebookParams = notebookParams};
    }

    public static RunParameters CreatePythonParams(IEnumerable<string> pythonParams)
    {
        return new RunParameters {PythonParams = pythonParams.ToList()};
    }

    public static RunParameters CreateSparkSubmitParams(IEnumerable<string> sparkSubmitParams)
    {
        return new RunParameters {SparkSubmitParams = sparkSubmitParams.ToList()};
    }

    public static RunParameters CreatePythonNamedParams(Dictionary<string, string> pythonNamedParams)
    {
        return new RunParameters {PythonNamedParams = pythonNamedParams};
    }

    /// <summary>
    /// A list of parameters for jobs with jar tasks, e.g. "jar_params": ["john doe", "35"]. The parameters will be used to invoke the main function of the main class specified in the spark jar task. If not specified upon run-now, it will default to an empty list. jar_params cannot be specified in conjunction with notebook_params. The json representation of this field (i.e. {"jar_params":["john doe","35"]}) cannot exceed 10,000 bytes.
    /// </summary>
    [JsonPropertyName("jar_params")]
    public List<string> JarParams { get; set; }

    /// <summary>
    /// A map from keys to values for jobs with notebook task, e.g. "notebook_params": {"name": "john doe", "age":  "35"}. The map is passed to the notebook and will be accessible through the dbutils.widgets.get function. See Widgets for more information.
    /// If not specified upon run-now, the triggered run will use the job’s base parameters.
    /// notebook_params cannot be specified in conjunction with jar_params.
    /// The json representation of this field (i.e. {"notebook_params":{"name":"john doe","age":"35"}}) cannot exceed 10,000 bytes.
    /// </summary>
    [JsonPropertyName("notebook_params")]
    public Dictionary<string, string> NotebookParams { get; set; }

    /// <summary>
    /// A list of parameters for jobs with python tasks, e.g. "python_params": ["john doe", "35"]. The parameters will be passed to python file as command line parameters. If specified upon run-now, it would overwrite the parameters specified in job setting. The json representation of this field (i.e. {"python_params":["john doe","35"]}) cannot exceed 10,000 bytes.
    /// </summary>
    [JsonPropertyName("python_params")]
    public List<string> PythonParams { get; set; }

    /// <summary>
    /// A list of parameters for jobs with spark submit task, e.g. "spark_submit_params": ["--class", "org.apache.spark.examples.SparkPi"]. The parameters will be passed to spark-submit script as command line parameters. If specified upon run-now, it would overwrite the parameters specified in job setting. The json representation of this field cannot exceed 10,000 bytes.
    /// </summary>
    [JsonPropertyName("spark_submit_params")]
    public List<string> SparkSubmitParams { get; set; }

    /// <summary>
    /// A map from keys to values for jobs with Python wheel task, e.g. `"python_named_params": {"name": "task", "data": "dbfs:/path/to/data.json"}`.
    /// </summary>
    [JsonPropertyName("python_named_params")]
    public Dictionary<string, string> PythonNamedParams { get; set; }
}
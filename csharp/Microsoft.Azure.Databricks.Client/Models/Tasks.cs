// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

#region NotebookTask

public record NotebookTask
{
    /// <summary>
    /// The absolute path of the notebook to be run in the Databricks Workspace. This path must begin with a slash. Relative paths will be supported in the future. This field is required.
    /// </summary>
    [JsonPropertyName("notebook_path")]
    public string NotebookPath { get; set; }

    /// <summary>
    /// Base parameters to be used for each run of this job. If the run is initiated by a call to run-now with parameters specified, the two parameters maps will be merged. If the same key is specified in base_parameters and in run-now, the value from run-now will be used.
    /// If the notebook takes a parameter that is not specified in the job’s base_parameters or the run-now override parameters, the default value from the notebook will be used.
    /// These parameters can be retrieved in a notebook by using dbutils.widgets.get().
    /// </summary>
    [JsonPropertyName("base_parameters")]
    public Dictionary<string, string> BaseParameters { get; set; }
}

#endregion

#region SparkJarTask

public record SparkJarTask
{
    /// <summary>
    /// The full name of the class containing the main method to be executed. This class must be contained in a JAR provided as a library.
    /// Note that the code should use SparkContext.getOrCreate to obtain a Spark context; otherwise, runs of the job will fail.
    /// </summary>
    [JsonPropertyName("main_class_name")]
    public string MainClassName { get; set; }

    /// <summary>
    /// Parameters that will be passed to the main method.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<string> Parameters { get; set; }
}

#endregion

#region SparkPythonTask

public record SparkPythonTask
{
    /// <summary>
    /// The URI of the Python file to be executed. DBFS and S3 paths are supported. This field is required.
    /// </summary>
    [JsonPropertyName("python_file")]
    public string PythonFile { get; set; }

    /// <summary>
    /// Command line parameters that will be passed to the Python file.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<string> Parameters { get; set; }
}

#endregion

#region SparkSubmitTask

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
public record SparkSubmitTask
{
    /// <summary>
    /// Command line parameters that will be passed to spark submit.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<string> Parameters { get; set; }
}

#endregion

#region PipelineTask

public record PipelineTask
{
    /// <summary>
    /// The full name of the pipeline task to execute.
    /// </summary>
    [JsonPropertyName("pipeline_id")]
    public string PipelineId { get; set; }

    /// <summary>
    /// If true, a full refresh will be triggered on the delta live table.
    /// </summary>
    [JsonPropertyName("full_refresh")]
    public bool FullRefresh { get; set; }
}

#endregion

#region PythonWheelTask

public record PythonWheelTask
{
    /// <summary>
    /// Name of the package to execute
    /// </summary>
    [JsonPropertyName("package_name")]
    public string PackageName { get; set; }

    /// <summary>
    /// Named entry point to use, if it does not exist in the metadata of the package it executes the function from the package directly using `$packageName.$entryPoint()`
    /// </summary>
    [JsonPropertyName("entry_point")]
    public string EntryPoint { get; set; }

    /// <summary>
    /// Command-line parameters passed to Python wheel task. Leave it empty if `named_parameters` is not null.
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<string> Parameters { get; set; }

    /// <summary>
    /// Command-line parameters passed to Python wheel task in the form of `["--name=task", "--data=dbfs:/path/to/data.json"]`. Leave it empty if `parameters` is not null.
    /// </summary>
    [JsonPropertyName("named_parameters")]
    public Dictionary<string, string> NamedParameters { get; set; }
}

#endregion

#region SQLTask

public record SQLTask
{
    /// <summary>
    /// If specified, indicates that this job must execute a SQL query.
    /// </summary>
    [JsonPropertyName("query")]
    public SQLQuery Query { get; set; }

    /// <summary>
    /// If specified, indicates that this job must refresh a SQL dashboard.
    /// </summary>
    [JsonPropertyName("dashboard")]
    public SQLDashboard Dashboard { get; set; }

    /// <summary>
    /// If specified, indicates that this job must refresh a SQL alert.
    /// </summary>
    [JsonPropertyName("alert")]
    public SQLAlert Alert { get; set; }

    /// <summary>
    /// If specified, indicates that this job runs a SQL file in a remote Git repository. Only one SQL statement is supported in a file. Multiple SQL statements separated by semicolons (;) are not permitted.
    /// </summary>
    [JsonPropertyName("file")]
    public SQLFile File { get; set; }

    /// <summary>
    /// Parameters to be used for each run of this job. The SQL alert task does not support custom parameters.
    /// </summary>
    [JsonPropertyName("parameters")]
    public Dictionary<string, string> Parameters { get; set; }

    /// <summary>
    /// The canonical identifier of the SQL warehouse. Only serverless and pro SQL warehouses are supported.
    /// </summary>
    [JsonPropertyName("warehouse_id")]
    public string WarehouseId { get; set; }
}

public record SQLQuery
{
    /// <summary>
    /// The canonical identifier of the SQL query.
    /// </summary>
    [JsonPropertyName("query_id")]
    public string QueryId { get; set; }
}

public record SQLDashboard
{
    /// <summary>
    /// The canonical identifier of the SQL dashboard.
    /// </summary>
    [JsonPropertyName("dashboard_id")]
    public string DashboardId { get; set; }

    /// <summary>
    /// If specified, dashboard snapshots are sent to subscriptions.
    /// </summary>
    [JsonPropertyName("subscriptions")]
    public List<SQLSubscription> Subscriptions { get; set; }

    /// <summary>
    /// Subject of the email sent to subscribers of this task.
    /// </summary>
    [JsonPropertyName("custom_subject")]
    public string CustomSubject { get; set; }

    /// <summary>
    /// If true, the dashboard snapshot is not taken, and emails are not sent to subscribers.
    /// </summary>
    [JsonPropertyName("pause_subscriptions")]
    public string PauseSubscription { get; set; }
}

public record SQLSubscription
{
    /// <summary>
    /// The user name to receive the subscription email. This parameter is mutually exclusive with destination_id. You cannot set both destination_id and user_name for subscription notifications.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    /// <summary>
    /// The canonical identifier of the destination to receive email notification. This parameter is mutually exclusive with user_name. You cannot set both destination_id and user_name for subscription notifications.
    /// </summary>
    [JsonPropertyName("destination_id")]
    public string DestinationId { get; set; }
}

public record SQLAlert
{
    /// <summary>
    /// The canonical identifier of the SQL alert.
    /// </summary>
    [JsonPropertyName("alert_id")]
    public string AlertId { get; set; }

    /// <summary>
    /// If specified, alert notifications are sent to subscribers.
    /// </summary>
    [JsonPropertyName("subscriptions")]
    public List<SQLSubscription> Subscriptions { get; set; }

    /// <summary>
    /// If true, the alert notifications are not sent to subscribers.
    /// </summary>
    [JsonPropertyName("pause_subscriptions")]
    public string PauseSubscription { get; set; }
}

public record SQLFile
{
    /// <summary>
    /// Relative path of the SQL file in the remote Git repository.
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; set; }
}

#endregion

#region DBTTask

public record DBTTask
{
    /// <summary>
    /// Optional (relative) path to the project directory, if no value is provided, the root of the git repository is used.
    /// </summary>
    [JsonPropertyName("project_directory")]
    public string ProjectDirectory { get; set; }

    /// <summary>
    /// A list of dbt commands to execute. All commands must start with dbt. This parameter must not be empty. A maximum of up to 10 commands can be provided.
    /// </summary>
    [JsonPropertyName("commands")]
    public List<string> Commands { get; set; }

    /// <summary>
    /// Optional schema to write to. This parameter is only used when a warehouse_id is also provided. If not provided, the default schema is used.
    /// </summary>
    [JsonPropertyName("schema")]
    public string Schema { get; set; }

    /// <summary>
    /// ID of the SQL warehouse to connect to. If provided, we automatically generate and provide the profile and connection details to dbt. It can be overridden on a per-command basis by using the --profiles-dir command line argument.
    /// </summary>
    [JsonPropertyName("warehouse_id")]
    public string WarehouseId { get; set; }

    /// <summary>
    /// Optional name of the catalog to use. The value is the top level in the 3-level namespace of Unity Catalog (catalog / schema / relation). The catalog value can only be specified if a warehouse_id is specified. Requires dbt-databricks >= 1.1.1.
    /// </summary>
    [JsonPropertyName("catalog")]
    public string Catalog { get; set; }

    /// <summary>
    /// Optional (relative) path to the profiles directory. Can only be specified if no warehouse_id is specified. If no warehouse_id is specified and this folder is unset, the root directory is used.
    /// </summary>
    [JsonPropertyName("profiles_directory")]
    public string ProfilesDirectory { get; set; }
}

#endregion

#region RunJobTask

public record RunJobTask
{
    /// <summary>
    /// ID of the job to trigger.
    /// </summary>
    [JsonPropertyName("job_id")]
    public long JobId { get; set; }
}

#endregion
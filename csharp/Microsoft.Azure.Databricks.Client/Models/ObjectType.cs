// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The type of the object in workspace.
/// </summary>
public enum ObjectType
{
    /// <summary>
    /// Notebook
    /// </summary>
    NOTEBOOK,

    /// <summary>
    /// Directory
    /// </summary>
    DIRECTORY,

    /// <summary>
    /// Library
    /// </summary>
    LIBRARY,

    /// <summary>
    /// File
    /// </summary>
    FILE,

    /// <summary>
    /// MLflow Experiment
    /// </summary>
    MLFLOW_EXPERIMENT,

    /// <summary>
    /// Git Repository
    /// </summary>
    REPO,

    /// <summary>
    /// Lakeview Dashboard
    /// </summary>
    DASHBOARD

}

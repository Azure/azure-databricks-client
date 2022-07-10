// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client.Models
{
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
        /// MLflow Experiment
        /// </summary>
        MLFLOW_EXPERIMENT
    }
}

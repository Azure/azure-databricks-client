// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client.Models
{
    public enum ExportFormat
    {
        /// <summary>
        /// The notebook will be imported/exported as source code.
        /// </summary>
        SOURCE,

        /// <summary>
        /// The notebook will be imported/exported as an HTML file.
        /// </summary>
        HTML,

        /// <summary>
        /// The notebook will be imported/exported as a Jupyter/IPython Notebook file.
        /// </summary>
        JUPYTER,

        /// <summary>
        /// The notebook will be imported/exported as Databricks archive format.
        /// </summary>
        DBC
    }
}
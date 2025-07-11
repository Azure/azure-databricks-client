﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

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
    DBC,

    /// <summary>
    /// The notebook is imported/exported to R Markdown format.
    /// </summary>
    R_MARKDOWN,

    /// <summary>
    /// The object is exported depending on the objects type.
    /// </summary>
    AUTO
}
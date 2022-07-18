// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The language of notebook.
/// </summary>
public enum Language
{
    /// <summary>
    /// Scala notebook.
    /// </summary>
    SCALA,

    /// <summary>
    /// Python notebook.
    /// </summary>
    PYTHON,

    /// <summary>
    /// SQL notebook.
    /// </summary>
    SQL,

    /// <summary>
    /// R notebook.
    /// </summary>
    R
}
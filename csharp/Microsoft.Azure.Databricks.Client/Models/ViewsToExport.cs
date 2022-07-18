// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// View to export: either code, all dashboards, or all.
/// </summary>
public enum ViewsToExport
{
    /// <summary>
    /// Code view of the notebook
    /// </summary>
    CODE,

    /// <summary>
    /// All dashboard views of the notebook
    /// </summary>
    DASHBOARDS,

    /// <summary>
    /// All views of the notebook
    /// </summary>
    ALL
}
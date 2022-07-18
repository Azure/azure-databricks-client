// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

public enum ResizeCause
{
    /// <summary>
    /// Automatically resized based on load.
    /// </summary>
    AUTOSCALE,

    /// <summary>
    /// User requested a new size.
    /// </summary>
    USER_REQUEST,

    /// <summary>
    /// Autorecovery monitor resized the cluster after it lost a node.
    /// </summary>
    AUTORECOVERY
}
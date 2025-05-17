// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The type of runtime engine.
/// </summary>
public enum RuntimeEngine
{
    /// <summary>
    /// No runtime engine.
    /// </summary>
    NULL,

    /// <summary>
    /// The Photon runtime engine.
    /// </summary>
    PHOTON,

    /// <summary>
    /// The standard runtime engine.
    /// </summary>
    STANDARD
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// Indicates the service that created the cluster.
/// </summary>
public enum ClusterSource
{
    /// <summary>
    /// Cluster created through the UI.
    /// </summary>
    UI,

    /// <summary>
    /// Cluster created by the Databricks Job Scheduler.
    /// </summary>
    JOB,

    /// <summary>
    /// Cluster created through an API call.
    /// </summary>
    API,

    SQL,

    MODELS,

    /// <summary>
    /// Cluster created through Pipeline.
    /// </summary>
    PIPELINE,

    PIPELINE_MAINTENANCE
}
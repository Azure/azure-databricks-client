﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// A value indicating the run's current lifecycle state. This field is always available in the response.
/// </summary>
public enum RunLifeCycleState
{
    /// <summary>
    /// The run has been triggered. If there is not already an active run of the same job, the cluster and execution context are being prepared. If there is already an active run of the same job, the run will immediately transition into a SKIPPED state without preparing any resources.
    /// </summary>
    PENDING,

    /// <summary>
    /// The task of this run is currently being executed.
    /// </summary>
    RUNNING,

    /// <summary>
    /// The task of this run has completed, and the cluster and execution context are being cleaned up.
    /// </summary>
    TERMINATING,

    /// <summary>
    /// The task of this run has completed, and the cluster and execution context have been cleaned up. This state is terminal.
    /// </summary>
    TERMINATED,

    /// <summary>
    /// This run was aborted because a previous run of the same job was already active. This state is terminal.
    /// </summary>
    SKIPPED,

    /// <summary>
    /// An exceptional state that indicates a failure in the Jobs service, such as network failure over a long period. If a run on a new cluster ends in an INTERNAL_ERROR state, the Jobs service will terminate the cluster as soon as possible. This state is terminal.
    /// </summary>
    INTERNAL_ERROR,

    BLOCKED,

    WAITING_FOR_RETRY,

    QUEUED
}
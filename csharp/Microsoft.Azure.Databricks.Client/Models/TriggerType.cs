﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// These are the type of triggers that can fire a run.
/// </summary>
public enum TriggerType
{
    /// <summary>
    /// These are schedules that periodically trigger runs, such as a cron scheduler.
    /// </summary>
    PERIODIC,

    /// <summary>
    /// These are one time triggers that only fire a single run. This means the user triggered a single run on demand through the UI or the API.
    /// </summary>
    ONE_TIME,

    /// <summary>
    /// This indicates a run that is triggered as a retry of a previously failed run. This occurs when the user requests to re-run the job in case of failures.
    /// </summary>
    RETRY,

    /// <summary>
    /// Indicates a run that is triggered using a Run Job task.
    /// </summary>
    RUN_JOB_TASK,

    /// <summary>
    /// Indicates a run that is triggered by a file arrival.
    /// </summary>
    FILE_ARRIVAL,

    /// <summary>
    /// Indicates a run that is triggered by a table update.
    /// </summary>
    TABLE,

    /// <summary>
    /// When you run your job with the continuous trigger, Databricks Jobs ensures there is always one active run of the job.
    /// </summary>
    CONTINUOUS
}
using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public interface IPipelinesApi : IDisposable
{
    /// <summary>
    /// Lists pipelines defined in the Delta Live Tables system.
    /// </summary>
    Task<PipelinesList> List(int maxResults = 25, string pageToken = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new data processing pipeline based on the requested configuration. 
    /// If successful, this method returns the ID of the new pipeline. 
    /// Also returns PipelineSpecification if dry_run is true.
    /// </summary>
    Task<(string, PipelineSpecification)> Create(
        PipelineSpecification pipelineSpecification,
        bool dryRun = false,
        bool allowDuplicateNames = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a pipeline with the supplied configuration.
    /// </summary>  
    Task Edit(
        string pipelineId,
        PipelineSpecification pipelineSpecification,
        bool allowDuplicateNames = false,
        DateTimeOffset? expectedLastModified = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a pipeline.
    /// </summary>
    Task Delete(string pipelineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets a pipeline.
    /// </summary>
    Task Reset(string pipelineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops a pipeline.
    /// </summary>
    Task Stop(string pipelineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves information about a single pipeline.
    /// </summary>
    Task<Pipeline> Get(string pipelineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an update from an active pipeline.
    /// </summary>
    Task<PipelineUpdate> GetUpdate(string pipelineId, string updateId, CancellationToken cancellation = default);

    /// <summary>
    /// List updates for an active pipeline.
    /// </summary>
    Task<PipelineUpdatesList> ListUpdates(
        string pipelineId,
        int maxResults = 25,
        string pageToken = null,
        string untilUpdateId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts or queues a pipeline update.
    /// </summary>
    Task<string> Start(
        string pipelineId,
        bool fullRefresh = false,
        PipelineUpdateCause cause = PipelineUpdateCause.API_CALL,
        IEnumerable<string> refreshSelection = default,
        IEnumerable<string> fullRefreshSelection = default,
        CancellationToken cancellation = default);

    /// <summary>
    /// Retrieves events for a pipeline.
    /// </summary>
    Task<PipelineEventsList> ListEvents(
        string pipelineId,
        int maxResults = 25,
        string orderBy = null,
        string filter = null,
        string pageToken = null,
        CancellationToken cancellationToken = default);
}

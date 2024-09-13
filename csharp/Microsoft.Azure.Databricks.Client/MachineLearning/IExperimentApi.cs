using Microsoft.Azure.Databricks.Client.Models.MachineLearning.Experiment;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.MachineLearning;

public interface IExperimentApi : IDisposable
{
    /// <summary>
    /// Gets the metadata, metrics, params, and tags for a run. In the case where multiple metrics with the same key are logged for a run, return only the value with the latest timestamp.
    /// If there are multiple values with the latest timestamp, return the maximum of these values.
    /// </summary>
    /// <param name="run_id">ID of the run to fetch. Must be provided.</param>
    Task<Run> GetRun(string run_id, CancellationToken cancellationToken = default);
}

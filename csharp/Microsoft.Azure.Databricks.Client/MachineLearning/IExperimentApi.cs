using Microsoft.Azure.Databricks.Client.Models.MachineLearning.Experiment;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.MachineLearning;

public interface IExperimentApi : IDisposable
{
    Task<Run> GetRun(string run_id, CancellationToken cancellationToken = default);
}

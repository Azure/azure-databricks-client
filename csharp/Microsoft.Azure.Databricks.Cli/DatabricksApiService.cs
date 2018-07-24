using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public class DatabricksApiService
    {
        private readonly DatabricksClient _client;

        public DatabricksApiService(CommandLineApplication app)
        {
            var options = app.GetOptions().ToArray();
            var clusterUrlOption = options.Single(o => o.LongName == "cluster-base-url");
            var accessTokenOption = options.Single(o => o.LongName == "access-token");

            var baseUrl = clusterUrlOption.HasValue() ? clusterUrlOption.Value() : null;
            if (baseUrl == null)
            {
                ConsoleLogger.WriteLineError("Must specify --cluster-base-url.");
                throw new ApplicationException("cluster base url not specified");
            }

            var accessToken = accessTokenOption.HasValue() ? accessTokenOption.Value() : null;
            if (accessToken == null)
            {
                ConsoleLogger.WriteLineError("Must specify --access-token.");
                throw new ApplicationException("access token not specified");
            }

            this._client = DatabricksClient.CreateClient(baseUrl, accessToken);
        }

        public async Task<long> CreateJob(JobSettings jobSettings)
        {
            ConsoleLogger.WriteLineVerbose($"[{DateTime.UtcNow:s}] Submitting job to cluster");

            try
            {
                var jobId = await _client.Jobs.Create(jobSettings);
                ConsoleLogger.WriteLineVerbose($"[{DateTime.UtcNow:s}] Job Id - {jobId}");
                return jobId;
            }
            catch (ClientApiException apiException)
            {
                ConsoleLogger.WriteLineError($"[{DateTime.UtcNow:s}] Job failed to submit - {apiException.Message}");
                throw;
            }
        }

        public async Task<RunState> RunNow(long jobId, int pollIntervalSeconds = 10)
        {
            ConsoleLogger.WriteLineVerbose($"[{DateTime.UtcNow:s}] Starting job {jobId} now.");

            var runId = (await _client.Jobs.RunNow(jobId, null)).RunId;

            ConsoleLogger.WriteLineVerbose($"[{DateTime.UtcNow:s}] Run Id: {runId}");
            
            while (true)
            {
                var run = await _client.Jobs.RunsGet(runId);

                ConsoleLogger.WriteLineVerbose(
                    $"[{DateTime.UtcNow:s}] \tLifeCycleState: {run.State.LifeCycleState}\tStateMessage: {run.State.StateMessage}");

                if (run.State.LifeCycleState == RunLifeCycleState.PENDING ||
                    run.State.LifeCycleState == RunLifeCycleState.RUNNING ||
                    run.State.LifeCycleState == RunLifeCycleState.TERMINATING)
                {
                    await Task.Delay(TimeSpan.FromSeconds(pollIntervalSeconds));
                }
                else
                {
                    return run.State;
                }
            }
        }
    }
}
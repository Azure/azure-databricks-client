using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client;
using Microsoft.Extensions.CommandLineUtils;
using Polly;

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
            var requestTimeoutOption = options.Single(o => o.LongName == "request-timeout");

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

            var requestTimeout = requestTimeoutOption.HasValue() ? int.Parse(requestTimeoutOption.Value()) : 30;
            this._client = DatabricksClient.CreateClient(baseUrl, accessToken, requestTimeout);
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

            var retryPolicy = Policy.Handle<WebException>()
                .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.BadGateway)
                .Or<ClientApiException>(e => e.StatusCode == HttpStatusCode.InternalServerError)
                .Or<ClientApiException>(e => e.Message.Contains("\"error_code\":\"TEMPORARILY_UNAVAILABLE\""))
                .OrResult<RunState>(runState => runState.LifeCycleState == RunLifeCycleState.PENDING)
                .OrResult(runState => runState.LifeCycleState == RunLifeCycleState.RUNNING)
                .OrResult(runState => runState.LifeCycleState == RunLifeCycleState.TERMINATING)
                .WaitAndRetryForeverAsync(
                    retryAttempt => TimeSpan.FromSeconds(pollIntervalSeconds),
                    (delegateResult, timespan) =>
                    {
                        if (delegateResult.Exception != null)
                        {
                            ConsoleLogger.WriteLineError(
                                $"[{DateTime.UtcNow:s}] Failed to query run status - {delegateResult.Exception}");
                        }
                    });

            var result = await retryPolicy.ExecuteAsync(async () =>
            {
                var run = await _client.Jobs.RunsGet(runId);
                ConsoleLogger.WriteLineVerbose(
                    $"[{DateTime.UtcNow:s}] \tLifeCycleState: {run.State.LifeCycleState}\tStateMessage: {run.State.StateMessage}");

                return run.State;
            });

            return result;
        }

        public async Task DeleteJob(long jobId)
        {
            ConsoleLogger.WriteLineVerbose($"[{DateTime.UtcNow:s}] Deleting job {jobId}...");
            await _client.Jobs.Delete(jobId);
            ConsoleLogger.WriteLineVerbose($"[{DateTime.UtcNow:s}] Job {jobId} deleted.");
        }
    }
}
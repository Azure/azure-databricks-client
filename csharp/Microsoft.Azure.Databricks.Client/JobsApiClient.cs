using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class JobsApiClient : ApiClient, IJobsApi
    {
        public JobsApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<long> Create(JobSettings jobSettings, CancellationToken cancellationToken = default)
        {
            var jobIdentifier =
                await HttpPost<JobSettings, dynamic>(this.HttpClient, "jobs/create", jobSettings, cancellationToken)
                    .ConfigureAwait(false);
            return jobIdentifier.job_id.ToObject<long>();
        }

        public async Task<IEnumerable<Job>> List(CancellationToken cancellationToken = default)
        {
            const string requestUri = "jobs/list";
            var jobList = await HttpGet<dynamic>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
            return PropertyExists(jobList, "jobs")
                ? jobList.jobs.ToObject<IEnumerable<Job>>()
                : Enumerable.Empty<Job>();
        }

        public async Task Delete(long jobId, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "jobs/delete", new { job_id = jobId }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Job> Get(long jobId, CancellationToken cancellationToken = default)
        {
            var requestUri = $"jobs/get?job_id={jobId}";
            return await HttpGet<Job>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }

        public async Task Reset(long jobId, JobSettings newSettings, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, "jobs/reset", new { job_id = jobId, new_settings = newSettings }, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<RunIdentifier> RunNow(long jobId, RunParameters runParams, CancellationToken cancellationToken = default)
        {
            var settings = new RunNowSettings
            {
                JobId = jobId
            };

            if (runParams != null)
            {
                settings.SparkSubmitParams = runParams.SparkSubmitParams;
                settings.PythonParams = runParams.PythonParams;
                settings.NotebookParams = runParams.NotebookParams;
                settings.JarParams = runParams.JarParams;
            }

            return await HttpPost<RunNowSettings, RunIdentifier>(this.HttpClient, "jobs/run-now", settings, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<long> RunSubmit(RunOnceSettings settings, CancellationToken cancellationToken = default)
        {
            var result = await HttpPost<RunOnceSettings, RunIdentifier>(this.HttpClient, "jobs/runs/submit", settings, cancellationToken)
                .ConfigureAwait(false);
            return result.RunId;
        }

        public async Task<RunList> RunsList(long? jobId = null, int offset = 0, int limit = 20, bool activeOnly = false,
            bool completedOnly = false, /*RunType? runType = null, */ CancellationToken cancellationToken = default)
        {
            if (activeOnly && completedOnly)
            {
                throw new ArgumentException(
                    $"{nameof(activeOnly)} and {nameof(completedOnly)} cannot both be true.");
            }

            var url = $"jobs/runs/list?limit={limit}&offset={offset}";
            if (jobId.HasValue)
            {
                url += $"&job_id={jobId.Value}";
            }

            if (activeOnly)
            {
                url += "&active_only=true";
            }

            if (completedOnly)
            {
                url += "&completed_only=true";
            }

            // if (runType.HasValue)
            // {
            //     url += $"&run_type={runType.Value}";
            // }

            return await HttpGet<RunList>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Run> RunsGet(long runId, CancellationToken cancellationToken = default)
        {
            var url = $"jobs/runs/get?run_id={runId}";
            return await HttpGet<Run>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task RunsCancel(long runId, CancellationToken cancellationToken = default)
        {
            var request = new { run_id = runId };
            await HttpPost(this.HttpClient, "jobs/runs/cancel", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task RunsDelete(long runId, CancellationToken cancellationToken = default)
        {
            var request = new { run_id = runId };
            await HttpPost(this.HttpClient, "jobs/runs/delete", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ViewItem>> RunsExport(long runId,
            ViewsToExport viewsToExport = ViewsToExport.CODE, CancellationToken cancellationToken = default)
        {
            var url = $"jobs/runs/export?run_id={runId}&views_to_export={viewsToExport}";
            var viewItemList = await HttpGet<dynamic>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

            return PropertyExists(viewItemList, "views")
                ? viewItemList.views.ToObject<IEnumerable<ViewItem>>()
                : Enumerable.Empty<ViewItem>();
        }

        public async Task<(string, string, Run)> RunsGetOutput(long runId, CancellationToken cancellationToken = default)
        {
            var url = $"jobs/runs/get-output?run_id={runId}";
            var response = await HttpGet<dynamic>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            Run run = response.metadata.ToObject<Run>();

            string error = PropertyExists(response, "error") ? response.error.ToObject<string>() : null;
            string notebookOutput = PropertyExists(response, "notebook_output") && PropertyExists(response.notebook_output, "result")
                ? response.notebook_output.result.ToObject<string>()
                : null;
            return (notebookOutput, error, run);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class JobsApiClient : ApiClient, IJobsApi
    {
        public JobsApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<long> Create(JobSettings jobSettings)
        {
            var jobIdentifier =
                await HttpPost<JobSettings, dynamic>(this.HttpClient, "jobs/create", jobSettings)
                    .ConfigureAwait(false);
            return jobIdentifier.job_id.ToObject<long>();
        }

        public async Task<IEnumerable<Job>> List()
        {
            const string requestUri = "jobs/list";
            var jobList = await HttpGet<dynamic>(this.HttpClient, requestUri).ConfigureAwait(false);
            return PropertyExists(jobList, "jobs")
                ? jobList.jobs.ToObject<IEnumerable<Job>>()
                : Enumerable.Empty<Job>();
        }

        public async Task Delete(long jobId)
        {
            await HttpPost(this.HttpClient, "jobs/delete", new { job_id = jobId }).ConfigureAwait(false);
        }

        public async Task<Job> Get(long jobId)
        {
            var requestUri = $"jobs/get?job_id={jobId}";
            return await HttpGet<Job>(this.HttpClient, requestUri).ConfigureAwait(false);
        }

        public async Task Reset(long jobId, JobSettings newSettings)
        {
            await HttpPost(this.HttpClient, "jobs/reset", new { job_id = jobId, new_settings = newSettings })
                .ConfigureAwait(false);
        }

        public async Task<RunIdentifier> RunNow(long jobId, RunParameters runParams)
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

            return await HttpPost<RunNowSettings, RunIdentifier>(this.HttpClient, "jobs/run-now", settings)
                .ConfigureAwait(false);
        }

        public async Task<long> RunSubmit(RunOnceSettings settings)
        {
            var result = await HttpPost<RunOnceSettings, RunIdentifier>(this.HttpClient, "jobs/runs/submit", settings)
                .ConfigureAwait(false);
            return result.RunId;
        }

        public async Task<RunList> RunsList(long? jobId = null, int offset = 0, int limit = 20, bool activeOnly = false,
            bool completedOnly = false)
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

            return await HttpGet<RunList>(this.HttpClient, url).ConfigureAwait(false);
        }

        public async Task<Run> RunsGet(long runId)
        {
            var url = $"jobs/runs/get?run_id={runId}";
            return await HttpGet<Run>(this.HttpClient, url).ConfigureAwait(false);
        }

        public async Task RunsCancel(long runId)
        {
            var request = new { run_id = runId };
            await HttpPost(this.HttpClient, "jobs/runs/cancel", request).ConfigureAwait(false);
        }

        public async Task RunsDelete(long runId)
        {
            var request = new { run_id = runId };
            await HttpPost(this.HttpClient, "jobs/runs/delete", request).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ViewItem>> RunsExport(long runId,
            ViewsToExport viewsToExport = ViewsToExport.CODE)
        {
            var url = $"jobs/runs/export?run_id={runId}&views_to_export={viewsToExport}";
            var viewItemList = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);

            return PropertyExists(viewItemList, "views")
                ? viewItemList.views.ToObject<IEnumerable<ViewItem>>()
                : Enumerable.Empty<ViewItem>();
        }

        public async Task<(string, string, Run)> RunsGetOutput(long runId)
        {
            var url = $"jobs/runs/get-output?run_id={runId}";
            var response = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);
            Run run = response.metadata.ToObject<Run>();

            string error = PropertyExists(response, "error") ? response.error.ToObject<string>() : null;
            string notebookOutput = PropertyExists(response, "notebook_output") && PropertyExists(response.notebook_output, "result")
                ? response.notebook_output.result.ToObject<string>()
                : null;
            return (notebookOutput, error, run);
        }
    }
}

using System;
using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;
public class JobsApiClient : ApiClient, IJobsApi
{
    protected override string ApiVersion => "2.1";

    public JobsApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<long> Create(JobSettings jobSettings,
        IEnumerable<AccessControlRequest> accessControlList = default,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(jobSettings, Options)!.AsObject();
            
        accessControlList.Iter(
            acr => request.Add("access_control_list", JsonSerializer.SerializeToNode(acr, Options))
        );

        var jobIdentifier =
            await HttpPost<JsonObject, JsonObject>(this.HttpClient, $"{ApiVersion}/jobs/create", request,
                    cancellationToken)
                .ConfigureAwait(false);
        return jobIdentifier["job_id"]!.GetValue<long>();
    }

    public async Task<JobList> List(int limit = 20, int offset = 0, bool expandTasks = false,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/jobs/list";
        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken)
            .ConfigureAwait(false);

        response.TryGetPropertyValue("jobs", out var jobsNode);
        var jobs = jobsNode
            .Map(node => node.Deserialize<IEnumerable<Job>>(Options))
            .GetOrElse(Enumerable.Empty<Job>);

        response.TryGetPropertyValue("has_more", out var hasMoreNode);
        var hasMore = hasMoreNode.Exists(node => node.GetValue<bool>());
            
        return new JobList {Jobs = jobs, HasMore = hasMore};
    }

    public async Task Delete(long jobId, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/jobs/delete", new { job_id = jobId }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<Job> Get(long jobId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/jobs/get?job_id={jobId}";
        return await HttpGet<Job>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task Reset(long jobId, JobSettings newSettings, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/jobs/reset", new { job_id = jobId, new_settings = newSettings }, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task Update(long jobId, JobSettings newSettings, string[] fieldsToRemove = default,
        CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{ApiVersion}/jobs/update",
                new { job_id = jobId, new_settings = newSettings, fields_to_remove = fieldsToRemove },
                cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<long> RunNow(long jobId, RunParameters runParams = default, string idempotencyToken = default,
        CancellationToken cancellationToken = default)
    {
        var request = runParams.Map(p => JsonSerializer.SerializeToNode(p, Options)!.AsObject())
            .GetOrElse(() => new JsonObject());
        request.Add("job_id", jobId);
        idempotencyToken.Iter(token => request.Add("idempotency_token", token));

        var result = await HttpPost<JsonObject, RunIdentifier>(
            this.HttpClient, $"{ApiVersion}/jobs/run-now", request, cancellationToken
        ).ConfigureAwait(false);

        return result.RunId;
    }

    public async Task<long> RunSubmit(RunSubmitSettings settings,
        IEnumerable<AccessControlRequest> accessControlList = default, string idempotencyToken = default,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(settings, Options)!.AsObject();
        idempotencyToken.Iter(token => request.Add("idempotency_token", token));
        accessControlList.Iter(
            acr => request.Add("access_control_list", JsonSerializer.SerializeToNode(acr, Options))
        );

        var result = await HttpPost<JsonObject, RunIdentifier>(
            this.HttpClient, $"{ApiVersion}/jobs/runs/submit", request, cancellationToken
        ).ConfigureAwait(false);

        return result.RunId;
    }

    public async Task<RunList> RunsList(long? jobId = default, int offset = 0, int limit = 25,
        bool activeOnly = default, bool completedOnly = default,
        RunType? runType = default, bool expandTasks = default, DateTimeOffset? startTimeFrom = default,
        DateTimeOffset? startTimeTo = default,
        CancellationToken cancellationToken = default)
    {
        if (activeOnly && completedOnly)
        {
            throw new ArgumentException(
                $"{nameof(activeOnly)} and {nameof(completedOnly)} cannot both be true."
            );
        }

        static string EmptyStr() => string.Empty;

        var url = $"{ApiVersion}/jobs/runs/list?limit={limit}&offset={offset}";

        url += jobId.Map(id => $"&job_id={id}").GetOrElse(EmptyStr);
        url += activeOnly ? "&active_only=true" : EmptyStr();
        url += completedOnly ? "&completed_only=true" : EmptyStr();
        url += runType.Map(type => $"&run_type={type}").GetOrElse(EmptyStr);
        url += expandTasks ? "&expand_task=true" : EmptyStr();
        url += startTimeFrom.Map(time => $"&start_time_from={time.ToUnixTimeMilliseconds()}").GetOrElse(EmptyStr);
        url += startTimeTo.Map(time => $"&start_time_to={time.ToUnixTimeMilliseconds()}").GetOrElse(EmptyStr);
        return await HttpGet<RunList>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<(Run, RepairHistory)> RunsGet(long runId, bool includeHistory = default,
        CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/jobs/runs/get?run_id={runId}&include_history={JsonValue.Create(includeHistory)}";
        var response = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        return (response.Deserialize<Run>(Options), response.Deserialize<RepairHistory>(Options));
    }

    public async Task RunsCancel(long runId, CancellationToken cancellationToken = default)
    {
        var request = new { run_id = runId };
        await HttpPost(this.HttpClient, $"{ApiVersion}/jobs/runs/cancel", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task RunsDelete(long runId, CancellationToken cancellationToken = default)
    {
        var request = new { run_id = runId };
        await HttpPost(this.HttpClient, $"{ApiVersion}/jobs/runs/delete", request, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<ViewItem>> RunsExport(long runId,
        ViewsToExport viewsToExport = ViewsToExport.CODE, CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/jobs/runs/export?run_id={runId}&views_to_export={viewsToExport}";
        var viewItemList = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

        viewItemList.TryGetPropertyValue("views", out var views);
        return views.Map(v => v.Deserialize<IEnumerable<ViewItem>>(Options)).GetOrElse(Enumerable.Empty<ViewItem>);
    }

    public async Task<RunOutput> RunsGetOutput(long runId, CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/jobs/runs/get-output?run_id={runId}";
        return await HttpGet<RunOutput>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<long> RunsRepair(RepairRunInput repairRunInput, RunParameters repairRunParameters,
        CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/jobs/runs/repair";
        var inputNode = JsonSerializer.SerializeToNode(repairRunInput, Options)!.AsObject();
        var parametersNode = JsonSerializer.SerializeToNode(repairRunParameters, Options)!.AsObject();
        foreach (var kvp in parametersNode)
        {
            if (kvp.Value == null)
                continue;
            
            var node = kvp.Value!.ToJsonString(Options);
            inputNode.Add(kvp.Key, JsonNode.Parse(node));
        }

        var response = await HttpPost<JsonObject, JsonObject>(this.HttpClient, url, inputNode, cancellationToken)
            .ConfigureAwait(false);
        return response["repair_id"]!.GetValue<long>();
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
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
        IEnumerable<AclPermissionItem> accessControlList = default,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(jobSettings, Options)!.AsObject();

        if (accessControlList != null)
        {
            request.Add("access_control_list", JsonSerializer.SerializeToNode(accessControlList, Options));
        }

        var jobIdentifier =
            await HttpPost<JsonObject, JsonObject>(this.HttpClient, $"{ApiVersion}/jobs/create", request,
                    cancellationToken)
                .ConfigureAwait(false);
        return jobIdentifier["job_id"]!.GetValue<long>();
    }

    public async Task<JobList> List(int limit = 20, int offset = 0, string name = default, bool expandTasks = false,
        CancellationToken cancellationToken = default)
    {
        if (limit < 1 || limit > 25)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "limit must be between 1 and 25");
        }

        if (offset < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "offset must be greater than or equal to 0");
        }

        var requestUri = $"{ApiVersion}/jobs/list?limit={limit}&offset={offset}&expand_tasks={expandTasks.ToString().ToLowerInvariant()}";

        if (name is not null)
        {
            requestUri += $"&name={name}";
        }

        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken)
            .ConfigureAwait(false);

        response.TryGetPropertyValue("jobs", out var jobsNode);
        var jobs = jobsNode?.Deserialize<IEnumerable<Job>>(Options) ?? Enumerable.Empty<Job>();
        var hasMore = response.TryGetPropertyValue("has_more", out var hasMoreNode) && hasMoreNode!.GetValue<bool>();
        return new JobList { Jobs = jobs, HasMore = hasMore };
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
        var request = runParams == null
            ? new JsonObject()
            : JsonSerializer.SerializeToNode(runParams, Options)!.AsObject();

        request.Add("job_id", jobId);

        if (!string.IsNullOrEmpty(idempotencyToken))
        {
            request.Add("idempotency_token", idempotencyToken);
        }

        var result = await HttpPost<JsonObject, RunIdentifier>(
            this.HttpClient, $"{ApiVersion}/jobs/run-now", request, cancellationToken
        ).ConfigureAwait(false);

        return result.RunId;
    }

    public async Task<long> RunSubmit(RunSubmitSettings settings,
        IEnumerable<AclPermissionItem> accessControlList = default, string idempotencyToken = default,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(settings, Options)!.AsObject();
        if (!string.IsNullOrEmpty(idempotencyToken))
        {
            request.Add("idempotency_token", idempotencyToken);
        }

        if (accessControlList != null)
        {
            request.Add("access_control_list", JsonSerializer.SerializeToNode(accessControlList, Options));
        }

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

        var url = $"{ApiVersion}/jobs/runs/list?limit={limit}&offset={offset}";

        if (jobId.HasValue)
        {
            url += $"&job_id={jobId.Value}";
        }

        url += activeOnly ? "&active_only=true" : string.Empty;
        url += completedOnly ? "&completed_only=true" : string.Empty;

        if (runType.HasValue)
        {
            url += $"&run_type={runType.Value}";
        }

        url += expandTasks ? "&expand_task=true" : string.Empty;

        if (startTimeFrom.HasValue)
        {
            url += $"&start_time_from={startTimeFrom.Value.ToUnixTimeMilliseconds()}";
        }

        if (startTimeTo.HasValue)
        {
            url += $"&start_time_to={startTimeTo.Value.ToUnixTimeMilliseconds()}";
        }

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

        return viewItemList.TryGetPropertyValue("views", out var views)
            ? views!.Deserialize<IEnumerable<ViewItem>>(Options)
            : Enumerable.Empty<ViewItem>();
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
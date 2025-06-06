﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

    private string BuildJobsListUrl(int limit, string name, bool expandTasks)
    {
        StringBuilder url = new($"{ApiVersion}/jobs/list?limit={limit}");

        if (name is not null)
        {
            url.Append($"&name={name}");
        }

        url.Append($"&expand_tasks={expandTasks.ToString().ToLowerInvariant()}");
        return url.ToString();
    }

    [Obsolete("The offset parameter is deprecated. Use method with pageToken to iterate through the pages.")]
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

        var url = BuildJobsListUrl(limit, name, expandTasks);
        url += $"&offset={offset}";

        var response = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken)
            .ConfigureAwait(false);

        response.TryGetPropertyValue("jobs", out var jobsNode);
        var jobs = jobsNode?.Deserialize<IEnumerable<Job>>(Options) ?? Enumerable.Empty<Job>();
        var hasMore = response.TryGetPropertyValue("has_more", out var hasMoreNode) && hasMoreNode!.GetValue<bool>();
        return new JobList { Jobs = jobs, HasMore = hasMore };
    }

    public global::Azure.AsyncPageable<Job> ListPageable(int pageSize = 20, string name = null, bool expandTasks = false, CancellationToken cancellationToken = default)
    {
        if (pageSize < 1 || pageSize > 25)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize must be between 1 and 25");
        }

        return new AsyncPageable<Job>(async (nextPageToken) =>
        {
            var url = BuildJobsListUrl(pageSize, name, expandTasks);
            url += string.IsNullOrEmpty(nextPageToken) ? string.Empty : $"&page_token={nextPageToken}";

            var jobList = await HttpGet<JobList>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            return (jobList.Jobs.ToList(), jobList.HasMore, jobList.NextPageToken);
        });
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

    public async Task<long> RunNow(long jobId, RunParameters runParams = default, string idempotencyToken = default, QueueSettings queueSettings = default,
        CancellationToken cancellationToken = default)
    {
        var request = runParams == null
            ? new JsonObject()
            : JsonSerializer.SerializeToNode(runParams, Options)!.AsObject();

        request.Add("job_id", jobId);

        if (queueSettings != null)
        {
            request.Add("queue", JsonSerializer.SerializeToNode(queueSettings));
        }

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

    private string BuildRunsListUrl(long? jobId = default, int limit = 25, bool activeOnly = default, bool completedOnly = default,
        RunType? runType = default, bool expandTasks = default, DateTimeOffset? startTimeFrom = default, DateTimeOffset? startTimeTo = default)
    {
        if (activeOnly && completedOnly)
        {
            throw new ArgumentException(
                $"{nameof(activeOnly)} and {nameof(completedOnly)} cannot both be true."
            );
        }

        StringBuilder url = new($"{ApiVersion}/jobs/runs/list?limit={limit}");

        if (jobId.HasValue)
        {
            url.Append($"&job_id={jobId.Value}");
        }

        url.Append(activeOnly ? "&active_only=true" : string.Empty);
        url.Append(completedOnly ? "&completed_only=true" : string.Empty);

        if (runType.HasValue)
        {
            url.Append($"&run_type={runType.Value}");
        }

        url.Append(expandTasks ? "&expand_tasks=true" : string.Empty);

        if (startTimeFrom.HasValue)
        {
            url.Append($"&start_time_from={startTimeFrom.Value.ToUnixTimeMilliseconds()}");
        }

        if (startTimeTo.HasValue)
        {
            url.Append($"&start_time_to={startTimeTo.Value.ToUnixTimeMilliseconds()}");
        }

        return url.ToString();
    }

    [Obsolete("The offset parameter is deprecated. Use method with pageToken to iterate through the pages.")]
    public async Task<RunList> RunsList(long? jobId = default, int offset = 0, int limit = 25,
        bool activeOnly = default, bool completedOnly = default, RunType? runType = default, bool expandTasks = default,
        DateTimeOffset? startTimeFrom = default, DateTimeOffset? startTimeTo = default, CancellationToken cancellationToken = default)
    {
        string url = BuildRunsListUrl(jobId, limit, activeOnly, completedOnly, runType, expandTasks, startTimeFrom, startTimeTo);
        url += $"&offset={offset}";
        return await HttpGet<RunList>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
    }

    public async Task<RunList> RunsList(string pageToken, long? jobId = default, int limit = 25,
        bool activeOnly = default, bool completedOnly = default, RunType? runType = default, bool expandTasks = default,
        DateTimeOffset? startTimeFrom = default, DateTimeOffset? startTimeTo = default, CancellationToken cancellationToken = default)
    {
        string url = BuildRunsListUrl(jobId, limit, activeOnly, completedOnly, runType, expandTasks, startTimeFrom, startTimeTo);
        url += string.IsNullOrEmpty(pageToken) ? string.Empty : $"&page_token={pageToken}";
        return await HttpGet<RunList>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
    }

    public global::Azure.AsyncPageable<Run> RunsListPageable(long? jobId = null, int pageSize = 25,
        bool activeOnly = false, bool completedOnly = false, RunType? runType = null, bool expandTasks = false,
        DateTimeOffset? startTimeFrom = null, DateTimeOffset? startTimeTo = null, CancellationToken cancellationToken = default)
    {
        return new AsyncPageable<Run>(async (nextPageToken) =>
        {
            var response = await RunsList(nextPageToken, jobId, pageSize, activeOnly, completedOnly, runType, expandTasks,
                startTimeFrom, startTimeTo, cancellationToken).ConfigureAwait(false);
            return (response.Runs.ToList(), response.HasMore, response.NextPageToken);
        });
    }

    public async Task<(Run, RepairHistory)> RunsGet(long runId, bool includeHistory = default, bool includeResolvedValues = default,
        CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/jobs/runs/get?run_id={runId}&include_history={JsonValue.Create(includeHistory)}&include_resolved_values={JsonValue.Create(includeResolvedValues)}";
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

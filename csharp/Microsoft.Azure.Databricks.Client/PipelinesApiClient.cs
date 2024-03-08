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

public class PipelinesApiClient : ApiClient, IPipelinesApi
{
    public PipelinesApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<PipelinesList> List(int maxResults = 25, string pageToken = default, CancellationToken cancellationToken = default)
    {
        if (maxResults < 1 || maxResults > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(maxResults), "limit must be between 1 and 100");
        }

        StringBuilder requestUriSb = new($"{ApiVersion}/pipelines?max_results={maxResults}");

        if (!string.IsNullOrEmpty(pageToken))
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var requestUri = requestUriSb.ToString();
        return await HttpGet<PipelinesList>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<(string, PipelineSpecification)> Create(
        PipelineSpecification pipelineSpecification,
        bool dryRun = true,
        bool allowDuplicateNames = false,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines";
        var request = JsonSerializer.SerializeToNode(pipelineSpecification, Options)!.AsObject();
        request.Add("dry_run", dryRun);
        request.Add("allow_duplicate_names", allowDuplicateNames);

        var response = await HttpPost<JsonObject, JsonObject>
                (this.HttpClient, requestUri, request, cancellationToken)
            .ConfigureAwait(false);

        return (
            response["pipeline_id"]?.GetValue<string>(),
            response["effective_settings"]?.AsObject().Deserialize<PipelineSpecification>(Options)
        );
    }

    public async Task Edit(
        string pipelineId,
        PipelineSpecification pipelineSpecification,
        bool allowDuplicateNames = false,
        DateTimeOffset? expectedLastModified = null,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}";
        var request = JsonSerializer.SerializeToNode(pipelineSpecification, Options)!.AsObject();
        request.Add("allow_duplicate_names", allowDuplicateNames);

        if (expectedLastModified != null)
        {
            request.Add("expected_last_modified", expectedLastModified);
        }

        await HttpPut(this.HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);
    }

    public async Task Delete(string pipelineId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}";
        await HttpDelete(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task Reset(string pipelineId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}/reset";
        await HttpPost(this.HttpClient, requestUri, new { }, cancellationToken);
    }

    public async Task Stop(string pipelineId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}/stop";
        await HttpPost(this.HttpClient, requestUri, new { }, cancellationToken);
    }

    public async Task<Pipeline> Get(string pipelineId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}";
        return await HttpGet<Pipeline>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<PipelineUpdate> GetUpdate(string pipelineId, string updateId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}/updates/{updateId}";
        var response = await HttpGet<Dictionary<string, PipelineUpdate>>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        return response["update"];
    }

    public async Task<PipelineUpdatesList> ListUpdates(
        string pipelineId,
        int maxResults = 25,
        string pageToken = null,
        string untilUpdateId = null,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder($"{ApiVersion}/pipelines/{pipelineId}/updates?max_results={maxResults}");

        if (pageToken != null)
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        if (untilUpdateId != null)
        {
            requestUriSb.Append($"&until_update_id={untilUpdateId}");
        }

        var requestUri = requestUriSb.ToString();
        return await HttpGet<PipelineUpdatesList>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> Start(
        string pipelineId,
        bool fullRefresh = false,
        PipelineUpdateCause cause = PipelineUpdateCause.API_CALL,
        IEnumerable<string> refreshSelection = default,
        IEnumerable<string> fullRefreshSelection = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = $"{ApiVersion}/pipelines/{pipelineId}/updates";
        var requestDict = new Dictionary<string, string>()
        {
            { "full_refresh", fullRefresh.ToString().ToLower() },
            { "cause", cause.ToString() }
        };

        var request = JsonSerializer.SerializeToNode(requestDict, Options).AsObject();

        if (refreshSelection != null)
        {
            var refreshSelectionJson = JsonSerializer.SerializeToNode(refreshSelection, Options);
            request.Add("refresh_selection", refreshSelectionJson);
        }

        if (fullRefreshSelection != null)
        {
            var fullRefreshSelectionJson = JsonSerializer.SerializeToNode(fullRefreshSelection, Options);
            request.Add("full_refresh_selection", fullRefreshSelectionJson);
        }

        var response = await HttpPost<JsonObject, JsonObject>(this.HttpClient, requestUri, request, cancellationToken).ConfigureAwait(false);

        return response["update_id"].GetValue<string>();
    }

    public async Task<PipelineEventsList> ListEvents(
        string pipelineId,
        int maxResults = 25,
        string orderBy = null,
        string filter = null,
        string pageToken = null,
        CancellationToken cancellationToken = default)
    {
        var requestUriSb = new StringBuilder($"{ApiVersion}/pipelines/{pipelineId}/events?max_results={maxResults}");

        if (orderBy != null)
        {
            requestUriSb.Append($"&order_by={orderBy}");
        }

        if (filter != null)
        {
            requestUriSb.Append($"&filter={filter}");
        }

        if (pageToken != null)
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var requestUri = requestUriSb.ToString();

        return await HttpGet<PipelineEventsList>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
    }
}

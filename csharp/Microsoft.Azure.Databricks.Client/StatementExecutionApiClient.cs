// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client;

public class StatementExecutionApiClient : ApiClient, IStatementExecutionApi
{
    private readonly string _apiBaseUrl;

    public StatementExecutionApiClient(HttpClient httpClient) : base(httpClient)
    {
        _apiBaseUrl = $"{ApiVersion}/sql/statements";
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    public async Task Cancel(string id, CancellationToken cancellationToken = default)
    {
        await HttpPost(this.HttpClient, $"{this._apiBaseUrl}/{id}/cancel", new { }, cancellationToken).ConfigureAwait(false);
    }

    public async Task<StatementExecution?> Execute(SqlStatement statement, CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.Serialize(statement, DatabricksSerializationContext.Default.SqlStatement);
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);

        return await SendRequest(
            this.HttpClient,
            HttpMethod.Post,
            this._apiBaseUrl,
            body,
            DatabricksSerializationContext.Default.StatementExecution,
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task<StatementExecutionResultChunk?> GetResultChunk(string id, int chunkIndex, CancellationToken cancellationToken = default)
    {
        return await SendRequest(
            this.HttpClient,
            HttpMethod.Get,
            $"{this._apiBaseUrl}/{id}/result/chunks/{chunkIndex}",
            null,
            DatabricksSerializationContext.Default.StatementExecutionResultChunk,
            cancellationToken
        ).ConfigureAwait(false);
    }

    public async Task<StatementExecution?> Get(string id, CancellationToken cancellationToken = default)
    {
        return await SendRequest(
            this.HttpClient,
            HttpMethod.Get,
            $"{this._apiBaseUrl}/{id}",
            null,
            DatabricksSerializationContext.Default.StatementExecution,
            cancellationToken
        ).ConfigureAwait(false);
    }
}

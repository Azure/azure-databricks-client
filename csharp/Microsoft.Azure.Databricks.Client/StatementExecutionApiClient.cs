// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class StatementExecutionApiClient : ApiClient, IStatementExecutionApi
    {
        private readonly string _apiBaseUrl;

        public StatementExecutionApiClient(HttpClient httpClient) : base(httpClient)
        {
            _apiBaseUrl = $"{ApiVersion}/sql/statements";
        }

        public async Task Cancel(string id, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, $"{this._apiBaseUrl}/{id}/cancel", new { }, cancellationToken).ConfigureAwait(false);
        }

        public async Task<StatementExecution> Execute(SqlStatement statement, CancellationToken cancellationToken = default)
        {
            var jsonObj = JsonSerializer.SerializeToNode(statement, Options)!.AsObject();

            var execution = await HttpPost<JsonObject, JsonObject>(
                this.HttpClient,
                this._apiBaseUrl,
                jsonObj,
                cancellationToken
            ).ConfigureAwait(false);

            return execution.Deserialize<StatementExecution>(Options);
        }

        public async Task<StatementExecutionResultChunk> GetResultChunk(string id, int chunkIndex, CancellationToken cancellationToken = default)
        {
            var execution = await HttpGet<JsonObject>(
                this.HttpClient,
                $"{this._apiBaseUrl}/{id}/result/chunks/{chunkIndex}",
                cancellationToken
            ).ConfigureAwait(false);

            return execution.Deserialize<StatementExecutionResultChunk>(Options);
        }

        public async Task<StatementExecution> Get(string id, CancellationToken cancellationToken = default)
        {
            var execution = await HttpGet<JsonObject>(
                this.HttpClient,
                $"{this._apiBaseUrl}/{id}",
                cancellationToken
            ).ConfigureAwait(false);

            return execution.Deserialize<StatementExecution>(Options);
        }
    }
}

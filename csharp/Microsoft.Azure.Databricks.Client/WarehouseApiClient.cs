// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class WarehouseApiClient : ApiClient, IWarehouseApi
    {
        private readonly string _apiBaseUrl;

        public WarehouseApiClient(HttpClient httpClient) : base(httpClient)
        {
            _apiBaseUrl = $"{ApiVersion}/sql/warehouses";
        }

        public async Task<string> Create(WarehouseAttributes warehouseAttributes, CancellationToken cancellationToken = default)
        {
            var jsonObj = JsonSerializer.SerializeToNode(warehouseAttributes, Options)!.AsObject();

            var warehouseIdentifier = await HttpPost<JsonObject, JsonObject>(
                this.HttpClient,
                this._apiBaseUrl,
                jsonObj,
                cancellationToken
            ).ConfigureAwait(false);

            return warehouseIdentifier["id"]!.GetValue<string>();
        }

        public async Task Delete(string id, CancellationToken cancellationToken = default)
        {
            var requestUri = $"{this._apiBaseUrl}/{id}";
            await HttpDelete(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<WarehouseInfo> Get(string id, CancellationToken cancellationToken = default)
        {
            var requestUri = $"{this._apiBaseUrl}/{id}";
            return await HttpGet<WarehouseInfo>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<WarehouseInfo>> List(int? runAsUserId = default, CancellationToken cancellationToken = default)
        {
            var requestUri = runAsUserId == null ? this._apiBaseUrl : $"{this._apiBaseUrl}?run_as_user_id={runAsUserId.Value}";
            var warehouseList = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
            warehouseList.TryGetPropertyValue("warehouses", out var warehousesNode);

            return warehousesNode?.Deserialize<IEnumerable<WarehouseInfo>>(Options) ?? Enumerable.Empty<WarehouseInfo>();
        }

        public async Task Start(string id, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, $"{this._apiBaseUrl}/{id}/start", new { }, cancellationToken).ConfigureAwait(false);
        }

        public async Task Stop(string id, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, $"{this._apiBaseUrl}/{id}/stop", new { }, cancellationToken).ConfigureAwait(false);
        }

        public async Task Update(string id, WarehouseAttributes warehouseAttributes, CancellationToken cancellationToken = default)
        {
            await HttpPost(this.HttpClient, $"{this._apiBaseUrl}/{id}/edit", warehouseAttributes, cancellationToken).ConfigureAwait(false);
        }
    }
}

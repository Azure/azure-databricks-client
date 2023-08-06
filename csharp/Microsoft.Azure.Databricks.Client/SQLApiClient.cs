// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http;

namespace Microsoft.Azure.Databricks.Client
{
    public class SQLApiClient : ApiClient, ISQLApi
    {
        public SQLApiClient(HttpClient httpClient) : base(httpClient)
        {
            this.DataWarehouseApi = new DataWarehouseApiClient(httpClient);
        }

        public IDataWarehouseApi DataWarehouseApi { get; }
    }
}

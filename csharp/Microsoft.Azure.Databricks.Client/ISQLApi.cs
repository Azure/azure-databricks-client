// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface ISQLApi : IDisposable
    {
        IStatementExecutionApi StatementExecution { get; }
        IWarehouseApi Warehouse { get; }
    }

    public interface IWarehouseApi : IDisposable
    {
        /// <summary>
        /// Creates a new SQL warehouse.
        /// </summary>
        Task<string> Create(WarehouseAttributes warehouseAttributes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all SQL warehouses that a user has manager permissions on.
        /// </summary>
        /// <param name="runAsUserId">Service Principal which will be used to fetch the list of warehouses.
        /// If not specified, the user from the session header is used.</param>
        Task<IEnumerable<WarehouseInfo>> List(int? runAsUserId = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the information for a single SQL warehouse.
        /// </summary>
        /// <param name="id">Required. Id of the SQL warehouse.</param>
        Task<WarehouseInfo> Get(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a SQL warehouse.
        /// </summary>
        /// <param name="id">Required. Id of the SQL warehouse.</param>
        Task Delete(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the configuration for a SQL warehouse.
        /// </summary>
        /// <param name="id">Required. Id of the warehouse to configure.</param>
        Task Update(string id, WarehouseAttributes warehouseAttributes, CancellationToken cancellationToken = default);

        /// <summary>
        /// Starts a SQL warehouse.
        /// </summary>
        /// <param name="id">Required. Id of the SQL warehouse.</param>
        Task Start(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops a SQL warehouse.
        /// </summary>
        /// <param name="id">Required. Id of the SQL warehouse.</param>
        Task Stop(string id, CancellationToken cancellationToken = default);
    }

    public interface IStatementExecutionApi : IDisposable
    {
        /// <summary>
        /// Execute a SQL statement.
        /// </summary>
        Task<StatementExecution> Execute(SqlStatement statement, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancel statement execution.
        /// </summary>
        /// <param name="id">Requried. Id of statement execution.</param>
        Task Cancel(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get status, manifest, and result first chunk.
        /// </summary>
        /// <param name="id">Requried. Id of statement execution.</param>
        Task<StatementExecution> Get(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get result chunk by index.
        /// </summary>
        /// <remarks>
        /// After the statement execution has SUCCEEDED, this request can be used to fetch any chunk by index. Whereas the first chunk with chunk_index=0 is typically fetched with statementexecution/executestatement or statementexecution/getstatement, this request can be used to fetch subsequent chunks. The response structure is identical to the nested result element described in the statementexecution/getstatement request, and similarly includes the next_chunk_index and next_chunk_internal_link fields for simple iteration through the result set.
        /// </remarks>
        /// <param name="id">Requried. Id of statement execution.</param>
        /// <param name="chunkIndex">Required. The index of the chunk.</param>
        Task<StatementExecutionResultChunk> GetResultChunk(string id, int chunkIndex, CancellationToken cancellationToken = default);
    }
}

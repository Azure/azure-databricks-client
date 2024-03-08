using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IConnectionsApi : IDisposable
{
    /// <summary>
    /// List all connections.
    /// </summary>
    Task<IEnumerable<Connection>> List(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a connection from it's name.
    /// </summary>
    Task<Connection> Get(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new connection to an external data source. 
    /// It allows users to specify connection details and configurations for interaction with the external server.
    /// </summary>
    Task<Connection> Create(
        ConnectionAttributes connectionAttributes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the connection that matches the supplied name.
    /// </summary>
    Task<Connection> Update(
        string connectionName,
        string name,
        Dictionary<string, string> options = default,
        string owner = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the connection that matches the supplied name.
    /// </summary>
    Task Delete(string connectionName, CancellationToken cancellationToken = default);
}

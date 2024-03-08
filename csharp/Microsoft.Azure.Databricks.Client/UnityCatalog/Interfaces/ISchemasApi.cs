using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ISchemasApi : IDisposable
{
    /// <summary>
    /// Gets an array of schemas for a catalog in the metastore. If the caller is the metastore admin or the owner of the parent catalog,
    /// all schemas for the catalog will be retrieved. Otherwise, only schemas owned by the caller
    /// (or for which the caller has the USE_SCHEMA privilege) will be retrieved. There is no guarantee of a specific ordering of the elements in the array.
    /// </summary>
    Task<IEnumerable<Schema>> List(
        string catalogName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new schema for catalog in the Metatastore. The caller must be a metastore admin, or have the CREATE_SCHEMA privilege in the parent catalog.
    /// </summary>
    Task<Schema> Create(
        SchemaAttributes attributes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the specified schema within the metastore. The caller must be a metastore admin, 
    /// the owner of the schema, or a user that has the USE_SCHEMA privilege on the schema.
    /// </summary>
    Task<Schema> Get(string schemaFullName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a schema for a catalog. The caller must be the owner of the schema or a metastore admin. 
    /// If the caller is a metastore admin, only the owner field can be changed in the update. 
    /// If the name field must be updated, the caller must be a metastore admin or have the CREATE_SCHEMA privilege on the parent catalog.
    /// </summary>
    Task<Schema> Update(
        string schemaFullName,
        string name = default,
        string owner = default,
        string comment = default,
        Dictionary<string, string> properties = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified schema from the parent catalog. The caller must be the owner of the schema or an owner of the parent catalog.
    /// </summary>
    Task Delete(string schemaFullName, CancellationToken cancellationToken = default);
}

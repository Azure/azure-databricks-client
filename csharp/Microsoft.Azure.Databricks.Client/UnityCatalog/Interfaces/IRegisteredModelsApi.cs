using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IRegisteredModelsApi : IDisposable
{
    /// <summary>
    /// List registered models. You can list registered models under a particular schema, or list all registered models in the current metastore.
    /// </summary>
    Task<IEnumerable<RegisteredModel>> ListRegisteredModels(
        string catalog_name = default,
        string schema_name = default,
        int max_results = default,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    ///Get a registered model
    /// The caller must be a metastore admin or an owner of(or have the EXECUTE privilege on) the registered model.For the latter case, the caller must also be the owner or have the USE_CATALOG privilege on the parent catalog and the USE_SCHEMA privilege on the parent schema.
    /// </summary>
    Task<RegisteredModel> GetRegisteredModel(string full_name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set an alias on the specified registered model.
    /// The caller must be a metastore admin or an owner of the registered model.For the latter case, the caller must also be the owner or have the USE_CATALOG privilege on the parent catalog and the USE_SCHEMA privilege on the parent schema.
    /// </summary>
    Task<RegisteredModelAlias> SetRegisteredModelAlias(
    string full_name,
    string alias,
    int version_num,
    CancellationToken cancellationToken = default);
}
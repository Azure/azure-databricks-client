using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IModelVersionApi : IDisposable
{
    /// <summary>
    /// List model versions. You can list model versions under a particular schema, or list all model versions in the current metastore.
    /// The returned models are filtered based on the privileges of the calling user. For example, the metastore admin is able to list all the model versions. A regular user needs to be the owner or have the EXECUTE privilege on the parent registered model to recieve the model versions in the response. For the latter case, the caller must also be the owner or have the USE_CATALOG privilege on the parent catalog and the USE_SCHEMA privilege on the parent schema.
    /// </summary>
    Task<IEnumerable<ModelVersion>> ListModelVersions(string full_name, int max_results = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a model version.
    /// The caller must be a metastore admin or an owner of (or have the EXECUTE privilege on) the parent registered model. For the latter case, the caller must also be the owner or have the USE_CATALOG privilege on the parent catalog and the USE_SCHEMA privilege on the parent schema.
    /// </summary>
    Task<ModelVersion> GetModelVersion(
        string full_name,
        int version,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get a model version by alias
    /// The caller must be a metastore admin or an owner of (or have the EXECUTE privilege on) the registered model. For the latter case, the caller must also be the owner or have the USE_CATALOG privilege on the parent catalog and the USE_SCHEMA privilege on the parent schema.
    /// </summary>
    Task<ModelVersion> GetModelVersionByAlias(string full_name, string alias, CancellationToken cancellationToken = default);
}

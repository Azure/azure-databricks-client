using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface IVolumesApi : IDisposable
{
    /// <summary>
    /// Creates a new volume.
    /// </summary>
    Task<Volume> Create(VolumeAttributes volumeAttributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a volume from the metastore for a specific catalog and schema.
    /// </summary>
    Task<Volume> Get(string fullVolumeName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified volume under the specified parent catalog and schema.
    /// </summary>
    Task<Volume> Update(
        string fullVolumeName,
        string name = default,
        string owner = default,
        string comment = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a volume from the specified parent catalog and schema.
    /// </summary>
    Task Delete(string fullVolumeName, CancellationToken cancellationToken = default);
}

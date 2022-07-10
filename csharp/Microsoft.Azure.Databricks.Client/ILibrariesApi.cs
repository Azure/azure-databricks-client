using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface ILibrariesApi : IDisposable
    {
        /// <summary>
        /// Get the status of all libraries on all clusters. A status will be available for all libraries installed on this cluster via the API or the libraries UI as well as libraries set to be installed on all clusters via the libraries UI. If a library has been set to be installed on all clusters, is_library_for_all_clusters will be true, even if the library was also installed on this specific cluster.
        /// </summary>
        Task<IDictionary<string, IEnumerable<LibraryFullStatus>>> AllClusterStatuses(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the status of libraries on a cluster. A status will be available for all libraries installed on this cluster via the API or the libraries UI as well as libraries set to be installed on all clusters via the libraries UI. If a library has been set to be installed on all clusters, is_library_for_all_clusters will be true, even if the library was was also installed on this specific cluster.
        /// </summary>
        /// <param name="clusterId">Unique identifier of the cluster whose status should be retrieved. This field is required.</param>
        Task<IEnumerable<LibraryFullStatus>> ClusterStatus(string clusterId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add libraries to be installed on a cluster. The installation is asynchronous - it happens in the background after the completion of this request. Note that the actual set of libraries to be installed on a cluster is the union of the libraries specified via this method and the libraries set to be installed on all clusters via the libraries UI.
        /// Note that CRAN libraries can only be installed on clusters running Databricks Runtime 3.2 or higher.
        /// </summary>
        /// <param name="clusterId">Unique identifier for the cluster on which to install these libraries. This field is required.</param>
        /// <param name="libraries">The libraries to install.</param>
        Task Install(string clusterId, IEnumerable<Library> libraries, CancellationToken cancellationToken = default);

        /// <summary>
        /// Set libraries to be uninstalled on a cluster. The libraries won’t be uninstalled until the cluster is restarted. Uninstalling libraries that are not installed on the cluster will have no impact but is not an error.
        /// </summary>
        /// <param name="clusterId">Unique identifier for the cluster on which to uninstall these libraries. This field is required.</param>
        /// <param name="libraries">The libraries to uninstall.</param>
        Task Uninstall(string clusterId, IEnumerable<Library> libraries, CancellationToken cancellationToken = default);
    }
}

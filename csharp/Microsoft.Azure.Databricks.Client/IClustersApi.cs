using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IClustersApi : IDisposable
    {
        /// <summary>
        /// Creates a new Spark cluster. This method will acquire new instances from the cloud provider if necessary. This method is asynchronous; the returned cluster_id can be used to poll the cluster status. When this method returns, the cluster will be in a PENDING state. The cluster will be usable once it enters a RUNNING state.
        /// </summary>
        Task<string> Create(ClusterInfo clusterInfo);

        /// <summary>
        /// Starts a terminated Spark cluster given its ID. This is similar to createCluster, except:
        ///     The previous cluster id and attributes are preserved.
        ///     The cluster starts with the last specified cluster size. If the previous cluster was an autoscaling cluster, the current cluster starts with the minimum number of nodes.
        ///     If the cluster is not in a TERMINATED state, nothing will happen.
        ///     Clusters launched to run a job cannot be started.
        /// </summary>
        Task Start(string clusterId);

        /// <summary>
        /// Edits the configuration of a cluster to match the provided attributes and size.
        /// A cluster can be edited if it is in a RUNNING or TERMINATED state.If a cluster is edited while in a RUNNING state, it will be restarted so that the new attributes can take effect.If a cluster is edited while in a TERMINATED state, it will remain TERMINATED. The next time it is started using the clusters/start API, the new attributes will take effect.An attempt to edit a cluster in any other state will be rejected with an INVALID_STATE error code.
        /// Clusters created by the Databricks Jobs service cannot be edited.
        /// </summary>
        Task Edit(string clusterId, ClusterInfo clusterConfig);

        /// <summary>
        /// Restarts a Spark cluster given its id. If the cluster is not in a RUNNING state, nothing will happen.
        /// </summary>
        Task Restart(string clusterId);

        /// <summary>
        /// Resizes a cluster to have a desired number of workers. This will fail unless the cluster is in a RUNNING state.
        /// </summary>
        Task Resize(string clusterId, int numWorkers);

        /// <summary>
        /// Resizes a cluster to have a desired number of workers. This will fail unless the cluster is in a RUNNING state.
        /// </summary>
        Task Resize(string clusterId, AutoScale autoScale);

        /// <summary>
        /// Retrieves the information for a cluster given its identifier. Clusters can be described while they are running, or up to 30 days after they are terminated.
        /// </summary>
        Task<ClusterInfo> Get(string clusterId);

        /// <summary>
        /// Terminates a Spark cluster given its id. The cluster is removed asynchronously. Once the termination has completed, the cluster will be in a TERMINATED state. If the cluster is already in a TERMINATING or TERMINATED state, nothing will happen.
        /// </summary>
        /// <param name="clusterId">The cluster to be terminated. This field is required.</param>
        Task Terminate(string clusterId);

        /// <summary>
        /// Permanently deletes a Spark cluster. If the cluster is running, it is terminated and its resources are asynchronously removed. If the cluster is terminated, then it is immediately removed.
        /// You cannot perform any action on a permanently deleted cluster and a permanently deleted cluster is no longer returned in the cluster list.
        /// </summary>
        /// <param name="clusterId">The cluster to be permanently deleted. This field is required.</param>
        Task Delete(string clusterId);

        /// <summary>
        /// Returns information about all pinned clusters, currently active clusters, up to 70 of the most recently terminated interactive clusters in the past 30 days, and up to 30 of the most recently terminated job clusters in the past 30 days. For example, if there is 1 pinned cluster, 4 active clusters, 45 terminated interactive clusters in the past 30 days, and 50 terminated job clusters in the past 30 days, then this API returns the 1 pinned cluster, 4 active clusters, all 45 terminated interactive clusters, and the 30 most recently terminated job clusters.
        /// </summary>
        Task<IEnumerable<ClusterInfo>> List();

        /// <summary>
        /// Pinning a cluster ensures that the cluster is always returned by the List API. Pinning a cluster that is already pinned has no effect.
        /// </summary>
        Task Pin(string clusterId);

        /// <summary>
        /// Unpinning a cluster will allow the cluster to eventually be removed from the list returned by the List API. Unpinning a cluster that is not pinned has no effect.
        /// </summary>
        Task Unpin(string clusterId);

        /// <summary>
        /// Returns a list of supported Spark node types. These node types can be used to launch a cluster.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<NodeType>> ListNodeTypes();

        /// <summary>
        /// Returns the list of available Spark versions. These versions can be used to launch a cluster.
        /// </summary>
        Task<IDictionary<string, string>> ListSparkVersions();
    }
}
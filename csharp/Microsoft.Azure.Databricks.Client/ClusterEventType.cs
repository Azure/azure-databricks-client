// ReSharper disable InconsistentNaming
namespace Microsoft.Azure.Databricks.Client
{
    public enum ClusterEventType
    {
        /// <summary>
        /// Indicates that the cluster is being created by someone.
        /// </summary>
        CREATING,

        /// <summary>
        /// Indicates that the cluster is being started by someone.
        /// </summary>
        STARTING,

        /// <summary>
        /// Indicates that the cluster is being started by someone.
        /// </summary>
        RESTARTING,

        /// <summary>
        /// Indicates that the cluster is being terminated.
        /// </summary>
        TERMINATING,

        /// <summary>
        /// Indicates that the cluster has been edited by someone.
        /// </summary>
        EDITED,

        /// <summary>
        /// Indicates the cluster finished creating, starting, or restarting. Includes the number of nodes in the cluster and a failure reason if some nodes could not be acquired.
        /// </summary>
        RUNNING,

        /// <summary>
        /// Indicates a change in the target size of the cluster (upsize or downsize).
        /// </summary>
        RESIZING,

        /// <summary>
        /// Indicates that nodes finished being added to the cluster. Includes the number of nodes in the cluster and a failure reason if some nodes could not be acquired.
        /// </summary>
        UPSIZE_COMPLETED,

        /// <summary>
        /// Indicates that some nodes were lost from the cluster.
        /// </summary>
        NODES_LOST
    }
}
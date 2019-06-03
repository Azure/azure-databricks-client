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
        /// Indicates that a disk is low on space, but adding disks would put it over the max capacity.
        /// </summary>
        DID_NOT_EXPAND_DISK,

        /// <summary>
        /// Indicates that a disk was low on space and the disks were expanded.
        /// </summary>
        EXPANDED_DISK,

        /// <summary>
        /// Indicates that a disk was low on space and disk space could not be expanded.
        /// </summary>
        FAILED_TO_EXPAND_DISK,

        /// <summary>
        /// The initialize scripts startingIndicates that the cluster scoped init script has started.
        /// </summary>
        INIT_SCRIPTS_STARTING,

        /// <summary>
        /// Indicates that the cluster scoped init script has finished.
        /// </summary>
        INIT_SCRIPTS_FINISHED,

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
        NODES_LOST,

        /// <summary>
        /// Indicates that the driver is healthy.
        /// </summary>
        DRIVER_HEALTHY,

        /// <summary>
        /// Indicates that the driver is unavailable.
        /// </summary>
        DRIVER_UNAVAILABLE,

        /// <summary>
        /// Indicates that a Spark exception was thrown from the driver.
        /// </summary>
        SPARK_EXCEPTION,

        /// <summary>
        /// Indicates that the driver is up but is not responsive, likely due to GC.
        /// </summary>
        DRIVER_NOT_RESPONDING,

        /// <summary>
        /// Indicates that the driver is up but DBFS is down.
        /// </summary>
        DBFS_DOWN,

        /// <summary>
        /// Indicates that the driver is up but the metastore is down.
        /// </summary>
        METASTORE_DOWN,

        /// <summary>
        /// Usage report containing the total and unused instance minutes of the autoscaling cluster over the last hour.
        /// </summary>
        AUTOSCALING_STATS_REPORT
    }
}
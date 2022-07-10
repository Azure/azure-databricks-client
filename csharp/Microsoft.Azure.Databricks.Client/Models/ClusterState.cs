// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// The state of a cluster. The current allowable state transitions are as follows:
    ///     PENDING -> RUNNING
    ///     PENDING -> TERMINATING
    ///     RUNNING -> RESIZING
    ///     RUNNING -> RESTARTING
    ///     RUNNING -> TERMINATING
    ///     RESTARTING -> RUNNING
    ///     RESTARTING -> TERMINATING
    ///     RESIZING -> RUNNING
    ///     RESIZING -> TERMINATING
    ///     TERMINATING -> TERMINATED
    /// </summary>
    public enum ClusterState
    {
        /// <summary>
        /// Indicates that a cluster is in the process of being created.
        /// </summary>
        PENDING,

        /// <summary>
        /// Indicates that a cluster has been started and is ready for use.
        /// </summary>
        RUNNING,

        /// <summary>
        /// Indicates that a cluster is in the process of restarting.
        /// </summary>
        RESTARTING,

        /// <summary>
        /// Indicates that a cluster is in the process of adding or removing nodes.
        /// </summary>
        RESIZING,

        /// <summary>
        /// Indicates that a cluster is in the process of being destroyed.
        /// </summary>
        TERMINATING,

        /// <summary>
        /// Indicates that a cluster has been successfully destroyed.
        /// </summary>
        TERMINATED,

        /// <summary>
        /// This state is not used anymore. It was used to indicate a cluster that failed to be created. Terminating and Terminated are used instead.
        /// </summary>
        ERROR,

        /// <summary>
        /// Indicates that a cluster is in an unknown state. A cluster should never be in this state.
        /// </summary>
        UNKNOWN
    }
}

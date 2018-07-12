// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// Indicates the service that created the cluster.
    /// </summary>
    public enum ClusterSource
    {
        /// <summary>
        /// Cluster created through the UI.
        /// </summary>
        UI,

        /// <summary>
        /// Cluster created by the Databricks Job Scheduler.
        /// </summary>
        JOB,

        /// <summary>
        /// Cluster created through an API call.
        /// </summary>
        API
    }
}

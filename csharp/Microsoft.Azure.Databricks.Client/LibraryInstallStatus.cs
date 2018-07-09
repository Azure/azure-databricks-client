// ReSharper disable InconsistentNaming
namespace Microsoft.Azure.Databricks.Client
{
    public enum LibraryInstallStatus
    {
        /// <summary>
        /// No action has yet been taken to install the library. This state should be very short lived.
        /// </summary>
        PENDING,

        /// <summary>
        /// Metadata necessary to install the library is being retrieved from the provided repository.
        /// For jar and egg libraries, this step is a no-op.
        /// </summary>
        RESOLVING,

        /// <summary>
        /// The library is actively being installed, either by adding resources to Spark or executing system commands inside the Spark nodes.
        /// </summary>
        INSTALLING,

        /// <summary>
        /// The library has been successfully installed and can now be used.
        /// </summary>
        INSTALLED,

        /// <summary>
        /// Some step in installation failed. More information can be found in the messages field.
        /// </summary>
        FAILED,

        /// <summary>
        /// The library has been marked for removal. Libraries can be removed only when clusters are restarted, so libraries that enter this state will remain until the cluster is restarted.
        /// </summary>
        UNINSTALL_ON_RESTART
    }
}
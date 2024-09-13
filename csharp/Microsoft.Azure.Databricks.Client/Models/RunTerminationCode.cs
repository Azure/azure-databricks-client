namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// A value indicating the run's current state.
/// </summary>
public enum RunTerminationCode
{
    /// <summary>
    /// The run was completed successfully.
    /// </summary>
    SUCCESS,

    /// <summary>
    /// The run was canceled during execution by the Azure Databricks platform; for example, if the maximum run duration was exceeded.
    /// </summary>
    CANCELED,

    /// <summary>
    /// Run was never executed, for example, if the upstream task run failed, the dependency type condition was not met, or there were no material tasks to execute.
    /// </summary>
    SKIPPED,

    /// <summary>
    /// The run encountered an unexpected error. Refer to the state message for further details.
    /// </summary>
    INTERNAL_ERROR,

    /// <summary>
    /// The run encountered an error while communicating with the Spark Driver.
    /// </summary>
    DRIVER_ERROR,

    /// <summary>
    /// The run failed due to a cluster error. Refer to the state message for further details.
    /// </summary>
    CLUSTER_ERROR,

    /// <summary>
    /// Failed to complete the checkout due to an error when communicating with the third party service.
    /// </summary>
    REPOSITORY_CHECKOUT_FAILED,

    /// <summary>
    /// The run failed because it issued an invalid request to start the cluster.
    /// </summary>
    INVALID_CLUSTER_REQUEST,

    /// <summary>
    /// The workspace has reached the quota for the maximum number of concurrent active runs. Consider scheduling the runs over a larger time frame.
    /// </summary>
    WORKSPACE_RUN_LIMIT_EXCEEDED,

    /// <summary>
    /// The run failed because it tried to access a feature unavailable for the workspace.
    /// </summary>
    FEATURE_DISABLED,

    /// <summary>
    /// The number of cluster creation, start, and upsize requests have exceeded the allotted rate limit. Consider spreading the run execution over a larger time frame.
    /// </summary>
    CLUSTER_REQUEST_LIMIT_EXCEEDED,

    /// <summary>
    /// The run failed due to an error when accessing the customer blob storage. Refer to the state message for further details.
    /// </summary>
    STORAGE_ACCESS_ERROR,

    /// <summary>
    /// The run was completed with task failures. For more details, refer to the state message or run output.
    /// </summary>
    RUN_EXECUTION_ERROR,

    /// <summary>
    /// The run failed due to a permission issue while accessing a resource. Refer to the state message for further details.
    /// </summary>
    UNAUTHORIZED_ERROR,

    /// <summary>
    /// The run failed while installing the user-requested library. Refer to the state message for further details. The causes might include, but are not limited to: The provided library is invalid, there are insufficient permissions to install the library, and so forth.
    /// </summary>
    LIBRARY_INSTALLATION_ERROR,

    /// <summary>
    /// The scheduled run exceeds the limit of maximum concurrent runs set for the job.
    /// </summary>
    MAX_CONCURRENT_RUNS_EXCEEDED,

    /// <summary>
    /// The run is scheduled on a cluster that has already reached the maximum number of contexts it is configured to create.
    /// </summary>
    MAX_SPARK_CONTEXTS_EXCEEDED,

    /// <summary>
    /// A resource necessary for run execution does not exist. Refer to the state message for further details.
    /// </summary>
    RESOURCE_NOT_FOUND,

    /// <summary>
    /// The run failed due to an invalid configuration. Refer to the state message for further details.
    /// </summary>
    INVALID_RUN_CONFIGURATION,

    /// <summary>
    /// The run failed due to a cloud provider issue. Refer to the state message for further details.
    /// </summary>
    CLOUD_FAILURE,

    /// <summary>
    /// The run was skipped due to reaching the job level queue size limit.
    /// </summary>
    MAX_JOB_QUEUE_SIZE_EXCEEDED
}
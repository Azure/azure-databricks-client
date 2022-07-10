// ReSharper disable InconsistentNaming

namespace Microsoft.Azure.Databricks.Client.Models
{
    public enum TerminationCode
    {
        /// <summary>
        /// A user terminated the cluster directly. Parameters should include a username field that indicates the specific user who terminated the cluster.
        /// </summary>
        USER_REQUEST,

        /// <summary>
        /// This cluster was launched by a job, and terminated when the job completed.
        /// </summary>
        JOB_FINISHED,

        /// <summary>
        /// This cluster was terminated since it was idle.
        /// </summary>
        INACTIVITY,

        /// <summary>
        /// The instance that hosted the spark driver was terminated by the cloud provider. In AWS, for example, AWS may retire instances and directly shut them down. Parameters should include an aws_instance_state_reason field indicating the AWS-provided reason why the instance was terminated.
        /// </summary>
        CLOUD_PROVIDER_SHUTDOWN,

        /// <summary>
        /// Databricks may lose connection to services on the driver instance. One such case is when problems arise in cloud networking infrastructure, or when the instance itself becomes unhealthy.
        /// </summary>
        COMMUNICATION_LOST,

        /// <summary>
        /// Databricks may hit cloud provider failures when requesting instances to launch clusters. For example, AWS limits the number of running instances and EBS volumes. If you ask Databricks to launch a cluster that requires instances or EBS volumes that exceed your AWS limit, the cluster will fail with this status code. Parameters should include one of aws_api_error_code, aws_instance_state_reason, or aws_spot_request_status to indicate the AWS-provided reason why Databricks could not request the required instances for the cluster.
        /// </summary>
        CLOUD_PROVIDER_LAUNCH_FAILURE,

        /// <summary>
        /// The Spark driver failed to start. Possible reasons may include incompatible libraries and initialization scripts that corrupted the Spark container.
        /// </summary>
        SPARK_STARTUP_FAILURE,

        /// <summary>
        /// Cannot launch the cluster because the user specified an invalid argument. For example, the use might specify an invalid spark version for the cluster.
        /// </summary>
        INVALID_ARGUMENT,

        /// <summary>
        /// While launching this cluster, Databricks failed to complete critical setup steps, terminating the cluster.
        /// </summary>
        UNEXPECTED_LAUNCH_FAILURE,

        /// <summary>
        /// Databricks encountered an unexpected error which forced the running cluster to be terminated. Contact Databricks support for additional details.
        /// </summary>
        INTERNAL_ERROR,

        /// <summary>
        /// Databricks was not able to access instances in order to start the cluster. This can be a transient networking issue. If the problem persists, this usually indicates a networking environment misconfiguration.
        /// </summary>
        INSTANCE_UNREACHABLE,

        /// <summary>
        /// Instance pool backed cluster specific failure Refer to Pools for details.
        /// </summary>
        INSTANCE_POOL_CLUSTER_FAILURE,

        /// <summary>
        /// Databricks cannot handle the request at this moment. Try again later and contact Databricks if the problem persists.
        /// </summary>
        REQUEST_REJECTED,

        /// <summary>
        /// The init script for the driver container failed, causing cluster creation to fail.
        /// </summary>
        INIT_SCRIPT_FAILURE,

        /// <summary>
        /// The Azure Databricks trial subscription expired.
        /// </summary>
        TRIAL_EXPIRED,

        /// <summary>
        /// Databricks driver may be down, resulting in a spark error.
        /// </summary>
        SPARK_ERROR,

        AZURE_RESOURCE_PROVIDER_THROTTLING,

        /// <summary>
        /// OTHER
        /// </summary>
        OTHER,

        /// <summary>
        /// The cluster failed to start because of user network configuration issues. Possible reasons include misconfiguration of firewall settings, UDR entries, DNS, or route tables.
        /// </summary>
        BOOTSTRAP_TIMEOUT,

        /// <summary>
        /// Azure Databricks was unable to launch containers on worker nodes for the cluster. Have your admin check your network configuration.
        /// </summary>
        CONTAINER_LAUNCH_FAILURE,

        /// <summary>
        /// The cluster failed to start because Databricks File System (DBFS) could not be reached.
        /// </summary>
        DBFS_COMPONENT_UNHEALTHY,

        /// <summary>
        /// Azure Databricks was not able to access the Spark driver, because it was not reachable.
        /// </summary>
        DRIVER_UNREACHABLE,

        /// <summary>
        /// Azure Databricks was not able to access the Spark driver, because it was unresponsive.
        /// </summary>
        DRIVER_UNRESPONSIVE,

        /// <summary>
        /// The cluster failed to start because the external metastore could not be reached
        /// </summary>
        METASTORE_COMPONENT_UNHEALTHY,

        /// <summary>
        /// The cluster was terminated due to an error in the network configuration. For example, a workspace with VNet injection had incorrect DNS settings that blocked access to worker artifacts.
        /// </summary>
        NETWORK_CONFIGURATION_FAILURE,

        /// <summary>
        /// Azure Databricks reached the Azure Resource Manager request limit which will prevent the Azure SDK from issuing any read or write request to the Azure Resource Manager. The request limit is applied to each subscription every hour. Retry after an hour or changing to a smaller cluster size might help to resolve the issue.
        /// </summary>
        AZURE_RESOURCE_MANAGER_THROTTLING
    }
}

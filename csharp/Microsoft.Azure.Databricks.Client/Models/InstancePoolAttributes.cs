using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record InstancePoolAttributes
    {
        /// <summary>
        /// The name of the instance pool. This is required for create and edit operations. It must be unique, non-empty, and less than 100 characters.
        /// </summary>
        [JsonPropertyName("instance_pool_name")]
        public string PoolName { get; set; }

        /// <summary>
        /// The minimum number of idle instances maintained by the pool. This is in addition to any instances in use by active clusters.
        /// </summary>
        [JsonPropertyName("min_idle_instances")]
        public int MinIdleInstances { get; set; }

        /// <summary>
        /// The maximum number of instances the pool can contain, including both idle instances and ones in use by clusters.
        /// Once the maximum capacity is reached, you cannot create new clusters from the pool and existing clusters cannot autoscale up until some instances are made idle in the pool via cluster termination or down-scaling.
        /// </summary>
        [JsonPropertyName("max_capacity")]
        public int MaxCapacity { get; set; }

        /// <summary>
        /// The node type for the instances in the pool. All clusters attached to the pool inherit this node type and the pool's idle instances are allocated based on this type. You can retrieve a list of available node types by using the List Node Types API call.
        /// </summary>
        [JsonPropertyName("node_type_id")]
        public string NodeTypeId { get; set; }

        /// <summary>
        /// Additional tags for instance pool resources. Azure Databricks tags all pool resources (e.g. VM disk volumes) with these tags in addition to default_tags
        /// Azure Databricks allows up to 6 custom tags.
        /// </summary>
        [JsonPropertyName("custom_tags")]
        public Dictionary<string, string> CustomTags { get; set; }

        /// <summary>
        /// The number of minutes that idle instances in excess of the min_idle_instances are maintained by the pool before being terminated.
        /// If not specified, excess idle instances are terminated automatically after a default timeout period.
        /// If specified, the time must be between 0 and 10000 minutes. If 0 is supplied, excess idle instances are removed as soon as possible.
        /// </summary>
        [JsonPropertyName("idle_instance_autotermination_minutes")]
        public int? IdleInstanceAutoTerminationMinutes { get; set; }

        /// <summary>
        /// Autoscaling Local Storage: when enabled, the instances in the pool dynamically acquire additional disk space when they are running low on disk space.
        /// </summary>
        [JsonPropertyName("enable_elastic_disk")]
        public bool EnableElasticDisk { get; set; }

        /// <summary>
        /// Defines the amount of initial remote storage attached to each instance in the pool.
        /// </summary>
        [JsonPropertyName("disk_spec")]
        public DiskSpec DiskSpec { get; set; }

        /// <summary>
        /// A list of Spark image versions the pool installs on each instance.
        /// Pool clusters that use one of the preloaded Spark version start faster as they do have to wait for the Spark image to download.
        /// You can retrieve a list of available Spark versions by using the Spark Versions API call.
        /// </summary>
        [JsonPropertyName("preloaded_spark_versions")]
        public string[] PreloadedSparkVersions { get; set; }

        /// <summary>
        /// A list with at least one Docker image the pool installs on each instance. Pool clusters that use a preloaded Docker image start faster as they do not have to wait for the image to download.
        /// Available only if your account has Databricks Container Services enabled.
        /// </summary>
        [JsonPropertyName("preloaded_docker_images")]
        public DockerImage[] PreloadedDockerImages { get; set; }

        /// <summary>
        /// Defines the instance availability type (such as spot or on-demand) and max bid price.
        /// </summary>
        [JsonPropertyName("azure_attributes")]
        public InstancePoolAzureAttributes AzureAttributes { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record ClusterSize
    {
        /// <summary>
        /// Number of worker nodes that this cluster should have. A cluster has one Spark Driver and num_workers Executors for a total of num_workers + 1 Spark nodes.
        /// </summary>
        /// <remarks>
        /// Note: When reading the properties of a cluster, this field reflects the desired number of workers rather than the actual current number of workers.
        /// For instance, if a cluster is resized from 5 to 10 workers, this field will immediately be updated to reflect the target size of 10 workers, whereas the workers listed in spark_info will gradually increase from 5 to 10 as the new nodes are provisioned.
        /// </remarks>
        [JsonPropertyName("num_workers")]
        public int? NumberOfWorkers { get; set; }

        /// <summary>
        /// Parameters needed in order to automatically scale clusters up and down based on load.Note: autoscaling works best with DB runtime versions 3.0 or later.
        /// </summary>
        [JsonPropertyName("autoscale")]
        public AutoScale AutoScale { get; set; }
    }
}
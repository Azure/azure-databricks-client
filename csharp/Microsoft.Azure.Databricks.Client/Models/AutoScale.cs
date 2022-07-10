using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record AutoScale
    {
        /// <summary>
        /// Gets or sets the minimum number of workers to which the cluster can scale down when underutilized. It is also the initial number of workers the cluster will have after creation.
        /// </summary>
        /// <value>
        /// The minimum number of workers to which the cluster can scale down when underutilized. It is also the initial number of workers the cluster will have after creation.
        /// </value>
        [JsonPropertyName("min_workers")]
        public int MinWorkers { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of workers to which the cluster can scale up when overloaded. Note that max_workers must be strictly greater than min_workers.
        /// </summary>
        /// <value>
        /// The maximum number of workers to which the cluster can scale up when overloaded. Note that max_workers must be strictly greater than min_workers.
        /// </value>
        [JsonPropertyName("max_workers")]
        public int MaxWorkers { get; set; }
    }
}

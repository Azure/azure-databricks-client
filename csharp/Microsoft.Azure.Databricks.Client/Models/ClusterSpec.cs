using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record ClusterSpec
{
    /// <summary>
    /// The id of an existing cluster that will be used for all runs of this job. Please note that when running jobs on an existing cluster, you may need to manually restart the cluster if it stops responding. We suggest running jobs on new clusters for greater reliability.
    /// </summary>
    [JsonPropertyName("existing_cluster_id")]
    public string ExistingClusterId { get; set; }

    /// <summary>
    /// A description of a cluster that will be created for each run.
    /// </summary>
    /// <value>
    /// The new cluster.
    /// </value>
    [JsonPropertyName("new_cluster")]
    public ClusterAttributes NewCluster { get; set; }

    /// <summary>
    /// An optional list of libraries to be installed on the cluster that will execute the job. The default value is an empty list.
    /// </summary>
    [JsonPropertyName("libraries")]
    public List<Library> Libraries { get; set; }
}
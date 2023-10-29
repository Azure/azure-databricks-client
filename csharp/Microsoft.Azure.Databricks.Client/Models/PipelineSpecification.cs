using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The pipeline specification. This field is not returned when called by ListPipelines
/// </summary>
public record PipelineSpecification
{
    /// <summary>
    /// Unique identifier for this pipeline.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Friendly identifier for this pipeline.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// DBFS root directory for storing checkpoints and tables.
    /// </summary>
    [JsonPropertyName("storage")]
    public string Storage { get; set; }

    /// <summary>
    /// String-String configuration for this pipeline execution.
    /// </summary>
    [JsonPropertyName("configuration")]
    public Dictionary<string, string> PipelineConfiguration { get; set; }

    /// <summary>
    /// Cluster settings for this pipeline deployment.
    /// </summary>
    [JsonPropertyName("clusters")]
    public IEnumerable<ClusterAttributes> Clusters { get; set; }

    /// <summary>
    /// Libraries or code needed by this deployment.
    /// </summary>
    [JsonPropertyName("libraries")]
    public IEnumerable<Library> Libraries { get; set; }

    /// <summary>
    /// Target schema (database) to add tables in this pipeline to. If not specified, no data is published to the Hive metastore or Unity Catalog.
    /// To publish to Unity Catalog, also specify catalog.
    /// </summary>
    [JsonPropertyName("target")]
    public string Target { get; set; }

    /// <summary>
    /// Filters on which Pipeline packages to include in the deployed graph.
    /// </summary>
    [JsonPropertyName("filters")]
    public PipelineFilters Filters { get; set; }

    /// <summary>
    /// Whether the pipeline is continuous or triggered. This replaces trigger.
    /// </summary>
    [JsonPropertyName("continuous")]
    public bool Continuous { get; set; }

    /// <summary>
    /// Whether the pipeline is in Development mode. Defaults to false.
    /// </summary>
    [JsonPropertyName("development")]
    public bool Development { get; set; }

    /// <summary>
    /// Whether Photon is enabled for this pipeline.
    /// </summary>
    [JsonPropertyName("photon")]
    public bool Photon { get; set; }

    /// <summary>
    /// Pipeline product edition.
    /// </summary>
    [JsonPropertyName("edition")]
    public string Edition { get; set; }

    /// <summary>
    /// DLT Release Channel that specifies which version to use.
    /// </summary>
    [JsonPropertyName("channel")]
    public string Channel { get; set; }

    /// <summary>
    /// A catalog in Unity Catalog to publish data from this pipeline to.
    /// If target is specified, tables in this pipeline are published to
    /// a target schema inside catalog (for example, catalog.target.table). 
    /// If target is not specified, no data is published to Unity Catalog.
    /// </summary>
    [JsonPropertyName("catalog")]
    public string Catalog { get; set; }

    /// <summary>
    /// List of notification settings for this pipeline.
    /// </summary>
    [JsonPropertyName("notifications")]
    public IEnumerable<PipelineNotification> Notifications { get; set; }
}

public record PipelineNotification
{
    /// <summary>
    /// A list of email addresses notified when a configured alert is triggered.
    /// </summary>
    [JsonPropertyName("email_recipients")]
    public IEnumerable<string> EmailRecipients { get; set; }

    /// <summary>
    /// A list of alerts that trigger the sending of notifications to the configured destinations. The supported alerts are:
    //    on-update-success: A pipeline update completes successfully.
    //    on-update-failure: Each time a pipeline update fails.
    //    on-update-fatal-failure: A pipeline update fails with a non-retryable (fatal) error.
    //    on-flow-failure: A single data flow fails.
    /// </summary>
    [JsonPropertyName("alerts")]
    public IEnumerable<string> Alerts { get; set; }
}

public enum PipelineNotificationAlert
{
    [EnumMember(Value = "on-update-success")]
    ON_UPDATE_SUCCESS,

    [EnumMember(Value = "on-update-failure")]
    ON_UPDATE_FAILURE,

    [EnumMember(Value = "on-update-fatal-failure")]
    ON_UPDATE_FATAL_FAILURE,

    [EnumMember(Value = "on-flow-failure")]
    ON_FLOW_FAILURE

}
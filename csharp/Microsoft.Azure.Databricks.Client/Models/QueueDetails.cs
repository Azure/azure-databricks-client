using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// If the run was queued, provides the details for queuing the run.
/// </summary>
public class QueueDetails
{
    /// <summary>
    /// The reason for queuing the run.
    /// </summary>
    [JsonPropertyName("code")]
    public QueueCode Code { get; set; }

    /// <summary>
    /// A descriptive message with the queuing details. This field is unstructured, and its exact format is subject to change.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
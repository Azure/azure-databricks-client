using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// Provides details around the current status of the run.
/// </summary>
public class RunStatus
{
    /// <summary>
    /// The current state of the run.
    /// </summary>
    [JsonPropertyName("state")]
    public RunStatusState State { get; set; }

    /// <summary>
    /// If the run is in a TERMINATING or TERMINATED state, details about the reason for terminating the run.
    /// </summary>
    [JsonPropertyName("termination_details")]
    public TerminationDetails TerminationDetails { get; set; }

    /// <summary>
    /// If the run was queued, details about the reason for queuing the run.
    /// </summary>
    [JsonPropertyName("queue_details")]
    public QueueDetails QueueDetails { get; set; }
}
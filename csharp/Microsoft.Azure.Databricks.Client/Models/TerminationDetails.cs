using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// Provides the details of a run that is in a TERMINATING or TERMINATED state.
/// </summary>
public class TerminationDetails
{
    /// <summary>
    /// The code indicates why the run was terminated.
    /// </summary>
    [JsonPropertyName("code")]
    public RunTerminationCode Code { get; set; }

    /// <summary>
    /// The describes the overall termination type.
    /// </summary>
    [JsonPropertyName("type")]
    public RunTerminationType Type { get; set; }

    /// <summary>
    /// A descriptive message with the queuing details. This field is unstructured, and its exact format is subject to change.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public enum RunTerminationType
{
    SUCCESS,
    INTERNAL_ERROR,
    CLIENT_ERROR,
    CLOUD_FAILURE
}
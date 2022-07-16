using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record RunIdentifier
{
    /// <summary>
    /// The globally unique id of the newly triggered run.
    /// </summary>
    [JsonPropertyName("run_id")]
    public long RunId { get; set; }

    /// <summary>
    /// The sequence number of this run among all runs of the job.
    /// </summary>
    [Obsolete("This is set to the same value as `run_id`.")]
    [JsonPropertyName("number_in_job")]
    public long NumberInJob { get; set; }
}
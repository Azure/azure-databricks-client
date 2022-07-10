using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record JobList
{
    /// <summary>
    /// The list of jobs.
    /// </summary>
    [JsonPropertyName("jobs")]
    public IEnumerable<Job> Jobs { get; set; }

    /// <summary>
    /// If true, additional jobs matching the provided filter are available for listing.
    /// </summary>
    [JsonPropertyName("has_more")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public bool HasMore { get; set; }
}
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public class PipelinesList
{
    public PipelinesList()
    {
        this.Pipelines = new List<Pipeline>();
    }

    /// <summary>
    /// The list of events matching the request criteria.
    /// </summary>
    [JsonPropertyName("statuses")]
    public IEnumerable<Pipeline> Pipelines { get; set; }

    /// <summary>
    /// If present, a token to fetch the next page of events.
    /// </summary>
    [JsonPropertyName("next_page_token")]
    public string NextPageToken { get; set; }

    [JsonIgnore]
    public bool HasMore => !string.IsNullOrEmpty(this.NextPageToken);
}

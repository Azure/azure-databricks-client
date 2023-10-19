using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record PipelineUpdatesList
{
    /// <summary>
    /// List of pipeline updates
    /// </summary>
    [JsonPropertyName("updates")]
    public IEnumerable<PipelineUpdate> Updates { get; set; }

    /// <summary>
    /// If present, then there are more results, and this a token to be used in a subsequent request to fetch the next page.
    /// </summary>
    [JsonPropertyName("next_page_token")]
    public string NextPageToken { get; set; }

    /// <summary>
    /// If present, then this token can be used in a subsequent request to fetch the previous page.
    /// </summary>
    [JsonPropertyName("prev_page_token")]
    public string PreviousPageToken { get; set; }
}

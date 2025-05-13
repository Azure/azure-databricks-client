using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public class DirectoriesList
{
    public DirectoriesList()
    {
        this.Contents = new List<DirectoryEntry>();
    }

    /// <summary>
    /// Array of DirectoryEntry.
    /// </summary>
    [JsonPropertyName("contents")]
    public IEnumerable<DirectoryEntry> Contents { get; set; }

    /// <summary>
    /// A token, which can be sent as `page_token` to retrieve the next page.
    /// </summary>
    [JsonPropertyName("next_page_token")]
    public string NextPageToken { get; set; }

    [JsonIgnore]
    public bool HasMore => !string.IsNullOrEmpty(this.NextPageToken);
}

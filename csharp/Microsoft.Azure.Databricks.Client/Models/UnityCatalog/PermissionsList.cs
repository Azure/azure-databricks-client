using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public class PermissionsList
{
    public PermissionsList()
    {
        this.Permissions = new List<Permission>();
    }

    /// <summary>
    /// The privileges assigned to each principal
    /// </summary>
    [JsonPropertyName("privilege_assignments")]
    public IEnumerable<Permission> Permissions { get; set; }

    /// <summary>
    /// Opaque token to retrieve the next page of results. Absent if there are no more pages. page_token should be set to this value for the next request (for the next page of results).
    /// </summary>
    [JsonPropertyName("next_page_token")]
    public string NextPageToken { get; set; }

    [JsonIgnore]
    public bool HasMore => !string.IsNullOrEmpty(this.NextPageToken);
}

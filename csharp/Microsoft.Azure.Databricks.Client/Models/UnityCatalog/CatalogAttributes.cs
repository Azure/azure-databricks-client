using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record CatalogAttributes
{
    /// <summary>
    /// Name of catalog.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// A map of key-value properties attached to the securable.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, string> Properties { get; set; }

    /// <summary>
    /// Storage root URL for managed tables within catalog.
    /// </summary>
    [JsonPropertyName("storage_root")]
    public string StorageRoot { get; set; }

    /// <summary>
    /// The name of delta sharing provider.
    ///
    ///A Delta Sharing catalog is a catalog that is based on a Delta share on a remote sharing server.
    /// </summary>
    [JsonPropertyName("provider_name")]
    public string ProviderName { get; set; }

    /// <summary>
    /// The name of the share under the share provider.
    /// </summary>
    [JsonPropertyName("share_name")]
    public string ShareName { get; set; }

    /// <summary>
    /// Whether the current securable is accessible from all workspaces or a specific set of workspaces.
    /// </summary>
    [JsonPropertyName("connection_name")]
    public string ConnectionName { get; set; }

    /// <summary>
    /// A map of key-value properties attached to the securable.
    /// </summary>
    [JsonPropertyName("options")]
    public Dictionary<string, string> Options { get; set; }
}

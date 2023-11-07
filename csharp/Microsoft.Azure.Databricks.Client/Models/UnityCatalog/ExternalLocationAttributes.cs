using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record ExternalLocationAttributes
{
    /// <summary>
    /// Name of the external location.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Path URL of the external location.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// Name of the storage credential used with this location.
    /// </summary>
    [JsonPropertyName("credential_name")]
    public string CredentialName { get; set; }

    /// <summary>
    /// Indicates whether the external location is read-only.
    /// </summary>
    [JsonPropertyName("read_only")]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}

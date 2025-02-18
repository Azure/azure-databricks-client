using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record ShareAttributes
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
    /// Storage root URL for the share.
    /// </summary>
    [JsonPropertyName("storage_root")]
    public string StorageRoot { get; set; }
}

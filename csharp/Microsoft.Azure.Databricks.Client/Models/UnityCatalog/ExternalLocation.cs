using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public class ExternalLocation
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

    /// <summary>
    /// The owner of the external location.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// Unique identifier of metastore hosting the external location.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// Unique ID of the location's storage credential.
    /// </summary>
    [JsonPropertyName("credential_id")]
    public string CredentialId { get; set; }

    /// <summary>
    /// Time at which this external location was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of external location creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CrteatedBy { get; set; }

    /// <summary>
    /// Time at which external location this was last modified, in epoch milliseconds
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of user who last modified the external location.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }
}

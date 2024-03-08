using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record ExternalLocation : ExternalLocationAttributes
{
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
    public string CreatedBy { get; set; }

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

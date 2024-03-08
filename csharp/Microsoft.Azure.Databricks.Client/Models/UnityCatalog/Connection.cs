using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Connection : ConnectionAttributes
{
    /// <summary>
    /// Username of current owner of the connection.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// Full name of connection.
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    /// <summary>
    /// URL of the remote data source, extracted from options.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// The type of credential.
    /// </summary>
    [JsonPropertyName("credential_type")]
    public string CredentialType { get; set; } = "USERNAME_PASSWORD";

    /// <summary>
    /// Unique identifier of the Connection.
    /// </summary>
    [JsonPropertyName("connection_id")]
    public string ConnectionId { get; set; }

    /// <summary>
    /// Unique identifier of parent metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// Time at which this connection was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of connection creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Time at which this connection was updated, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of user who last modified connection.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// Kind of connection securable.
    /// </summary>
    [JsonPropertyName("securable_kind")]
    public SecurableKind? SecurableKind { get; set; }

    [JsonPropertyName("securable_type")]
    public string SecurableType { get; set; } = "CONNECTION";

    /// <summary>
    /// Status of an asynchronously provisioned resource.
    /// </summary>
    [JsonPropertyName("provisioning_info")]
    public ProvisioningInfo ProvisioningInfo { get; set; }
}

public enum ConnectionType
{
    MYSQL,
    POSTGRESQL,
    SNOWFLAKE,
    REDSHIFT,
    SQLDW,
    SQLSERVER,
    DATABRICKS
}

public enum SecurableKind
{
    CONNECTION_BIGQUERY,
    CONNECTION_MYSQL,
    CONNECTION_POSTGRESQL,
    CONNECTION_SNOWFLAKE,
    CONNECTION_REDSHIFT,
    CONNECTION_SQLSERVER,
    CONNECTION_SQLDW,
    CONNECTION_DATABRICKS,
    CONNECTION_ONLINE_CATALOG
}

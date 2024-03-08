using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Catalog : CatalogAttributes
{
    /// <summary>
    /// Username of current owner of catalog.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// Unique identifier of parent metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// Time at which this catalog was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of catalog creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Time at which this catalog was last modified, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of user who last modified catalog.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// The type of the catalog.
    /// </summary>
    [JsonPropertyName("catalog_type")]
    public CatalogType? CatalogType { get; set; }

    /// <summary>
    /// Storage Location URL (full path) for managed tables within catalog.
    /// </summary>
    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }

    /// <summary>
    /// Whether the current securable is accessible from all workspaces or a specific set of workspaces.
    /// </summary>
    [JsonPropertyName("isolation_mode")]
    public IsolationMode? IsolationMode { get; set; }

    /// <summary>
    /// The full name of the catalog. Corresponds with the name field.
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    /// <summary>
    /// Kind of catalog securable.
    /// </summary>
    [JsonPropertyName("securable_kind")]
    public CatalogSecurableKind? CatalogSecurableKind { get; set; }

    /// <summary>
    /// Default: "CATALOG"
    /// </summary>
    [JsonPropertyName("securable_type")]
    public string SecurableType { get; set; } = "CATALOG";

    /// <summary>
    /// Status of an asynchronously provisioned resource.
    /// </summary>
    [JsonPropertyName("provisioning_info")]
    public ProvisioningInfo ProvisioningInfo { get; set; }

    /// <summary>
    /// Indicate whether or not the catalog info contains only browsable metadata.
    /// </summary>
    [JsonPropertyName("browse_only")]
    public bool BrowseOnly { get; set; }
}

public enum CatalogType
{
    MANAGED_CATALOG,
    DELTASHARING_CATALOG,
    SYSTEM_CATALOG,
    INTERNAL_CATALOG
}

public enum IsolationMode
{
    OPEN,
    ISOLATED
}

public enum CatalogSecurableKind
{
    CATALOG_STANDARD,
    CATALOG_INTERNAL,
    CATALOG_DELTASHARING,
    CATALOG_SYSTEM,
    CATALOG_SYSTEM_DELTASHARING,
    CATALOG_FOREIGN_BIGQUERY,
    CATALOG_FOREIGN_MYSQL,
    CATALOG_FOREIGN_POSTGRESQL,
    CATALOG_FOREIGN_SQLDW,
    CATALOG_FOREIGN_REDSHIFT,
    CATALOG_FOREIGN_SNOWFLAKE,
    CATALOG_FOREIGN_SQLSERVER,
    CATALOG_FOREIGN_DATABRICKS,
    CATALOG_ONLINE,
    CATALOG_ONLINE_INDEX
}
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Metastore
{
    /// <summary>
    /// Unique identifier of the metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// The user-specified name of the metastore.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// [Deprecated] Unique identifier of the metastore's (Default) Data Access Configuration.
    /// </summary>
    [JsonPropertyName("default_data_access_config_id")]
    public string DefaultDataAccessConfigId { get; set; }

    /// <summary>
    /// UUID of storage credential to access the metastore storage_root.
    /// </summary>
    [JsonPropertyName("storage_root_credential_id")]
    public string StorageRootCredentialId { get; set; }

    /// <summary>
    /// Cloud vendor of the metastore home shard (e.g., aws, azure, gcp).
    /// </summary>
    [JsonPropertyName("cloud")]
    public string Cloud { get; set; }

    /// <summary>
    /// Cloud region which the metastore serves (e.g., us-west-2, westus).
    /// </summary>
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// Globally unique metastore ID across clouds and regions, of the form cloud:region:metastore_id.
    /// </summary>
    [JsonPropertyName("global_metastore_id")]
    public string GlobalMetastoreId { get; set; }

    /// <summary>
    /// Name of the storage credential to access the metastore storage_root.
    /// </summary>
    [JsonPropertyName("storage_root_credential_name")]
    public string StorageRootCredentialName { get; set; }

    /// <summary>
    /// Privilege model version of the metastore, of the form major.minor (e.g., 1.0).
    /// </summary>
    [JsonPropertyName("privilege_model_version")]
    public string PrivilegeModelVersion { get; set; }

    /// <summary>
    /// The scope of Delta Sharing enabled for the metastore.
    /// </summary>
    [JsonPropertyName("delta_sharing_scope")]
    public DeltaSharingScope? DeltaSharingScope { get; set; }

    /// <summary>
    /// The lifetime of delta sharing recipient token in seconds.
    /// </summary>
    [JsonPropertyName("delta_sharing_recipient_token_lifetime_in_seconds")]
    public long DeltaSharingRecipientTokenLifetimeInSeconds { get; set; }

    /// <summary>
    /// The organization name of a Delta Sharing entity, to be used in Databricks-to-Databricks Delta Sharing as the official name.
    /// </summary>
    [JsonPropertyName("delta_sharing_organization_name")]
    public string DeltaSharingOrganizationName { get; set; }

    /// <summary>
    /// The storage root URL for metastore.
    /// </summary>
    [JsonPropertyName("storage_root")]
    public string StorageRoot { get; set; }

    /// <summary>
    /// The owner of the metastore.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// Time at which this metastore was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of metastore creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Time at which the metastore was last modified, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of the user who last modified the metastore.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }
}

public record MetastoreAssignment
{
    /// <summary>
    /// The unique ID of the metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// The unique ID of the Azure Databricks workspace.
    /// </summary>
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }

    /// <summary>
    /// The name of the default catalog in the metastore.
    /// </summary>
    [JsonPropertyName("default_catalog_name")]
    public string DefaultCatalogName { get; set; }
}

public enum DeltaSharingScope
{
    INTERNAL,
    INTERNAL_AND_EXTERNAL
}
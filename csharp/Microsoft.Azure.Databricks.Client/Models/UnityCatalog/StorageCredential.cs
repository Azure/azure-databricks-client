using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record StorageCredentialAttributes
{
    /// <summary>
    /// The credential name. The name must be unique within the metastore.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; }

    /// <summary>
    /// Comment associated with the credential.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; init; }

    /// <summary>
    /// Whether the storage credential is only usable for read operations.
    /// </summary>
    [JsonPropertyName("read_only")]
    public bool ReadOnly { get; init; }

    /// <summary>
    /// The Azure service principal configuration.
    /// </summary>
    [JsonPropertyName("azure_service_principal")]
    public AzureServicePrincipal AzureServicePrincipal { get; init; }

    /// <summary>
    /// The Azure managed identity configuration.
    /// </summary>
    [JsonPropertyName("azure_managed_identity")]
    public AzureManagedIdentity AzureManagedIdentity { get; init; }

    /// <summary>
    /// Username of the current owner of the credential.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; init; }
}

public record StorageCredential : StorageCredentialAttributes
{
    /// <summary>
    /// The unique identifier of the credential.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; }

    /// <summary>
    /// Unique identifier of the parent metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; init; }

    /// <summary>
    /// Time at which this credential was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>
    /// Username of the credential creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; init; }

    /// <summary>
    /// Time at which this credential was last modified, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>
    /// Username of the user who last modified the credential.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; init; }

    /// <summary>
    /// Whether this credential is the current metastore's root storage credential.
    /// </summary>
    [JsonPropertyName("used_for_managed_storage")]
    public bool UsedForManagedStorage { get; init; }
}

public record AzureManagedIdentity
{
    /// <summary>
    /// The Azure resource ID of the Azure Databricks Access Connector. 
    /// Use the format /subscriptions/{guid}/resourceGroups/{rg-name}/providers/Microsoft.Databricks/accessConnectors/{connector-name}
    /// </summary>
    [JsonPropertyName("access_connector_id")]
    public string AccessConnectorId { get; set; }

    /// <summary>
    /// The Azure resource ID of the managed identity. 
    /// Use the format /subscriptions/{guid}/resourceGroups/{rg-name}/providers/Microsoft.ManagedIdentity/userAssignedIdentities/{identity-name}. 
    /// This is only available for user-assgined identities. For system-assigned identities, the access_connector_id is used to identify the identity. 
    /// If this field is not provided, then we assume the AzureManagedIdentity is for a system-assigned identity.
    /// </summary>
    [JsonPropertyName("managed_identity_id")]
    public string ManagedIdentityId { get; set; }

    /// <summary>
    /// The Azure Databricks internal ID that represents this managed identity.
    /// </summary>
    [JsonPropertyName("credential_id")]
    public string CredentialId { get; set; }
}

public record AzureServicePrincipal
{
    /// <summary>
    /// The directory ID corresponding to the Azure Active Directory (AAD) tenant of the application.
    /// </summary>
    [JsonPropertyName("directory_id")]
    public string DirectoryId { get; set; }

    /// <summary>
    /// The application ID of the application registration within the referenced AAD tenant.
    /// </summary>
    [JsonPropertyName("application_id")]
    public string ApplicationId { get; set; }

    /// <summary>
    /// The client secret generated for the above app ID in AAD.
    /// </summary>
    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }
}
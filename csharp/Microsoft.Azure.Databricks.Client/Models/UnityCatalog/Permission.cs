using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Permission
{
    /// <summary>
    /// The principal (user email address or group name).
    /// </summary>
    [JsonPropertyName("principal")]
    public string Principal { get; set; }

    /// <summary>
    /// The privileges assigned to the principal.
    /// </summary>
    [JsonPropertyName("privileges")]
    public IEnumerable<Privilege> Privileges { get; set; }
}

public record EffectivePermission
{
    /// <summary>
    /// The principal (user email address or group name).
    /// </summary>
    [JsonPropertyName("principal")]
    public string Principal { get; set; }

    /// <summary>
    /// The privileges assigned to the principal.
    /// </summary>
    [JsonPropertyName("privileges")]
    public IEnumerable<PrivilegeObject> Privileges { get; set; }
}

public record PrivilegeObject
{
    /// <summary>
    /// The privileges assigned to the principal.
    /// </summary>
    [JsonPropertyName("privilege")]
    public Privilege? Privileges { get; set; }

    /// <summary>
    /// The type of the object that conveys this privilege via inheritance. 
    /// This field is omitted when privilege is not inherited (it's assigned to the securable itself).
    /// </summary>
    [JsonPropertyName("inherited_from_type")]
    public SecurableType? InheritedFromType { get; set; }

    /// <summary>
    /// The full name of the object that conveys this privilege via inheritance. 
    /// This field is omitted when privilege is not inherited (it's assigned to the securable itself).
    /// </summary>
    [JsonPropertyName("inherited_from_name")]
    public string InheritedFromName { get; set; }
}

public record PermissionsUpdate
{
    /// <summary>
    /// The principal whose privileges we are changing.
    /// </summary>
    [JsonPropertyName("principal")]
    public string Principal { get; set; }

    /// <summary>
    /// The set of privileges to add.
    /// </summary>
    [JsonPropertyName("add")]
    public IEnumerable<Privilege> Add { get; set; }

    /// <summary>
    /// The set of privileges to remove.
    /// </summary>
    [JsonPropertyName("remove")]
    public IEnumerable<Privilege> Remove { get; set; }
}

public enum Privilege
{
    READ_PRIVATE_FILES,
    WRITE_PRIVATE_FILES,
    CREATE,
    USAGE,
    USE_CATALOG,
    USE_SCHEMA,
    CREATE_SCHEMA,
    CREATE_VIEW,
    CREATE_EXTERNAL_TABLE,
    CREATE_MATERIALIZED_VIEW,
    CREATE_FUNCTION,
    CREATE_MODEL,
    CREATE_CATALOG,
    CREATE_MANAGED_STORAGE,
    CREATE_EXTERNAL_LOCATION,
    CREATE_STORAGE_CREDENTIAL,
    CREATE_SHARE,
    CREATE_RECIPIENT,
    CREATE_PROVIDER,
    USE_SHARE,
    USE_RECIPIENT,
    USE_PROVIDER,
    USE_MARKETPLACE_ASSETS,
    SET_SHARE_PERMISSION,
    SELECT,
    MODIFY,
    REFRESH,
    EXECUTE,
    READ_FILES,
    WRITE_FILES,
    CREATE_TABLE,
    ALL_PRIVILEGES,
    CREATE_CONNECTION,
    USE_CONNECTION,
    APPLY_TAG,
    CREATE_FOREIGN_CATALOG,
    MANAGE_ALLOWLIST
}

public enum SecurableType
{
    catalog,
    schema,
    table,
    storage_credential,
    external_location,
    function,
    share,
    provider,
    recipient,
    metastore,
    pipeline,
    volume,
    connection
}

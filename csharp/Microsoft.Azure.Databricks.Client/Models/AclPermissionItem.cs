// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// An abstract item representing an ACL rule for an object. To be used with the permissions API
/// </summary>
public abstract record AclPermissionItem
{
    /// <summary>
    /// The principal to which the permission is applied. This field is required.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual string Principal { get; set; }

    /// <summary>
    /// The permission level applied to the principal. This field is required.
    /// </summary>

    [JsonPropertyName("permission_level")]
    public virtual PermissionLevel PermissionLevel { get; set; }

    /// <summary>
    /// Specifies whether the permission is inherited from a parent ACL rather than set explicitly. See related property <seealso cref="InheritedFromObject"/>.
    /// </summary>
    [JsonPropertyName("inherited")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Inherited { get; set; }

    /// <summary>
    /// The list of parent ACL object IDs that contribute to inherited permission on an ACL object. This is only defined if related property <seealso cref="Inherited"/> is set to true.
    /// </summary>
    [JsonPropertyName("inherited_from_object")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string> InheritedFromObject { get; set; }
}

/// <summary>
/// An item representing an ACL rule applied to a specific group. To be used when updated permission levels through the permissions Api
/// </summary>
public record GroupAclItem : AclPermissionItem
{
    [JsonPropertyName("group_name")]
    public override string Principal
    {
        get => base.Principal;
        set => base.Principal = value;
    }

    [JsonPropertyName("permission_level")]
    public override PermissionLevel PermissionLevel
    {
        get => base.PermissionLevel;
        set
        {
            if (value == PermissionLevel.IS_OWNER)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "PermissionLevel for GroupAclItem cannot be IS_OWNER.");
            }

            base.PermissionLevel = value;
        }
    }
}

/// <summary>
/// An item representing an ACL rule applied to a specific service principal. To be used when updated permission levels through the permissions Api
/// </summary>
public record ServicePrincipalAclItem : AclPermissionItem
{
    [JsonPropertyName("service_principal_name")]
    public override string Principal
    {
        get => base.Principal;
        set => base.Principal = value;
    }
}

/// <summary>
/// An item representing an ACL rule applied to a specific user. To be used when updated permission levels through the permissions Api
/// </summary>
public record UserAclItem : AclPermissionItem
{
    [JsonPropertyName("user_name")]
    public override string Principal
    {
        get => base.Principal;
        set => base.Principal = value;
    }
}


/// <summary>
/// An item representing an ACL rule applied to the given principal (user or group) on the associated scope point.
/// Kept for V1 Secrets API.
/// </summary>
public record AclPermissionItemV1
{
    /// <summary>
    /// The principal to which the permission is applied. This field is required.
    /// </summary>
    [JsonPropertyName("principal")]
    public string Principal { get; set; }

    /// <summary>
    /// The permission level applied to the principal. This field is required.
    /// </summary>
    [JsonPropertyName("permission")]
    public PermissionLevelV1 Permission { get; set; }
}

/// <summary>
/// Kept for V1 Secrets API
/// </summary>
public enum PermissionLevelV1
{
    /// <summary>
    /// Allowed to perform read operations (get, list) on secrets in this scope.
    /// </summary>
    READ,

    /// <summary>
    /// Allowed to read and write secrets to this secret scope.
    /// </summary>
    WRITE,

    /// <summary>
    /// Allowed to read/write ACLs, and read/write secrets to this secret scope.
    /// </summary>
    MANAGE
}
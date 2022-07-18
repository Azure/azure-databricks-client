// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters;

public class AclPermissionItemConverter : JsonConverter<AclPermissionItem>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(AclPermissionItem).IsAssignableFrom(typeToConvert);
    }

    private static readonly JsonSerializerOptions EnumOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public override bool HandleNull => true;

    public override AclPermissionItem Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var aclItemNode = JsonNode.Parse(ref reader)!.AsObject();

        AclPermissionItem aclItem;

        if (aclItemNode.TryGetPropertyValue("user_name", out var userName))
        {
            aclItem = new UserAclItem
            {
                Principal = userName!.GetValue<string>()
            };
        }
        else if (aclItemNode.TryGetPropertyValue("group_name", out var groupName))
        {
            aclItem = new GroupAclItem
            {
                Principal = groupName!.GetValue<string>()
            };
        }
        else if (aclItemNode.TryGetPropertyValue("service_principal_name", out var servicePrincipalName))
        {
            aclItem = new ServicePrincipalAclItem
            {
                Principal = servicePrincipalName!.GetValue<string>()
            };
        }
        else
        {
            throw new NotImplementedException(
                $"JsonConverter not implemented for node {aclItemNode.ToJsonString()}");
        }

        if (aclItemNode.ContainsKey("all_permissions"))
        {
            var permissionNode = aclItemNode["all_permissions"]![0]!.AsObject();
            aclItem.PermissionLevel = permissionNode["permission_level"]!.Deserialize<PermissionLevel>(options);
            aclItem.Inherited = permissionNode.TryGetPropertyValue("inherited", out var inherited) &&
                                inherited!.GetValue<bool>();
            aclItem.InheritedFromObject =
                permissionNode.TryGetPropertyValue("inherited_from_object", out var inheritedFrom)
                    ? inheritedFrom!.Deserialize<IEnumerable<string>>(options)
                    : Enumerable.Empty<string>();
        }
        else
        {
            aclItem.PermissionLevel = aclItemNode["permission_level"]!.Deserialize<PermissionLevel>(options);
        }

        return aclItem;
    }

    public override void Write(Utf8JsonWriter writer, AclPermissionItem value, JsonSerializerOptions options)
    {
        var node = value switch
        {
            UserAclItem user => JsonSerializer.SerializeToNode(user, EnumOptions),
            GroupAclItem group => JsonSerializer.SerializeToNode(group, EnumOptions),
            ServicePrincipalAclItem sp => JsonSerializer.SerializeToNode(sp, EnumOptions),
            _ => throw new NotImplementedException($"JsonConverter not implemented for type {value.GetType()}")
        };

        node!.WriteTo(writer);
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The ACL permission levels for use with the permissions api. The 'GetPermissionLevels' methods
/// can be used to determine which levels are appropriate to which method. 
/// </summary>
public enum PermissionLevel
{
    CAN_USE,
    CAN_MANAGE,
    CAN_ATTACH_TO,
    CAN_RESTART,
    CAN_MANAGE_RUN,
    IS_OWNER,
    CAN_RUN,
    CAN_READ,
    CAN_EDIT,
    CAN_MANAGE_STAGING_VERSIONS,
    CAN_MANAGE_PRODUCTION_VERSIONS,
    CAN_VIEW
}

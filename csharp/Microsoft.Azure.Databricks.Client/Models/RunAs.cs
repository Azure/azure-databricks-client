// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public sealed record RunAs
{
    /// <summary>
    /// The email of an active workspace user. Non-admin users can only set this field to their own email.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    /// <summary>
    /// Application ID of an active service principal. Setting this field requires the servicePrincipal/user role.
    /// </summary>
    [JsonPropertyName("service_principal_name")]
    public string ServicePrincipalName { get; set; }
}
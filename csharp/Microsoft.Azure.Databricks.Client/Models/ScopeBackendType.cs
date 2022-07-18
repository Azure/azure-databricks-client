﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// The types of secret scope backends in the Secret Manager. Only Databricks backed secret scope is supported.
/// </summary>
public enum ScopeBackendType
{
    /// <summary>
    /// A secret scope in which secrets are stored in an Azure Key Vault.
    /// </summary>
    AZURE_KEYVAULT,

    /// <summary>
    /// A secret scope in which secrets are stored in Databricks managed storage and encrypted with a cloud-based specific encryption key.
    /// </summary>
    DATABRICKS
}
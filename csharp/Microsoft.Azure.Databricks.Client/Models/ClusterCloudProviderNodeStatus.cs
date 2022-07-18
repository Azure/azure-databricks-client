// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Azure.Databricks.Client.Models;

public enum ClusterCloudProviderNodeStatus
{
    /// <summary>
    /// Node type not available for subscription.
    /// </summary>
    NotEnabledOnSubscription,

    /// <summary>
    /// Node type not available in region.
    /// </summary>
    NotAvailableInRegion
}
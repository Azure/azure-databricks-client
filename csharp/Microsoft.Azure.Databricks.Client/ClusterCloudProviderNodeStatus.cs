namespace Microsoft.Azure.Databricks.Client
{
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
}
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public record InstancePoolAzureAttributes
    {
        /// <summary>
        /// Availability type used for all subsequent nodes past the `first_on_demand` ones.
        /// </summary>
        [JsonPropertyName("availability")]
        public AzureAvailability? Availability { get; set; }

        /// <summary>
        /// The max bid price used for Azure spot instances. You can set this to greater than or equal to the current spot price. You can also set this to -1 (the default),
        /// which specifies that the instance cannot be evicted on the basis of price.
        /// The price for the instance will be the current price for spot instances or the price for a standard instance.
        /// You can view historical pricing and eviction rates in the Azure portal.
        /// </summary>
        [JsonPropertyName("spot_bid_max_price")]
        public double SpotBidMaxPrice { get; set; }
    }

    public record AzureAttributes : InstancePoolAzureAttributes
    {
        /// <summary>
        /// The first `first_on_demand` nodes of the cluster will be placed on on-demand instances.
        /// This value must be greater than 0, or else cluster creation validation fails.
        /// If this value is greater than or equal to the current cluster size, all nodes will be placed on on-demand instances.
        /// If this value is less than the current cluster size, `first_on_demand` nodes will be placed on on-demand instances and the remainder will be placed on availability instances.
        /// This value does not affect cluster size and cannot be mutated over the lifetime of a cluster.
        /// </summary>
        [JsonPropertyName("first_on_demand")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int FirstOnDemand { get; set; }
    }

    /// <summary>
    /// The Azure instance availability type behavior.
    /// </summary>
    public enum AzureAvailability
    {
        /// <summary>
        /// Use spot instances.
        /// </summary>
        SPOT_AZURE,

        /// <summary>
        /// Use on-demand instances.
        /// </summary>
        ON_DEMAND_AZURE,

        /// <summary>
        /// Preferably use spot instances, but fall back to on-demand instances if spot instances cannot be acquired (for example, if Azure spot prices are too high or out of quota). Does not apply to pool availability.
        /// </summary>
        SPOT_WITH_FALLBACK_AZURE
    }
}

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record ProvisioningInfo
{
    /// <summary>
    /// Enum: "STATE_UNSPECIFIED" "PROVISIONING" "ACTIVE" "FAILED" "DELETING"
    /// </summary>
    [JsonPropertyName("state")]
    public ProvisioningState? State { get; set; }
}

public enum ProvisioningState
{
    STATE_UNSPECIFIED,
    PROVISIONING,
    ACTIVE,
    FAILED,
    DELETING
}

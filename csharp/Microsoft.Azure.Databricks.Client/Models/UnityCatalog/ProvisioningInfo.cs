using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public class ProvisioningInfo
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

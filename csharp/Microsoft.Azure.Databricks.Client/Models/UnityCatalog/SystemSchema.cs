using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record SystemSchema
{
    /// <summary>
    /// Name of the system schema.
    /// </summary>
    [JsonPropertyName("schema")]
    public string Schema { get; set; }

    /// <summary>
    /// The current state of enablement for the system schema. An empty string means the system schema is available and ready for opt-in.
    /// </summary>
    [JsonPropertyName("state")]
    public SystemSchemaState State { get; set; }
}

public enum SystemSchemaState
{
    AVAILABLE,
    ENABLE_INITIALIZED,
    ENABLE_COMPLETED,
    DISABLE_INITIALIZED,
    UNAVAILABLE
}

public enum SystemSchemaName
{
    lineage,
    operational_data,
    access,
    billing
}

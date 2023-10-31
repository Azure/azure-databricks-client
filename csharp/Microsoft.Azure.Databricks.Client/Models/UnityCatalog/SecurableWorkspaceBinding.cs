using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record SecurableWorkspaceBinding
{
    [JsonPropertyName("workspace_id")]
    public long WorkspaceId { get; set; }

    [JsonPropertyName("binding_type")]
    public BindingType? BindingType { get; set; }
}

public enum BindingType
{
    BINDING_TYPE_READ_WRITE,
    BINDING_TYPE_READ_ONLY
}
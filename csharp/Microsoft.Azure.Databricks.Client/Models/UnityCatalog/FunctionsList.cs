using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record FunctionsList
{
    /// <summary>
    /// An array of function information objects.
    /// </summary>
    [JsonPropertyName("functions")]
    public IEnumerable<Function> Functions { get; set; }
}

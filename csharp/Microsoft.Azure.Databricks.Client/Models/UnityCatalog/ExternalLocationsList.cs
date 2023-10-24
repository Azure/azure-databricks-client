using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public class ExternalLocationsList
{
    /// <summary>
    /// An array of external locations.
    /// </summary>
    [JsonPropertyName("external_locations")]
    public IEnumerable<ExternalLocation> ExternalLocations { get; set; }
}

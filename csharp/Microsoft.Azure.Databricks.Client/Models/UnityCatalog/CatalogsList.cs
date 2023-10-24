using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record CatalogsList
{
    /// <summary>
    /// An array of catalog information objects.
    /// </summary>
    [JsonPropertyName("catalogs")]
    public IEnumerable<Catalog> Catalogs { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public class ConnectionsList
{
    /// <summary>
    /// An array of connection information objects.
    /// </summary>
    [JsonPropertyName("connections")]
    public IEnumerable<Connection> Connections { get; set; }
}

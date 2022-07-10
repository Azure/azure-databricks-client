using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// The status of the library on a specific cluster.
    /// </summary>
    public record LibraryFullStatus
    {
        /// <summary>
        /// Unique identifier for the library.
        /// </summary>
        [JsonPropertyName("library")]
        public Library Library { get; set; }

        /// <summary>
        /// Status of installing the library on the cluster.
        /// </summary>
        [JsonPropertyName("status")]
        public LibraryInstallStatus Status { get; set; }

        /// <summary>
        /// All the info and warning messages that have occurred so far for this library.
        /// </summary>
        [JsonPropertyName("messages")]
        public IEnumerable<string> Messages { get; set; }

        /// <summary>
        /// Whether the library was set to be installed on all clusters via the libraries UI.
        /// </summary>
        [JsonPropertyName("is_library_for_all_clusters")]
        public bool IsLibraryForAllClusters { get; set; }
    }
}
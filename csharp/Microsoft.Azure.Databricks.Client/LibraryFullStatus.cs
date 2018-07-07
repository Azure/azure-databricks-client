using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// The status of the library on a specific cluster.
    /// </summary>
    public class LibraryFullStatus
    {
        /// <summary>
        /// Unique identifier for the library.
        /// </summary>
        [JsonProperty(PropertyName = "library")]
        public Library Library { get; set; }

        /// <summary>
        /// Status of installing the library on the cluster.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public LibraryInstallStatus Status { get; set; }

        /// <summary>
        /// All the info and warning messages that have occurred so far for this library.
        /// </summary>
        [JsonProperty(PropertyName = "messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<string> Messages { get; set; }

        /// <summary>
        /// Whether the library was set to be installed on all clusters via the libraries UI.
        /// </summary>
        [JsonProperty(PropertyName = "is_library_for_all_clusters", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLibraryForAllClusters { get; set; }
    }
}
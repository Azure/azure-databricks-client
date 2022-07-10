using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// The information of the object in workspace. It will be returned by list and get-status.
    /// </summary>
    public record ObjectInfo
    {
        /// <summary>
        /// The type of the object. It could be NOTEBOOK, DIRECTORY or LIBRARY.
        /// </summary>
        [JsonPropertyName("object_type")]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// The absolute path of the object.
        /// </summary>
        [JsonPropertyName("path")]
        public string Path { get; set; }

        /// <summary>
        /// The language of the object. This value is set only if the object type is NOTEBOOK.
        /// </summary>
        [JsonPropertyName("language")]
        public Language? Language { get; set; }

        /// <summary>
        /// Unique identifier for a NOTEBOOK or DIRECTORY.
        /// </summary>
        [JsonPropertyName("object_id")]
        public long ObjectId { get; set; }
    }
}
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// The information of the object in workspace. It will be returned by list and get-status.
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>
        /// The type of the object. It could be NOTEBOOK, DIRECTORY or LIBRARY.
        /// </summary>
        [JsonProperty(PropertyName = "object_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// The absolute path of the object.
        /// </summary>
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// The language of the object. This value is set only if the object type is NOTEBOOK.
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Language? Language { get; set; }
    }
}
﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// The exported content is in HTML format. We may support other formats in the future. For example, if the view to export is dashboards, one HTML string is returned for every dashboard.
    /// </summary>
    public class ViewItem
    {
        /// <summary>
        /// Content of the view
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        /// <summary>
        /// Name of the view item. In the case of code view, it would be the notebook's name. In the case of dashboard view, it would be the dashboard's name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Type of the view item (e.g., NOTEBOOK, DASHBOARD)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ViewType Type { get; set; }
    }
}
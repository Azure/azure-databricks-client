using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// Docker image connection information.
    /// </summary>
    public class DockerImage
    {
        /// <summary>
        /// URL for the Docker image.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Basic authentication information for Docker repository.
        /// </summary>
        [JsonProperty(PropertyName = "basic_auth")]
        public DockerBasicAuth BasicAuth { get; set; }
    }

    /// <summary>
    /// Docker repository basic authentication information.
    /// </summary>
    public class DockerBasicAuth
    {
        /// <summary>
        /// User name for the Docker repository.
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        /// <summary>
        /// Password for the Docker repository.
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}

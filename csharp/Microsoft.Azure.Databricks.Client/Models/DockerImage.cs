using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// Docker image connection information.
    /// </summary>
    public record DockerImage
    {
        /// <summary>
        /// URL for the Docker image.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Basic authentication information for Docker repository.
        /// </summary>
        [JsonPropertyName("basic_auth")]
        public DockerBasicAuth BasicAuth { get; set; }
    }

    /// <summary>
    /// Docker repository basic authentication information.
    /// </summary>
    public record DockerBasicAuth
    {
        /// <summary>
        /// User name for the Docker repository.
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        /// <summary>
        /// Password for the Docker repository.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Azure.Databricks.Client
{
    public class Client
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL of the databricks portal. ex. https://southcentralus.azuredatabricks.net</param>
        /// <param name="token">The access token.</param>
        /// <param name="timeoutSeconds">The timeout in seconds for the http requests.</param>
        private Client(string baseUrl, string token, long timeoutSeconds = 30)
        {
            var apiUrl = new Uri(new Uri(baseUrl), "api/2.0/");

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = apiUrl,
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            this.Clusters = new ClustersApiClient(httpClient);
            this.Jobs = new JobsApiClient(httpClient);
            this.Dbfs = new DbfsApiClient(httpClient);
            this.Secrets = new SecretsApiClient(httpClient);
            this.Groups = new GroupsApiClient(httpClient);
            this.Libraries = new LibrariesApiClient(httpClient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="clusterApi">The cluster API implementation.</param>
        /// <param name="jobsApi">The jobs API implementation.</param>
        /// <param name="dbfsApi">The dbfs API implementation.</param>
        /// <param name="secretsApi">The secrets API implementation.</param>
        /// <param name="groupsApi">The groups API implementation.</param>
        /// <param name="librariesApi">The libraries API implementation.</param>
        private Client(IClustersApi clusterApi, IJobsApi jobsApi, IDbfsApi dbfsApi, ISecretsApi secretsApi, IGroupsApi groupsApi, ILibrariesApi librariesApi)
        {
            this.Clusters = clusterApi;
            this.Jobs = jobsApi;
            this.Dbfs = dbfsApi;
            this.Secrets = secretsApi;
            this.Groups = groupsApi;
            this.Libraries = librariesApi;
        }

        /// <summary>
        /// Create client object with specified base URL, access token and timeout.
        /// </summary>
        /// <param name="baseUrl">Base URL for the databricks resource. For example: https://southcentralus.azuredatabricks.net</param>
        /// <param name="token">The access token. To generate a token, refer to this document: https://docs.databricks.com/api/latest/authentication.html#generate-a-token </param>
        /// <param name="timeoutSeconds">Web request time out in seconds</param>
        public static Client CreateClient(string baseUrl, string token, long timeoutSeconds = 30)
        {
            return new Client(baseUrl, token, timeoutSeconds);
        }

        /// <summary>
        /// Create client object with mock implementation. This is for unit testing purpose.
        /// </summary>
        public static Client CreateClient(IClustersApi clusterApi, IJobsApi jobsApi, IDbfsApi dbfsApi, ISecretsApi secretsApi, IGroupsApi groupsApi, ILibrariesApi librariesApi)
        {
            return new Client(clusterApi, jobsApi, dbfsApi, secretsApi, groupsApi, librariesApi);
        }

        public IClustersApi Clusters { get; }

        public IJobsApi Jobs { get; }

        public IDbfsApi Dbfs { get; }

        public ISecretsApi Secrets { get; }

        public IGroupsApi Groups { get; }

        public ILibrariesApi Libraries { get; }
    }
}

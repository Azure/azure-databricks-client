using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Azure.Databricks.Client
{
    public class DatabricksClient : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabricksClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL of the databricks portal. ex. https://southcentralus.azuredatabricks.net</param>
        /// <param name="token">The access token.</param>
        /// <param name="timeoutSeconds">The timeout in seconds for the http requests.</param>
        protected DatabricksClient(string baseUrl, string token, long timeoutSeconds = 30)
        {
            var apiUrl = new Uri(new Uri(baseUrl), "api/");

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(handler, false)
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
            this.Token = new TokenApiClient(httpClient);
            this.Workspace = new WorkspaceApiClient(httpClient);
            this.InstancePool = new InstancePoolApiClient(httpClient);
            this.Permissions = new PermissionsApiClient(httpClient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabricksClient"/> class.
        /// </summary>
        /// <remarks>
        /// The AAD service principal used to acquire the token must be contributor role to the databricks workspace resource.
        /// </remarks>
        /// <param name="baseUrl">The base URL of the databricks portal. ex. https://southcentralus.azuredatabricks.net</param>
        /// <param name="workspaceResourceId">The ResourceId of the databricks workspace.</param>
        /// <param name="databricksToken">The AAD token used to access the global databricks application (2ff814a6-3304-4ab8-85cb-cd0e6f879c1d).</param>
        /// <param name="managementToken">The AAD token for Azure management API (https://management.core.windows.net/).</param>
        /// <param name="timeoutSeconds">The timeout in seconds for the http requests.</param>
        protected DatabricksClient(string baseUrl, string workspaceResourceId, string databricksToken, string managementToken,  long timeoutSeconds = 30)
        {
            var apiUrl = new Uri(new Uri(baseUrl), "api/");

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = apiUrl,
                Timeout = TimeSpan.FromSeconds(timeoutSeconds)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", databricksToken);
            httpClient.DefaultRequestHeaders.Add("X-Databricks-Azure-SP-Management-Token", managementToken);
            httpClient.DefaultRequestHeaders.Add("X-Databricks-Azure-Workspace-Resource-Id", workspaceResourceId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            this.Clusters = new ClustersApiClient(httpClient);
            this.Jobs = new JobsApiClient(httpClient);
            this.Dbfs = new DbfsApiClient(httpClient);
            this.Secrets = new SecretsApiClient(httpClient);
            this.Groups = new GroupsApiClient(httpClient);
            this.Libraries = new LibrariesApiClient(httpClient);
            this.Token = new TokenApiClient(httpClient);
            this.Workspace = new WorkspaceApiClient(httpClient);
            this.InstancePool = new InstancePoolApiClient(httpClient);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabricksClient"/> class.
        /// </summary>
        /// <param name="clusterApi">The cluster API implementation.</param>
        /// <param name="jobsApi">The jobs API implementation.</param>
        /// <param name="dbfsApi">The dbfs API implementation.</param>
        /// <param name="secretsApi">The secrets API implementation.</param>
        /// <param name="groupsApi">The groups API implementation.</param>
        /// <param name="librariesApi">The libraries API implementation.</param>
        /// <param name="tokenApi">The token API implementation.</param>
        /// <param name="workspaceApi">The workspace API implementation.</param>
        /// <param name="instancePoolApi">The instance pool API implementation.</param>
        /// <param name="permissionsApi">The permissions API implementation.</param>
        protected DatabricksClient(IClustersApi clusterApi, IJobsApi jobsApi, IDbfsApi dbfsApi, ISecretsApi secretsApi,
            IGroupsApi groupsApi, ILibrariesApi librariesApi, ITokenApi tokenApi, IWorkspaceApi workspaceApi, IInstancePoolApi instancePoolApi
            , IPermissionsApi permissionsApi)
        {
            this.Clusters = clusterApi;
            this.Jobs = jobsApi;
            this.Dbfs = dbfsApi;
            this.Secrets = secretsApi;
            this.Groups = groupsApi;
            this.Libraries = librariesApi;
            this.Token = tokenApi;
            this.Workspace = workspaceApi;
            this.InstancePool = instancePoolApi;
            this.Permissions = permissionsApi;
        }

        /// <summary>
        /// Create client object with specified base URL, access token and timeout.
        /// </summary>
        /// <param name="baseUrl">Base URL for the databricks resource. For example: https://southcentralus.azuredatabricks.net</param>
        /// <param name="token">The access token. To generate a token, refer to this document: https://docs.databricks.com/api/latest/authentication.html#generate-a-token </param>
        /// <param name="timeoutSeconds">Web request time out in seconds</param>
        public static DatabricksClient CreateClient(string baseUrl, string token, long timeoutSeconds = 30)
        {
            return new DatabricksClient(baseUrl, token, timeoutSeconds);
        }

        /// <summary>
        /// Create client object with specified base URL, workspace resourceId, databricks token, management API token and timeout.
        /// This is to support calling Databricks API with an Azure AAD app.
        /// </summary>
        /// <remarks>
        /// This feature is still in preview.
        /// The AAD service principal used to acquire the token must be contributor role to the databricks workspace resource.
        /// Request the access token via the AAD oauth2 API (v1): https://docs.microsoft.com/en-us/azure/active-directory/azuread-dev/v1-oauth2-client-creds-grant-flow#request-an-access-token
        /// </remarks>
        /// <param name="baseUrl">The base URL of the databricks portal. ex. https://southcentralus.azuredatabricks.net</param>
        /// <param name="workspaceResourceId">The ResourceId of the databricks workspace.</param>
        /// <param name="databricksToken">The AAD token used to access the global databricks application (resource to claim: "2ff814a6-3304-4ab8-85cb-cd0e6f879c1d").</param>
        /// <param name="managementToken">The AAD token for Azure management API (resource to claim: "https://management.core.windows.net/").</param>
        /// <param name="timeoutSeconds">The timeout in seconds for the http requests.</param>
        public static DatabricksClient CreateClient(string baseUrl, string workspaceResourceId, string databricksToken, string managementToken, long timeoutSeconds = 30)
        {
            return new DatabricksClient(baseUrl, workspaceResourceId, databricksToken, managementToken, timeoutSeconds);
        }

        /// <summary>
        /// Create client object with mock implementation. This is for unit testing purpose.
        /// </summary>
        public static DatabricksClient CreateClient(IClustersApi clusterApi, IJobsApi jobsApi, IDbfsApi dbfsApi,
            ISecretsApi secretsApi, IGroupsApi groupsApi, ILibrariesApi librariesApi, ITokenApi tokenApi,
            IWorkspaceApi workspaceApi, IInstancePoolApi instancePoolApi, IPermissionsApi permissionsApi)
        {
            return new DatabricksClient(clusterApi, jobsApi, dbfsApi, secretsApi, groupsApi, librariesApi, tokenApi,
                workspaceApi, instancePoolApi, permissionsApi);
        }

        public IClustersApi Clusters { get; }

        public IJobsApi Jobs { get; }

        public IDbfsApi Dbfs { get; }

        public ISecretsApi Secrets { get; }

        public IGroupsApi Groups { get; }

        public ILibrariesApi Libraries { get; }

        public ITokenApi Token { get; }

        public IWorkspaceApi Workspace { get; }

        public IInstancePoolApi InstancePool { get; }

        public IPermissionsApi Permissions { get; }

        public void Dispose()
        {
            Clusters.Dispose();
            Jobs.Dispose();
            Dbfs.Dispose();
            Secrets.Dispose();
            Groups.Dispose();
            Libraries.Dispose();
            Token.Dispose();
            Workspace.Dispose();
            InstancePool.Dispose();
        }
    }
}

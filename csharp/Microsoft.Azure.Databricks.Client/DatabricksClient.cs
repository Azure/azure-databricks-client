// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace Microsoft.Azure.Databricks.Client;

public class DatabricksClient : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabricksClient"/> class.
    /// </summary>
    private DatabricksClient(HttpClient httpClient)
    {
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
        this.ClusterPolicies = new ClusterPoliciesApiClient(httpClient);
        this.GlobalInitScriptsApi = new GlobalInitScriptsApi(httpClient);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabricksClient"/> class.
    /// </summary>
    /// <param name="baseUrl">The base URL of the databricks portal. ex. https://southcentralus.azuredatabricks.net</param>
    /// <param name="token">The access token.</param>
    /// <param name="timeoutSeconds">The timeout in seconds for the http requests.</param>
    /// <param name="httpClientConfig">A custom function to configure the HttpClient object.</param>
    protected DatabricksClient(string baseUrl, string token, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
        : this(CreateHttpClient(baseUrl, token, timeoutSeconds, httpClientConfig))
    {
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
    /// <param name="httpClientConfig">A custom function to configure the HttpClient object.</param>
    protected DatabricksClient(string baseUrl, string workspaceResourceId, string databricksToken,
        string managementToken, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
        : this(CreateHttpClient(baseUrl, workspaceResourceId, databricksToken, managementToken, timeoutSeconds, httpClientConfig))
    {
    }

    private static HttpClient CreateHttpClient(string baseUrl, string beareToken, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
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

        httpClientConfig?.Invoke(httpClient);

        SetDefaultHttpHeaders(httpClient);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", beareToken);
        return httpClient;
    }

    private static HttpClient CreateHttpClient(string baseUrl, string workspaceResourceId, string databricksToken,
        string managementToken, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
    {
        var httpClient = CreateHttpClient(baseUrl, databricksToken, timeoutSeconds, httpClientConfig);
        httpClient.DefaultRequestHeaders.Add("X-Databricks-Azure-SP-Management-Token", managementToken);
        httpClient.DefaultRequestHeaders.Add("X-Databricks-Azure-Workspace-Resource-Id", workspaceResourceId);
        return httpClient;
    }

    private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

    private static void SetDefaultHttpHeaders(HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Microsoft.Azure.Databricks.Client",
            Version));
    }

    /// <summary>
    /// Create client object with specified base URL, access token and timeout.
    /// </summary>
    /// <param name="baseUrl">Base URL for the databricks resource. For example: https://southcentralus.azuredatabricks.net</param>
    /// <param name="token">The access token. To generate a token, refer to this document: https://docs.databricks.com/api/latest/authentication.html#generate-a-token </param>
    /// <param name="timeoutSeconds">Web request time out in seconds</param>
    /// <param name="httpClientConfig">A custom function to configure the HttpClient object.</param>
    public static DatabricksClient CreateClient(string baseUrl, string token, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
    {
        return new DatabricksClient(baseUrl, token, timeoutSeconds, httpClientConfig);
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
    /// <param name="httpClientConfig">A custom function to configure the HttpClient object.</param>
    public static DatabricksClient CreateClient(string baseUrl, string workspaceResourceId, string databricksToken, string managementToken, long timeoutSeconds = 30,
        Action<HttpClient> httpClientConfig = default)
    {
        return new DatabricksClient(baseUrl, workspaceResourceId, databricksToken, managementToken, timeoutSeconds, httpClientConfig);
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

    public IClusterPoliciesApi ClusterPolicies { get; }

    public IGlobalInitScriptsApi GlobalInitScriptsApi { get; }

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
        ClusterPolicies.Dispose();
        GlobalInitScriptsApi.Dispose();
        GC.SuppressFinalize(this);
    }
}
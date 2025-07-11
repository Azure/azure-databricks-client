﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

using Azure.Core;

namespace Microsoft.Azure.Databricks.Client;

public partial class DatabricksClient : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatabricksClient"/> class.
    /// </summary>
    private DatabricksClient(HttpClient httpClient)
    {
        this.Clusters = new ClustersApiClient(httpClient);
        this.Jobs = new JobsApiClient(httpClient);
        this.Dbfs = new DbfsApiClient(httpClient);
        this.Files = new FilesApiClient(httpClient);
        this.Secrets = new SecretsApiClient(httpClient);
        this.Groups = new GroupsApiClient(httpClient);
        this.Libraries = new LibrariesApiClient(httpClient);
        this.Token = new TokenApiClient(httpClient);
        this.Workspace = new WorkspaceApiClient(httpClient);
        this.InstancePool = new InstancePoolApiClient(httpClient);
        this.Permissions = new PermissionsApiClient(httpClient);
        this.ClusterPolicies = new ClusterPoliciesApiClient(httpClient);
        this.GlobalInitScriptsApi = new GlobalInitScriptsApi(httpClient);
        this.SQL = new SQLApiClient(httpClient);
        this.Repos = new ReposApiClient(httpClient);
        this.Pipelines = new PipelinesApiClient(httpClient);
        this.UnityCatalog = new UnityCatalogClient(httpClient);
        this.MachineLearning = new MachineLearningClient(httpClient);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabricksClient"/> class for mocking.
    /// </summary>
    protected DatabricksClient()
    {
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

        var httpClient = new HttpClient(handler, false)
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

    private static HttpClient CreateHttpClient(string baseUrl, Func<Task<string>> getToken, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
    {
        var apiUrl = new Uri(new Uri(baseUrl), "api/");

        var decompressHandler = new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };

        var bearerHeaderHandler = new BearerHeaderHandler(getToken, decompressHandler);

        var httpClient = new HttpClient(bearerHeaderHandler, false)
        {
            BaseAddress = apiUrl,
            Timeout = TimeSpan.FromSeconds(timeoutSeconds)
        };

        httpClientConfig?.Invoke(httpClient);

        SetDefaultHttpHeaders(httpClient);
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

    /// <summary>
    /// Create client object with specified base URL, a TokenCredential object and timeout value.
    /// </summary>
    public static DatabricksClient CreateClient(string baseUrl, TokenCredential credential, long timeoutSeconds = 30, Action<HttpClient> httpClientConfig = default)
    {
        const string DatabricksScope = "2ff814a6-3304-4ab8-85cb-cd0e6f879c1d/.default";
        var httpClient = CreateHttpClient(
            baseUrl,
            async () => (await credential.GetTokenAsync(new TokenRequestContext(new string[] { DatabricksScope }), default)).Token,
            timeoutSeconds,
            httpClientConfig
        );
        return new DatabricksClient(httpClient);
    }

    public virtual IClustersApi Clusters { get; }

    public virtual IJobsApi Jobs { get; }

    public virtual IDbfsApi Dbfs { get; }

    public virtual IFilesApi Files { get; }

    public virtual ISecretsApi Secrets { get; }

    public virtual IGroupsApi Groups { get; }

    public virtual ILibrariesApi Libraries { get; }

    public virtual ITokenApi Token { get; }

    public virtual IWorkspaceApi Workspace { get; }

    public virtual IInstancePoolApi InstancePool { get; }

    public virtual IPermissionsApi Permissions { get; }

    public virtual IClusterPoliciesApi ClusterPolicies { get; }

    public virtual IGlobalInitScriptsApi GlobalInitScriptsApi { get; }

    public virtual ISQLApi SQL { get; }

    public virtual IReposApi Repos { get; }

    public virtual IPipelinesApi Pipelines { get; }

    public virtual UnityCatalogClient UnityCatalog { get; }

    public virtual MachineLearningClient MachineLearning { get; }

    public void Dispose()
    {
        Clusters?.Dispose();
        Jobs?.Dispose();
        Dbfs?.Dispose();
        Files?.Dispose();
        Secrets?.Dispose();
        Groups?.Dispose();
        Libraries?.Dispose();
        Token?.Dispose();
        Workspace?.Dispose();
        InstancePool?.Dispose();
        Permissions?.Dispose();
        ClusterPolicies?.Dispose();
        GlobalInitScriptsApi?.Dispose();
        SQL?.Dispose();
        Repos?.Dispose();
        Pipelines?.Dispose();
        UnityCatalog?.Dispose();
        MachineLearning?.Dispose();
        GC.SuppressFinalize(this);
    }
}

// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Databricks.Client.Converters;

namespace Microsoft.Azure.Databricks.Client;

public abstract class ApiClient : IDisposable
{
    protected readonly HttpClient HttpClient;

    protected virtual string ApiVersion => "2.0";

    protected string BaseUnityCatalogUri => "2.1/unity-catalog";

    protected string BaseMLFlowApiUri => "2.0/mlflow";

    protected static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        PropertyNameCaseInsensitive = true,
        Converters = {
            new JsonStringEnumConverter(),
            new MillisecondEpochDateTimeConverter(),
            new LibraryConverter(),
            new SecretScopeConverter(),
            new AclPermissionItemConverter(),
            new DepedencyConverter(),
            new TableConstraintConverter(),
        }
    };

    protected ApiClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected static ClientApiException CreateApiException(HttpResponseMessage response)
    {
        var statusCode = response.StatusCode;
        var errorContent = response.Content.ReadAsStringAsync().Result;
        return new ClientApiException(errorContent, statusCode);
    }

    protected static async Task<TResult?> SendRequest<TResult>(HttpClient httpClient, HttpMethod method, string requestUri, HttpContent? content, JsonTypeInfo<TResult> typeInfo, CancellationToken cancellationToken = default)
    {
        using var response = await FetchResponse(httpClient, method, requestUri, content, cancellationToken).ConfigureAwait(false);
        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync(responseStream, typeInfo, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    private static async Task<TResult?> SendRequest<TBody, TResult>(HttpClient httpClient, HttpMethod method, string requestUri, TBody? body, CancellationToken cancellationToken = default)
    {
        using var response = await FetchResponse(httpClient, method, requestUri, body, cancellationToken).ConfigureAwait(false);
        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync<TResult>(responseStream, Options, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    private static async Task SendRequest<TBody>(HttpClient httpClient, HttpMethod method, string requestUri, TBody? body, CancellationToken cancellationToken = default)
    {
        await FetchResponse(httpClient, method, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    private static async Task<HttpContentHeaders> SendHeadRequest<TBody>(HttpClient httpClient, HttpMethod method,
        string requestUri, TBody? body = default, CancellationToken cancellationToken = default)
    {
        using var response = await FetchResponse(httpClient, method, requestUri, body, cancellationToken).ConfigureAwait(false);
        return response.Content.Headers;
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    private static async Task<HttpResponseMessage> FetchResponse<TBody>(HttpClient httpClient, HttpMethod method,
        string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(method, requestUri)
        {
            Content = body == null ? null : new StringContent(JsonSerializer.Serialize(body, Options), Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        return response;
    }

    private static async Task<HttpResponseMessage> FetchResponse(HttpClient httpClient, HttpMethod method,
        string requestUri, HttpContent? content, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(method, requestUri)
        {
            Content = content,
        };

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        return response;
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task<T?> HttpGet<T>(HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
    {
        return await SendRequest<object, T>(httpClient, HttpMethod.Get, requestUri, null, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task HttpPost<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        await SendRequest(httpClient, HttpMethod.Post, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task<TResult?> HttpPost<TBody, TResult>(HttpClient httpClient, string requestUri,
        TBody body, CancellationToken cancellationToken = default)
    {
        return await SendRequest<TBody, TResult>(httpClient, HttpMethod.Post, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task HttpPatch<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        await SendRequest(httpClient, HttpMethod.Patch, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task<TResult?> HttpPatch<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        return await SendRequest<TBody, TResult>(httpClient, HttpMethod.Patch, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task<TResult?> HttpPut<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        return await SendRequest<TBody, TResult>(httpClient, HttpMethod.Put, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task HttpPut<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        await SendRequest(httpClient, HttpMethod.Put, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task HttpDelete(HttpClient httpClient, string requestUri,
        CancellationToken cancellationToken = default)
    {
        await SendRequest<object>(httpClient, HttpMethod.Delete, requestUri, null, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task<HttpContentHeaders> HttpHead<TBody>(HttpClient httpClient, string requestUri, TBody body = default,
        CancellationToken cancellationToken = default)
    {
        return await SendHeadRequest(httpClient, HttpMethod.Head, requestUri, body, cancellationToken).ConfigureAwait(false);
    }

    [RequiresUnreferencedCode("Calls System.Text.Json.JsonSerializer.DeserializeAsync<TValue>(Stream, JsonSerializerOptions, CancellationToken)")]
    protected static async Task<HttpContentHeaders> HttpHead(HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
    {
        return await SendHeadRequest<object>(httpClient, HttpMethod.Head, requestUri, null, cancellationToken).ConfigureAwait(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            HttpClient?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

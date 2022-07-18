// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Converters;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public abstract class ApiClient : IDisposable
{
    protected readonly HttpClient HttpClient;

    protected virtual string ApiVersion => "2.0";

    protected static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        PropertyNameCaseInsensitive = true,
        Converters = {
            new JsonStringEnumConverter(),
            new MillisecondEpochDateTimeConverter(),
            new LibraryConverter(),
            new SecretScopeConverter(),
            new AclPermissionItemConverter()
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

    protected static async Task<T> HttpGet<T>(HttpClient httpClient, string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        var respContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<T>(respContent, Options);
    }

    protected static async Task HttpPost<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {

        HttpContent content = new StringContent(JsonSerializer.Serialize(body, Options));
        var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }
    }

    protected static async Task<TResult> HttpPost<TBody, TResult>(HttpClient httpClient, string requestUri,
        TBody body, CancellationToken cancellationToken = default)
    {
        HttpContent content = new StringContent(JsonSerializer.Serialize(body, Options));
        var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        var respContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResult>(respContent, Options);
    }

    protected static async Task HttpPatch<TBody>(HttpClient httpClient, string requestUri, TBody body,
        CancellationToken cancellationToken = default)
    {
        HttpContent content = new StringContent(JsonSerializer.Serialize(body, Options));
        var request = new HttpRequestMessage(HttpMethod.Patch, requestUri)
        {
            Content = content
        };

        var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }
    }

    protected static async Task<TResult> HttpPatch<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        HttpContent reqContent = new StringContent(JsonSerializer.Serialize(body, Options));
        var request = new HttpRequestMessage(HttpMethod.Patch, requestUri)
        {
            Content = reqContent
        };

        var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        var respContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResult>(respContent, Options);
    }

    protected static async Task<TResult> HttpPut<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        HttpContent reqContent = new StringContent(JsonSerializer.Serialize(body, Options));
        var response = await httpClient.PutAsync(requestUri, reqContent, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        var respContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        return JsonSerializer.Deserialize<TResult>(respContent, Options);
    }

    protected static async Task HttpPut<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
    {
        HttpContent reqContent = new StringContent(JsonSerializer.Serialize(body, Options));
        var response = await httpClient.PutAsync(requestUri, reqContent, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }
    }

    protected static async Task HttpDelete(HttpClient httpClient, string requestUri,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }
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
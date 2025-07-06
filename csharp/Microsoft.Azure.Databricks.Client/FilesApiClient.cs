using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client;

public sealed class FilesApiClient : ApiClient, IFilesApi
{
    private readonly string _apiBaseUrl;

    public FilesApiClient(HttpClient httpClient) : base(httpClient)
    {
        _apiBaseUrl = $"{ApiVersion}/fs";
    }

    public async Task<(IEnumerable<DirectoryEntry>, string)> ListDirectoryContents(string directoryPath, long? pageSize = default,
        string pageToken = default, CancellationToken cancellationToken = default)
    {
        if (pageSize < 0 || pageSize > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be between 0 and 1000.");
        }

        StringBuilder requestUriSb = new($"{_apiBaseUrl}/directories{directoryPath}?");
        if (pageSize != null)
        {
            requestUriSb.Append($"&page_size={pageSize}");
        }

        if (!string.IsNullOrEmpty(pageToken))
        {
            requestUriSb.Append($"&page_token={pageToken}");
        }

        var requestUri = requestUriSb.ToString();
        var response = await HttpGet<JsonObject>(this.HttpClient, requestUri, cancellationToken).ConfigureAwait(false);

        response.TryGetPropertyValue("contents", out var contentNode);
        response.TryGetPropertyValue("next_page_token", out var nextPageTokenNode);

        var entries = contentNode?.Deserialize<IEnumerable<DirectoryEntry>>(Options) ?? Enumerable.Empty<DirectoryEntry>();
        var nextPageToken = nextPageTokenNode?.Deserialize<string>(Options) ?? string.Empty;

        return (entries, nextPageToken);
    }

    public global::Azure.AsyncPageable<DirectoryEntry> ListDirectoryContentsPageable(string directoryPath, long? pageSize = null, CancellationToken cancellationToken = default)
    {
        return new AsyncPageable<DirectoryEntry>(async (pageToken) =>
        {
            var (entries, nextPageToken) = await ListDirectoryContents(
                directoryPath,
                pageSize,
                pageToken,
                cancellationToken).ConfigureAwait(false);

            return (entries.ToList(), !string.IsNullOrEmpty(nextPageToken), nextPageToken);
        });
    }

    public async Task<HttpContentHeaders> GetDirectoryMetadata(string directoryPath, CancellationToken cancellationToken = default)
    {
        return await HttpHead(this.HttpClient, $"{_apiBaseUrl}/directories{directoryPath}", cancellationToken);
    }

    public async Task CreateDirectory(string directoryPath, CancellationToken cancellationToken = default)
    {
        await HttpPut<object>(this.HttpClient, $"{_apiBaseUrl}/directories{directoryPath}", null, cancellationToken);
    }

    public async Task DeleteDirectory(string directoryPath, CancellationToken cancellationToken = default)
    {
        await HttpDelete(this.HttpClient, $"{_apiBaseUrl}/directories{directoryPath}", cancellationToken);
    }

    public async Task Download(string filePath, Stream stream, string range = default, string ifUnmodifiedSince = default, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/files{filePath}");
        if (!string.IsNullOrEmpty(range))
        {
            request.Headers.Add("Range", range);
        }

        if (!string.IsNullOrEmpty(ifUnmodifiedSince))
        {
            request.Headers.Add("If-Unmodified-Since", ifUnmodifiedSince);
        }

        var response = await this.HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        await contentStream.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    public async Task<HttpContentHeaders> GetFileMetadata(string filePath, string range = default, string ifUnmodifiedSince = default, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Head, $"{_apiBaseUrl}/files{filePath}");
        if (!string.IsNullOrEmpty(range))
        {
            request.Headers.Add("Range", range);
        }

        if (!string.IsNullOrEmpty(ifUnmodifiedSince))
        {
            request.Headers.Add("If-Unmodified-Since", ifUnmodifiedSince);
        }

        var response = await this.HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }

        return response.Content.Headers;
    }

    public async Task Upload(string filePath, Stream stream, bool? overwrite = default,
        CancellationToken cancellationToken = default)
    {
        var requestUri = overwrite == null ? $"{_apiBaseUrl}/files{filePath}" : $"{_apiBaseUrl}/files{filePath}?overwrite={overwrite.ToString().ToLowerInvariant()}";

        using var content = new StreamContent(stream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        var response = await this.HttpClient.PutAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            throw CreateApiException(response);
        }
    }

    public async Task Delete(string filePath, CancellationToken cancellationToken = default)
    {
        await HttpDelete(this.HttpClient, $"{_apiBaseUrl}/files{filePath}", cancellationToken).ConfigureAwait(false);
    }


}

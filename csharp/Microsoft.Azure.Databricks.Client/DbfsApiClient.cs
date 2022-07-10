using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.Azure.Databricks.Client.Models;
using FileInfo = Microsoft.Azure.Databricks.Client.Models.FileInfo;

namespace Microsoft.Azure.Databricks.Client
{
    public sealed class DbfsApiClient : ApiClient, IDbfsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbfsApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        public DbfsApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<long> Create(string path, bool overwrite, CancellationToken cancellationToken = default)
        {
            var request = new { path, overwrite };
            var response = await HttpPost<dynamic, FileHandle>(this.HttpClient, $"{ApiVersion}/dbfs/create", request, cancellationToken).ConfigureAwait(false);
            return response.Handle;
        }

        public async Task AddBlock(long fileHandle, byte[] data, CancellationToken cancellationToken = default)
        {
            var request = new { handle = fileHandle, data };
            await HttpPost(this.HttpClient, $"{ApiVersion}/dbfs/add-block", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task Close(long fileHandle, CancellationToken cancellationToken = default)
        {
            var handle = new FileHandle { Handle = fileHandle };
            await HttpPost(this.HttpClient, $"{ApiVersion}/dbfs/close", handle, cancellationToken).ConfigureAwait(false);
        }

        public async Task Upload(string path, bool overwrite, Stream stream, CancellationToken cancellationToken = default)
        {
            const int mb = 1024 * 1024;
            var handle = await this.Create(path, overwrite, cancellationToken).ConfigureAwait(false);

            var originalPosition = 0L;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            var buffer = new byte[mb];
            try
            {
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, mb), cancellationToken)) > 0)
                {
                    var contents = new byte[bytesRead];
                    Array.Copy(buffer, contents, bytesRead);
                    await this.AddBlock(handle, contents, cancellationToken).ConfigureAwait(false);
                }

                await this.Close(handle, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public async Task Delete(string path, bool recursive, CancellationToken cancellationToken = default)
        {
            var request = new { path = path, recursive };
            await HttpPost(this.HttpClient, $"{ApiVersion}/dbfs/delete", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<FileInfo> GetStatus(string path, CancellationToken cancellationToken = default)
        {
            var encodedPath = WebUtility.UrlEncode(path);
            var url = $"{ApiVersion}/dbfs/get-status?path={encodedPath}";
            var result = await HttpGet<FileInfo>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task<IEnumerable<FileInfo>> List(string path, CancellationToken cancellationToken = default)
        {
            var encodedPath = WebUtility.UrlEncode(path);
            var url = $"{ApiVersion}/dbfs/list?path={encodedPath}";
            var result = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            if (result.TryGetPropertyValue("files", out var files))
            {
                return files.Deserialize<IEnumerable<FileInfo>>();
            }
            else
            {
                return Enumerable.Empty<FileInfo>();
            }
        }

        public async Task Mkdirs(string path, CancellationToken cancellationToken = default)
        {
            var request = new { path };
            await HttpPost(this.HttpClient, $"{ApiVersion}/dbfs/mkdirs", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task Move(string sourcePath, string destinationPath, CancellationToken cancellationToken = default)
        {
            var request = new { source_path = sourcePath, destination_path = destinationPath };
            await HttpPost(this.HttpClient, $"{ApiVersion}/dbfs/move", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task Put(string path, byte[] contents, bool overwrite, CancellationToken cancellationToken = default)
        {
            var form = new MultipartFormDataContent
                {
                    {new StringContent(path), "path"},
                    {new StringContent(overwrite.ToString().ToLowerInvariant()), "overwrite"},
                    {new ByteArrayContent(contents), "contents"}
                };

            var response = await this.HttpClient.PostAsync($"{ApiVersion}/dbfs/put", form, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }
        }

        public async Task<FileReadBlock> Read(string path, long offset, long length, CancellationToken cancellationToken = default)
        {
            var encodedPath = WebUtility.UrlEncode(path);
            var url = $"{ApiVersion}/dbfs/read?path={encodedPath}&offset={offset}&length={length}";
            var result = await HttpGet<FileReadBlock>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            return result;
        }

        public async Task Download(string path, Stream stream, CancellationToken cancellationToken = default)
        {
            const int mb = 1024 * 1024;
            var totalBytesRead = 0L;
            var block = await Read(path, totalBytesRead, mb, cancellationToken);

            while (block.BytesRead > 0)
            {
                totalBytesRead += block.BytesRead;
                await stream.WriteAsync(block.Data.AsMemory(0, block.Data.Length), cancellationToken);
                block = await Read(path, totalBytesRead, mb, cancellationToken);
            }
        }
    }
}

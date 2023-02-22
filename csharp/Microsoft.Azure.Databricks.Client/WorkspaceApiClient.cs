// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public class WorkspaceApiClient : ApiClient, IWorkspaceApi
{
    public WorkspaceApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task Delete(string path, bool recursive, CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(new { path, recursive }, Options)!.AsObject();
        await HttpPost(this.HttpClient, $"{ApiVersion}/workspace/delete", request, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<byte[]> Export(string path, ExportFormat format, CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/workspace/export?path={path}&format={format}";
        var result = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        return Convert.FromBase64String(result["content"]!.GetValue<string>());
    }

    public async Task<ObjectInfo> GetStatus(string path, CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/workspace/get-status?path={path}";
        return await HttpGet<ObjectInfo>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
    }

    public async Task Import(string path, ExportFormat format, Language? language, byte[] content, bool overwrite,
        CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(new
        { path, format = format.ToString(), language = language?.ToString(), content, overwrite })!.AsObject();

        await HttpPost(this.HttpClient, $"{ApiVersion}/workspace/import", request, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IEnumerable<ObjectInfo>> List(string path, CancellationToken cancellationToken = default)
    {
        var url = $"{ApiVersion}/workspace/list?path={path}";
        var result = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

        return result.TryGetPropertyValue("objects", out var objects)
            ? from obj in objects!.AsArray() select obj.Deserialize<ObjectInfo>()
            : Enumerable.Empty<ObjectInfo>();
    }

    public async Task Mkdirs(string path, CancellationToken cancellationToken = default)
    {
        var request = JsonSerializer.SerializeToNode(new { path }, Options)!.AsObject();
        await HttpPost(this.HttpClient,
            $"{ApiVersion}/workspace/mkdirs",
            request,
            cancellationToken).ConfigureAwait(false);
    }
}
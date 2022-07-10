using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class WorkspaceApiClient : ApiClient, IWorkspaceApi
    {
        public WorkspaceApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task Delete(string path, bool recursive, CancellationToken cancellationToken = default)
        {
            var request = new {path, recursive};
            await HttpPost<dynamic>(this.HttpClient, $"{ApiVersion}/workspace/delete", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<byte[]> Export(string path, ExportFormat format, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/workspace/export?path={path}&format={format}";
            var result = await HttpGet<dynamic>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
            return result.content.ToObject<byte[]>();
        }

        public async Task<ObjectInfo> GetStatus(string path, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/workspace/get-status?path={path}";
            return await HttpGet<ObjectInfo>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);
        }

        public async Task Import(string path, ExportFormat format, Language? language, byte[] content, bool overwrite, CancellationToken cancellationToken = default)
        {
            var request = new { path, format = format.ToString(), language = language?.ToString(), content, overwrite};
            await HttpPost<dynamic>(this.HttpClient, $"{ApiVersion}/workspace/import", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ObjectInfo>> List(string path, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/workspace/list?path={path}";
            var result = await HttpGet<dynamic>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

            return PropertyExists(result, "objects")
                ? result.objects.ToObject<IEnumerable<ObjectInfo>>()
                : Enumerable.Empty<ObjectInfo>();
        }

        public async Task Mkdirs(string path, CancellationToken cancellationToken = default)
        {
            var request = new {path};
            await HttpPost<dynamic>(this.HttpClient, $"{ApiVersion}/workspace/mkdirs", request, cancellationToken).ConfigureAwait(false);
        }
    }
}

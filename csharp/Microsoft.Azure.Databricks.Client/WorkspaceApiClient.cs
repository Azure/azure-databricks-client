using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class WorkspaceApiClient : ApiClient, IWorkspaceApi
    {
        public WorkspaceApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task Delete(string path, bool recursive)
        {
            var request = new {path, recursive};
            await HttpPost<dynamic>(this.HttpClient, "workspace/delete", request).ConfigureAwait(false);
        }

        public async Task<byte[]> Export(string path, ExportFormat format)
        {
            var url = $"workspace/export?path={path}&format={format}";
            return await HttpGet<byte[]>(this.HttpClient, url).ConfigureAwait(false);
        }

        public async Task<ObjectInfo> GetStatus(string path)
        {
            var url = $"workspace/get-status?path={path}";
            return await HttpGet<ObjectInfo>(this.HttpClient, url).ConfigureAwait(false);
        }

        public async Task Import(string path, ExportFormat format, Language? language, byte[] content, bool overwrite)
        {
            var request = new { path, format, language, content, overwrite};
            await HttpPost<dynamic>(this.HttpClient, "workspace/import", request).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ObjectInfo>> List(string path)
        {
            var url = $"workspace/list?path={path}";
            var result = await HttpGet<dynamic>(this.HttpClient, url).ConfigureAwait(false);

            return PropertyExists(result, "objects")
                ? result.objects.ToObject<IEnumerable<ObjectInfo>>()
                : Enumerable.Empty<ObjectInfo>();
        }

        public async Task Mkdirs(string path)
        {
            var request = new {path};
            await HttpPost<dynamic>(this.HttpClient, "workspace/mkdirs", request).ConfigureAwait(false);
        }
    }
}

using Microsoft.Azure.Databricks.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public class LibrariesApiClient : ApiClient, ILibrariesApi
    {
        public LibrariesApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<IDictionary<string, IEnumerable<LibraryFullStatus>>> AllClusterStatuses(CancellationToken cancellationToken = default)
        {
            var result = await HttpGet<JsonObject>(this.HttpClient, $"{ApiVersion}/libraries/all-cluster-statuses", cancellationToken)
                .ConfigureAwait(false);

            if (result.TryGetPropertyValue("statuses", out var statuses))
            {
                return statuses
                    .Deserialize<IEnumerable<JsonObject>>(Options)
                    .ToDictionary(
                        e => e["cluster_id"].Deserialize<string>(Options),
                        e => e["library_statuses"].Deserialize<IEnumerable<LibraryFullStatus>>(Options)
                    );
            }
            else
            {
                return new Dictionary<string, IEnumerable<LibraryFullStatus>>();
            }
        }

        public async Task<IEnumerable<LibraryFullStatus>> ClusterStatus(string clusterId, CancellationToken cancellationToken = default)
        {
            var url = $"{ApiVersion}/libraries/cluster-status?cluster_id={clusterId}";
            var result = await HttpGet<JsonObject>(this.HttpClient, url, cancellationToken).ConfigureAwait(false);

            if (result.TryGetPropertyValue("library_statuses", out var library_statuses))
            {
                return library_statuses.Deserialize<IEnumerable<LibraryFullStatus>>();
            }
            else
            {
                return Enumerable.Empty<LibraryFullStatus>();
            }
        }

        public async Task Install(string clusterId, IEnumerable<Library> libraries, CancellationToken cancellationToken = default)
        {
            if (libraries == null)
            {
                return;
            }

            var array = libraries as Library[] ?? libraries.ToArray();

            if (!array.Any())
            {
                return;
            }

            var request = new { cluster_id = clusterId, libraries = array };
            await HttpPost(this.HttpClient, $"{ApiVersion}/libraries/install", request, cancellationToken).ConfigureAwait(false);
        }

        public async Task Uninstall(string clusterId, IEnumerable<Library> libraries, CancellationToken cancellationToken = default)
        {
            if (libraries == null)
            {
                return;
            }

            var array = libraries as Library[] ?? libraries.ToArray();

            if (!array.Any())
            {
                return;
            }

            var request = new { cluster_id = clusterId, libraries = array };
            await HttpPost(this.HttpClient, $"{ApiVersion}/libraries/uninstall", request, cancellationToken).ConfigureAwait(false);
        }
    }
}
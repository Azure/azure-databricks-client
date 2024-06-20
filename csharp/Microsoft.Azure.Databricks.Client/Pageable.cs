using Azure;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    internal class AsyncPageable<T> : global::Azure.AsyncPageable<T>
    {
        private readonly Func<string, Task<(List<T>, bool, string)>> _getNextPage;

        public AsyncPageable(Func<string, Task<(List<T>, bool, string)>> getNextPage)
        {
            this._getNextPage = getNextPage;
        }

        public override async IAsyncEnumerable<Page<T>> AsPages(string continuationToken = null, int? pageSizeHint = null)
        {
            var nextPageToken = continuationToken;
            var hasNextPage = true;

            do
            {
                var response = await _getNextPage(nextPageToken);
                hasNextPage = response.Item2;
                nextPageToken = response.Item3;
                yield return Page<T>.FromValues(response.Item1, nextPageToken, null);
            } while (hasNextPage);
        }
    }
}

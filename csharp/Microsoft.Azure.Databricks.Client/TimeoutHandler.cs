using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    /// <summary>
    /// In case HttpClient times out, throw <see cref="TimeoutException"/> instead of <see cref="TaskCanceledException"/>.
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class TimeoutHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException();
            }
        }
    }
}


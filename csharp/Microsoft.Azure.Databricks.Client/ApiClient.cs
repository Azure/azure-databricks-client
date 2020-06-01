using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Databricks.Client
{
    public abstract class ApiClient : IDisposable
    {
        protected readonly HttpClient HttpClient;

        private static readonly JsonSerializerSettings JsonSerializerSettings =
            new JsonSerializerSettings {DefaultValueHandling = DefaultValueHandling.Ignore};

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

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        protected static async Task HttpPost<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body, JsonSerializerSettings));
            var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }
        }

        protected static async Task<TResult> HttpPost<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body, JsonSerializerSettings));
            var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResult>(responseContent);
        }

        protected static bool PropertyExists(JObject obj, string propertyName)
        {
            return obj.ContainsKey(propertyName);
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
}

using Microsoft.Azure.Databricks.Client.Converters;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public abstract class ApiClient : IDisposable
    {
        protected readonly HttpClient HttpClient;

        protected virtual string ApiVersion => "2.0";

        protected static readonly JsonSerializerOptions Options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Converters = {
                new JsonStringEnumConverter(),
                new MillisecondEpochDateTimeConverter(),
                new LibraryConverter(),
                new SecretScopeConverter(),
                new AccessControlRequestConverter()
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

            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(responseStream, Options);
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

        protected static async Task<TResult> HttpPost<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonSerializer.Serialize(body, Options));
            var response = await httpClient.PostAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }

            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return JsonSerializer.Deserialize<TResult>(responseStream, Options);
        }

        protected static async Task HttpPatch<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body, JsonSerializerSettings));
            HttpRequestMessage msg = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };
            var response = await httpClient.SendAsync(msg).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }
        }

        protected static async Task<TResult> HttpPatch<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body, JsonSerializerSettings));
            HttpRequestMessage msg = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };
            var response = await httpClient.SendAsync(msg).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResult>(responseContent);
        }

        protected static async Task<TResult> HttpPut<TBody, TResult>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body, JsonSerializerSettings));
            var response = await httpClient.PutAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResult>(responseContent);
        }

        protected static async Task HttpPut<TBody>(HttpClient httpClient, string requestUri, TBody body, CancellationToken cancellationToken = default)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(body, JsonSerializerSettings));
            var response = await httpClient.PutAsync(requestUri, content, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw CreateApiException(response);
            }
        }

        protected static bool PropertyExists(JObject obj, string propertyName)
        protected static bool PropertyExists(JsonObject obj, string propertyName)
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

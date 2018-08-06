using System;
using System.Net;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClientApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ClientApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
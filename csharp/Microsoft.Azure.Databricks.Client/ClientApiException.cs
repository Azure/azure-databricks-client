using System;
using System.Net;
using System.Runtime.Serialization;

namespace Microsoft.Azure.Databricks.Client
{
    public class ClientApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ClientApiException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        protected ClientApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
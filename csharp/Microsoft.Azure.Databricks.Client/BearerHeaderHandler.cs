// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

/// <summary>
/// A class that handles the addition of a Bearer token to HTTP headers.
/// </summary>
internal class BearerHeaderHandler : DelegatingHandler
{
    private readonly Func<Task<string>> _getToken;

    /// <summary>
    /// Initializes a new instance of the <see cref="BearerHeaderHandler"/> class.
    /// </summary>
    /// <param name="getToken">A function that retrieves the token.</param>
    internal BearerHeaderHandler(Func<Task<string>> getToken)
    {
        _getToken = getToken;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BearerHeaderHandler"/> class with a specified inner handler.
    /// </summary>
    /// <param name="getToken">A function that retrieves the token.</param>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    internal BearerHeaderHandler(Func<Task<string>> getToken, HttpMessageHandler innerHandler) : base(innerHandler)
    {
        _getToken = getToken;
    }

    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains("Authorization"))
        {
            string token = this._getToken().Result;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return base.Send(request, cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains("Authorization"))
        {
            string token = await this._getToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
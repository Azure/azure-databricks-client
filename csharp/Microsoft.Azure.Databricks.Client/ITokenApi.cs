using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface ITokenApi : IDisposable
    {
        /// <summary>
        /// Creates and returns a token for a user. This call returns the error QUOTA_EXCEEDED if the user exceeds their token quota.
        /// This API is available to all users.
        /// </summary>
        /// <param name="lifetimeSeconds">The lifetime of the token, in seconds. If no lifetime is specified, this token remains valid indefinitely.</param>
        /// <param name="comment">Optional description to attach to the token.</param>
        Task<(string, PublicTokenInfo)> Create(long? lifetimeSeconds, string comment, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all the valid tokens for a user-workspace pair.
        /// This API is available to all users.
        /// </summary>
        Task<IEnumerable<PublicTokenInfo>> List(CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes an access token. This call returns the error RESOURCE_DOES_NOT_EXIST if a token with the given ID is not valid.
        /// This API is available to all users.
        /// </summary>
        /// <param name="tokenId">The ID of the token to be revoked.</param>
        /// <returns></returns>
        Task Revoke(string tokenId, CancellationToken cancellationToken = default);
    }
}
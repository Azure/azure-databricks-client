// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public interface IClusterPoliciesApi : IDisposable
{
    /// <summary>
    /// Return a policy specification given a policy ID.
    /// </summary>
    Task<Policy> Get(string policyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return a list of policies accessible by the requesting user.
    /// </summary>
    Task<IEnumerable<Policy>> List(ListOrder sortOrder = default, PolicySortColumn sortBy = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new policy with a given name and definition.
    /// </summary>
    Task<string> Create(string name, string definition, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update an existing policy. This may make some clusters governed by this policy invalid. For such clusters the next cluster edit must provide a confirming configuration, but otherwise they can continue to run.
    /// </summary>
    Task Edit(string policyId, string name, string definition, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a policy. Clusters governed by this policy can still run, but cannot be edited.
    /// </summary>
    Task Delete(string policyId, CancellationToken cancellationToken = default);
}
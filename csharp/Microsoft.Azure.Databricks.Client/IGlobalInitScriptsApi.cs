// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client;

public interface IGlobalInitScriptsApi : IDisposable
{
    /// <summary>
    /// Get a list of all global init scripts for this workspace.
    /// </summary>
    /// <returns>All properties for each script but not the script contents. To retrieve the contents of a script, use the <see cref="Get"/> method.</returns>
    Task<IEnumerable<GlobalInitScript>> List(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all the details of a script, including its contents.
    /// </summary>
    Task<GlobalInitScript> Get(string scriptId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new global init script in this workspace.
    /// </summary>
    /// <param name="name">The name of the script. Pattern: ^[a-zA-Z0-9_\-\. ]{1,100}$.</param>
    /// <param name="script">The content of the script. **No Base64 encoding is needed.**</param>
    /// <param name="enabled">Specifies whether the script is enabled. The script runs only if enabled.</param>
    /// <param name="position">
    /// The position of a global init script, where 0 represents the first global init script to run, 1 is the second global init script to run, and so on.
    /// If you omit the position for a new global init script, it gets the last position. It runs after all current global init scripts.
    /// Setting any value greater than the position of the last script is equivalent to the last position.
    /// </param>
    /// <returns>The ID of the new script</returns>
    Task<string> Create(string name, string script, bool enabled = false, int? position = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a global init script.
    /// </summary>
    Task Delete(string scriptId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update a global init script, specifying only the fields to change. All fields are optional. Null fields retain their current value.
    /// </summary>
    /// <param name="scriptId">The ID of the global init script.</param>
    /// <param name="name">The name of the script</param>
    /// <param name="script">The content of the script. **No Base64 encoding is needed.**</param>
    /// <param name="enabled">Specifies whether the script is enabled. The script runs only if enabled.</param>
    /// <param name="position">
    /// The position of a script, where 0 represents the first script to run, 1 is the second script to run, and so on.
    /// To move the script so that it runs first, set its position to 0.
    /// To move the script to the end, set it to any value greater or equal to the position of the last script. For example, suppose there are three existing scripts with positions 0, 1 and 2. Any position value of 2 or greater puts the script in the last position (2).
    /// If an explicit position value conflicts with an existing script, your request succeeds. The original script at that position and all later scripts have their position incremented by 1.
    /// </param>
    Task Update(string scriptId, string name = null, string script = null, bool? enabled = default, int? position = default,
        CancellationToken cancellationToken = default);
}
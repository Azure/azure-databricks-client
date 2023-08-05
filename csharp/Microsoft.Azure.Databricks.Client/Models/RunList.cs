// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record RunList
{
    /// <summary>
    /// A list of runs, from most recently started to least.
    /// </summary>
    [JsonPropertyName("runs")]
    public IEnumerable<Run> Runs { get; set; }

    /// <summary>
    /// If true, additional runs matching the provided filter are available for listing.
    /// </summary>
    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    /// <summary>
    /// A token that can be used to list the next page of runs.
    /// </summary>
    [JsonPropertyName("next_page_token")]
    public string NextPageToken { get; set; }

    /// <summary>
    /// A token that can be used to list the previous page of runs.
    /// </summary>
    [JsonPropertyName("prev_page_token")]
    public string PrevPageToken { get; set; }
}
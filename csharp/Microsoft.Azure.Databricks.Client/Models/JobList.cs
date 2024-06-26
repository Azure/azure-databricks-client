﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record JobList
{
    public JobList()
    {
        this.Jobs = new List<Job>();
    }

    /// <summary>
    /// The list of jobs.
    /// </summary>
    [JsonPropertyName("jobs")]
    public IEnumerable<Job> Jobs { get; set; }

    /// <summary>
    /// If true, additional jobs matching the provided filter are available for listing.
    /// </summary>
    [JsonPropertyName("has_more")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
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
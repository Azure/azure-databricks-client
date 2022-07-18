// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record ClusterLogConf
{
    /// <summary>
    /// For dbfs, destination must be provided. For example, { "dbfs" : { "destination" : "dbfs:/home/cluster_log" } }
    /// </summary>
    [JsonPropertyName("dbfs")]
    public DbfsStorageInfo Dbfs { get; set; }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record GlobalInitScript
{

    [JsonPropertyName("script_id")]
    public string ScriptId { get; set; }

    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Name { get; set; }

    [JsonPropertyName("position")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Position { get; set; }

    [JsonPropertyName("enabled")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Enabled { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    [JsonPropertyName("script")]
    [JsonInclude]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string ScriptEncoded { get; private set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string Script
    {
        get => this.ScriptEncoded == null
            ? null
            : Encoding.UTF8.GetString(Convert.FromBase64String(this.ScriptEncoded));
        set => this.ScriptEncoded = value == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }
}
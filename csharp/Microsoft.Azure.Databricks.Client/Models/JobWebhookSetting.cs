// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record JobWebhookSetting
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
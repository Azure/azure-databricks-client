// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters;

public class MillisecondEpochDateTimeConverter : JsonConverter<DateTimeOffset?>
{
    public override bool HandleNull => true;

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetInt64(out var time))
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(time);
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? dto, JsonSerializerOptions options)
    {
        if (dto.HasValue)
        {
            writer.WriteNumberValue(dto.Value.ToUnixTimeMilliseconds());
        }
    }
}
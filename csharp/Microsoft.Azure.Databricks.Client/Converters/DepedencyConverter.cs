using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters;

public class DepedencyConverter : JsonConverter<Dependency>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(Dependency).IsAssignableFrom(typeToConvert);
    }

    public override bool HandleNull => true;

    public override Dependency Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dependency = JsonNode.Parse(ref reader)!.AsObject();

        if (dependency.TryGetPropertyValue("table", out _))
        {
            return dependency.Deserialize<TableDependency>();
        }

        if (dependency.TryGetPropertyValue("function", out _))
        {
            return dependency.Deserialize<FunctionDependency>();
        }

        throw new NotSupportedException("Dependency not recognized");
    }

    public override void Write(Utf8JsonWriter writer, Dependency value, JsonSerializerOptions options)
    {
        var node = value switch
        {
            TableDependency tab => JsonSerializer.SerializeToNode(tab),
            FunctionDependency fun => JsonSerializer.SerializeToNode(fun),
            _ => throw new NotImplementedException($"JsonConverter not implemented for type {value.GetType()}")
        };

        node!.WriteTo(writer);
    }
}

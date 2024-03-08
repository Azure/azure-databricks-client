using Microsoft.Azure.Databricks.Client.Models;
using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Converters;

public class TableConstraintConverter : JsonConverter<TableConstraint>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeof(TableConstraint).IsAssignableFrom(typeToConvert);
    }
    public override bool HandleNull => true;

    public override TableConstraint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var tableConstraint = JsonNode.Parse(ref reader)!.AsObject();

        if (tableConstraint.TryGetPropertyValue("primary_key_constraint", out _))
        {
            return tableConstraint.Deserialize<PrimaryKeyTableConstraint>();
        }

        if (tableConstraint.TryGetPropertyValue("foreign_key_constraint", out _))
        {
            return tableConstraint.Deserialize<ForeignKeyTableConstraint>();
        }

        if (tableConstraint.TryGetPropertyValue("named_table_constraint", out _))
        {
            return tableConstraint.Deserialize<NamedTableConstraint>();
        }

        throw new NotSupportedException("Table constraint not recognized.");
    }

    public override void Write(Utf8JsonWriter writer, TableConstraint value, JsonSerializerOptions options)
    {
        var node = value switch
        {
            PrimaryKeyTableConstraint pk => JsonSerializer.SerializeToNode(pk),
            ForeignKeyTableConstraint fk => JsonSerializer.SerializeToNode(fk),
            NamedTableConstraint nam => JsonSerializer.SerializeToNode(nam),
            _ => throw new NotImplementedException($"JsonConverter not implemented for type {value.GetType()}")
        };

        node!.WriteTo(writer);
    }

}

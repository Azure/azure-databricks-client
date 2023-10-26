using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record TableConstraintAttributes
{
    /// <summary>
    /// The full name of the table referenced by the constraint.
    /// </summary>
    [JsonPropertyName("full_name_arg")]
    public string FullNameArg { get; set; }

    /// <summary>
    /// A table constraint, as defined by one of the following fields being set: 
    /// primary_key_constraint, foreign_key_constraint, named_table_constraint.
    /// </summary>
    [JsonPropertyName("constraint")]
    public ConstraintRecord Constraint { get; set; }
}

public record ConstraintRecord
{
    [JsonPropertyName("primary_key_constraint")]
    public PrimaryKey PrimaryKeyConstraint { get; set; }

    [JsonPropertyName("foreign_key_constraint")]
    public ForeignKey ForeignKeyConstraint { get; set; }

    [JsonPropertyName("named_table_constraint")]
    public NamedTable NamedTableConstraint { get; set; }
}

public record PrimaryKey
{
    /// <summary>
    /// The name of the constraint.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Column names for this constraint.
    /// </summary>
    [JsonPropertyName("child_columns")]
    public IEnumerable<string> ChildColumns { get; set; }
}


public record ForeignKey
{
    /// <summary>
    /// The name of the constraint.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Column names for this constraint.
    /// </summary>
    [JsonPropertyName("child_columns")]
    public IEnumerable<string> ChildColumns { get; set; }

    /// <summary>
    /// The full name of the parent constraint.
    /// </summary>
    [JsonPropertyName("parent_table")]
    public string ParentTable { get; set; }

    /// <summary>
    /// Column names for this constraint.
    /// </summary>
    [JsonPropertyName("parent_columns")]
    public IEnumerable<string> ParentColumns { get; set; }
}

public record NamedTable
{
    /// <summary>
    /// The name of the constraint.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Function
{
    /// <summary>
    /// Name of the function, relative to the parent schema.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Name of the parent catalog.
    /// </summary>
    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    /// <summary>
    /// Name of the parent schema relative to its parent catalog.
    /// </summary>
    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    /// <summary>
    /// The array of FunctionParameterInfo definitions of the function's parameters.
    /// </summary>
    [JsonPropertyName("input_params")]
    public IEnumerable<FunctionParameter> InputParameters { get; set; }

    /// <summary>
    /// Scalar function return data type.
    /// </summary>
    [JsonPropertyName("data_type")]
    public DataType? DataType { get; set; }

    /// <summary>
    /// Pretty printed function data type.
    /// </summary>
    [JsonPropertyName("full_data_type")]
    public string FullDataType { get; set; }

    /// <summary>
    /// Table function return parameters.
    /// </summary>
    [JsonPropertyName("return_params")]
    public IEnumerable<FunctionParameter> ReturnParameters { get; set; }

    /// <summary>
    /// Function language.
    /// </summary>
    [JsonPropertyName("routine_body")]
    public Language? RoutineBody { get; set; }

    /// <summary>
    /// Function body.
    /// </summary>
    [JsonPropertyName("routine_definition")]
    public string RoutineDefinition { get; set; }

    /// <summary>
    /// Function dependencies.
    /// </summary>
    [JsonPropertyName("routine_dependencies")]
    public IEnumerable<Dependency> RoutineDependencies { get; set; }

    /// <summary>
    /// Function parameter style.
    /// </summary>
    [JsonPropertyName("parameter_style")]
    public string ParameterStyle { get; set; } = "S";

    /// <summary>
    /// Whether the function is deterministic.
    /// </summary>
    [JsonPropertyName("is_deterministic")]
    public bool IsDeterministic { get; set; }

    /// <summary>
    /// Function SQL data access.
    /// </summary>
    [JsonPropertyName("sql_data_access")]
    public FunctionSqlDataAccess? SqlDataAccess { get; set; }

    /// <summary>
    /// Function null call.
    /// </summary>
    [JsonPropertyName("is_null_call")]
    public bool IsNullCall { get; set; }

    /// <summary>
    /// Function security type.
    /// </summary>
    [JsonPropertyName("security_type")]
    public string SecurityType { get; set; } = "DEFINER";

    /// <summary>
    /// Specific name of the function; Reserved for future use.
    /// </summary>
    [JsonPropertyName("specific_name")]
    public string SpecificName { get; set; }

    /// <summary>
    /// External function name.
    /// </summary>
    [JsonPropertyName("external_name")]
    public string ExternalName { get; set; }

    /// <summary>
    /// External function language.
    /// </summary>
    [JsonPropertyName("external_language")]
    public string ExternalLanguage { get; set; }

    /// <summary>
    /// IEnumerable of schemas whose objects can be referenced without qualification.
    /// </summary>
    [JsonPropertyName("sql_path")]
    public string SqlPath { get; set; }

    /// <summary>
    /// Username of the current owner of the function.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// A map of key-value properties attached to the securable.
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, string> Properties { get; set; }

    /// <summary>
    /// Unique identifier of the parent metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// Full name of the function, in the form of catalog_name.schema_name.function__name.
    /// </summary>
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    /// <summary>
    /// Time at which this function was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of the function creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Time at which this function was last updated, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of the user who last modified the function.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// Id of the Function, relative to the parent schema.
    /// </summary>
    [JsonPropertyName("function_id")]
    public string FunctionId { get; set; }
}

public record FunctionParameter
{
    /// <summary>
    /// Name of parameter.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Full data type spec, SQL/catalogString text.
    /// </summary>
    [JsonPropertyName("type_text")]
    public string TypeText { get; set; }

    /// <summary>
    /// Full data type spec, JSON-serialized.
    /// </summary>
    [JsonPropertyName("type_json")]
    public string TypeJson { get; set; }

    /// <summary>
    /// Name of type (INT, STRUCT, MAP, etc.).
    /// </summary>
    [JsonPropertyName("type_name")]
    public DataType? TypeName { get; set; }

    /// <summary>
    /// Digits of precision; required on Create for DecimalTypes.
    /// </summary>
    [JsonPropertyName("type_precision")]
    public int TypePrecision { get; set; }

    /// <summary>
    /// Digits to the right of the decimal; Required on Create for DecimalTypes.
    /// </summary>
    [JsonPropertyName("type_scale")]
    public int TypeScale { get; set; }

    /// <summary>
    /// Format of IntervalType.
    /// </summary>
    [JsonPropertyName("type_interval_type")]
    public string TypeIntervalType { get; set; }

    /// <summary>
    /// Ordinal position of the column (starting at position 0).
    /// </summary>
    [JsonPropertyName("position")]
    public int Position { get; set; }

    /// <summary>
    /// The mode of the function parameter.
    /// </summary>
    [JsonPropertyName("parameter_mode")]
    public string ParameterMode { get; set; } = "IN";

    /// <summary>
    /// The type of function parameter.
    /// Enum: "PARAM", "COLUMN".
    /// </summary>
    [JsonPropertyName("parameter_type")]
    public ParameterType? ParameterType { get; set; }

    /// <summary>
    /// Default value of the parameter.
    /// </summary>
    [JsonPropertyName("parameter_default")]
    public string ParameterDefault { get; set; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }
}

public enum ParameterType
{
    PARAM,
    COLUMN
}

public enum Language
{
    SQL,
    EXTERNAL
}


public enum FunctionSqlDataAccess
{
    CONTAINS_SQL,
    READS_SQL_DATA,
    NO_SQL
}
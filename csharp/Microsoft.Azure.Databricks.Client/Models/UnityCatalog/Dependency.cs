using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public abstract record Dependency;

public record TableDependency : Dependency
{
    [JsonPropertyName("table")]
    public TableDependencyProperties Table { get; set; }
}

public record FunctionDependency : Dependency
{
    [JsonPropertyName("function")]
    public FunctionDependencyProperties Function { get; set; }
}

public record TableDependencyProperties
{
    [JsonPropertyName("table_full_name")]
    public string TableFullName { get; set; }
}

public record FunctionDependencyProperties
{
    [JsonPropertyName("function_full_name")]
    public string FunctionFullName { get; set; }
}
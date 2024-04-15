using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Table : TableSummary
{
    /// <summary>
    /// Name of table, relative to parent schema.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Name of parent catalog.
    /// </summary>
    [JsonPropertyName("catalog_name")]
    public string CatalogName { get; set; }

    /// <summary>
    /// Name of parent schema relative to its parent catalog.
    /// </summary>
    [JsonPropertyName("schema_name")]
    public string SchemaName { get; set; }

    /// <summary>
    /// Data source format
    /// </summary>
    [JsonPropertyName("data_source_format")]
    public DataSourceFormat? DataSourceFormat { get; set; }

    /// <summary>
    /// The array of ColumnInfo definitions of the table's columns.
    /// </summary>
    [JsonPropertyName("columns")]
    public IEnumerable<Column> Columns { get; set; }

    /// <summary>
    /// Storage root URL for table (for MANAGED, EXTERNAL tables)
    /// </summary>
    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }

    /// <summary>
    /// View definition SQL (when table_type is VIEW, MATERIALIZED_VIEW, or STREAMING_TABLE)
    /// </summary>
    [JsonPropertyName("view_definition")]
    public string ViewDefinition { get; set; }

    /// <summary>
    /// View dependencies (when table_type == VIEW or MATERIALIZED_VIEW, STREAMING_TABLE)
    /// </summary>
    [JsonPropertyName("view_dependencies")]
    public ViewDependencies ViewDependencies { get; set; }

    /// <summary>
    /// List of schemes whose objects can be referenced without qualification.
    /// </summary>
    [JsonPropertyName("sql_path")]
    public string SqlPath { get; set; }

    /// <summary>
    /// Username of current owner of table.
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
    /// Name of the storage credential, when a storage credential is configured for use with this table.
    /// </summary>
    [JsonPropertyName("storage_credential_name")]
    public string StorageCredentialName { get; set; }

    /// <summary>
    /// List of table constraints.
    /// </summary>
    [JsonPropertyName("table_constraints")]
    public IEnumerable<TableConstraint> TableConstraints { get; set; }

    [JsonPropertyName("row_filter")]
    public RowFilter RowFilter { get; set; }

    /// <summary>
    /// Unique identifier of parent metastore.
    /// </summary>
    [JsonPropertyName("metastore_id")]
    public string MetastoreId { get; set; }

    /// <summary>
    /// Deprecated. Unique ID of the Data Access Configuration to use with the table data.
    /// </summary>
    [JsonPropertyName("data_access_configuration_id")]
    public string DataAccessConfigurationId { get; set; }

    /// <summary>
    /// Time at which this table was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of table creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Time at which this table was last modified, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of user who last modified the table.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// Time at which this table was deleted, in epoch milliseconds. Field is omitted if table is not deleted.
    /// </summary>
    [JsonPropertyName("deleted_at")]
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Name of table, relative to parent schema.
    /// </summary>
    [JsonPropertyName("table_id")]
    public string TableId { get; set; }

    /// <summary>
    /// Information pertaining to current state of the delta table.
    /// </summary>
    [JsonPropertyName("delta_runtime_properties_kvpairs")]
    public DeltaRuntimePropertyBag DeltaRuntimeProperties { get; set; }
}

public record Column
{
    /// <summary>
    /// Name of Column.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Full data type specification as SQL/catalogString text.
    /// </summary>
    [JsonPropertyName("type_text")]
    public string TypeText { get; set; }

    /// <summary>
    /// Full data type specification, JSON-serialized.
    /// </summary>
    [JsonPropertyName("type_json")]
    public string TypeJson { get; set; }

    /// <summary>
    /// Name of type (INT, STRUCT, MAP, etc.).
    /// </summary>
    [JsonPropertyName("type_name")]
    public DataType? TypeName { get; set; }

    /// <summary>
    /// Digits of precision; required for DecimalTypes.
    /// </summary>
    [JsonPropertyName("type_precision")]
    public int TypePrecision { get; set; }

    /// <summary>
    /// Digits to right of decimal; Required for DecimalTypes.
    /// </summary>
    [JsonPropertyName("type_scale")]
    public int TypeScale { get; set; }

    /// <summary>
    /// Format of IntervalType.
    /// </summary>
    [JsonPropertyName("type_interval_type")]
    public string TypeIntervalType { get; set; }

    /// <summary>
    /// Ordinal position of column (starting at position 0).
    /// </summary>
    [JsonPropertyName("position")]
    public int Position { get; set; }

    /// <summary>
    /// User-provided free-form text description.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// Whether field may be Null (default: true).
    /// </summary>
    [JsonPropertyName("nullable")]
    public bool Nullable { get; set; } = true;

    /// <summary>
    /// Partition index for column.
    /// </summary>
    [JsonPropertyName("partition_index")]
    public int PartitionIndex { get; set; }

    [JsonPropertyName("mask")]
    public Mask Mask { get; set; }
}

public record Mask
{
    /// <summary>
    /// The full name of the column mask SQL UDF.
    /// </summary>
    [JsonPropertyName("function_name")]
    public string FunctionName { get; set; }

    /// <summary>
    /// The list of additional table columns to be passed as input to the column mask function. 
    /// </summary>
    [JsonPropertyName("using_column_names")]
    public List<string> UsingColumnNames { get; set; }
}

public record RowFilter
{
    /// <summary>
    /// The full name of the row filter SQL UDF.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The list of table columns to be passed as input to the row filter function.
    /// The column types should match the types of the filter function arguments.
    /// </summary>
    [JsonPropertyName("input_column_names")]
    public List<string> InputColumnNames { get; set; }
}

public record DeltaRuntimePropertyBag
{
    /// <summary>
    /// A map of key-value properties attached to the securable.
    /// </summary>
    [JsonPropertyName("delta_runtime_properties")]
    public Dictionary<string, string> Properties { get; set; }
}

public abstract record TableConstraint;

public record PrimaryKeyTableConstraint : TableConstraint
{
    [JsonPropertyName("primary_key_constraint")]
    public PrimaryKeyConstraint PrimaryKey { get; set; }
}

public record ForeignKeyTableConstraint : TableConstraint
{
    [JsonPropertyName("foreign_key_constraint")]
    public ForeignKeyConstraint ForeignKey { get; set; }
}

public record NamedTableConstraint : TableConstraint
{
    [JsonPropertyName("named_table_constraint")]
    public NamedConstraint NamedTable { get; set; }
}

public record ViewDependencies
{
    /// <summary>
    /// Array of dependencies.
    /// </summary>
    [JsonPropertyName("dependencies")]
    public IEnumerable<Dependency> Dependencies { get; set; }
}

public enum TableType
{
    MANAGED,
    EXTERNAL,
    VIEW,
    MATERIALIZED_VIEW,
    STREAMING_TABLE,
    FOREIGN
}

public enum DataSourceFormat
{
    DELTA,
    CSV,
    JSON,
    AVRO,
    PARQUET,
    ORC,
    TEXT,
    UNITY_CATALOG,
    DELTASHARING,
    POSTGRESQL_FORMAT
}
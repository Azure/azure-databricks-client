// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

/// <summary>
/// A SQL statement and its execution options.
/// </summary>
public record SqlStatement
{
    /// <summary>
    /// Creates a new SQL statement.
    /// </summary>
    /// <param name="statement">The SQL statement to execute.</param>
    /// <param name="warehouseId">Warehouse upon which to execute a statement.</param>
    /// <param name="parameters">A list of parameters to pass into a SQL statement containing parameter markers.</param>
    /// <returns>A SQL statement that can be executed.</returns>
    public static SqlStatement Create(string statement, string warehouseId, params SqlStatementParameter[] parameters)
    {
        ArgumentNullException.ThrowIfNull(statement);
        ArgumentNullException.ThrowIfNull(warehouseId);

        return new SqlStatement
        {
            Statement = statement,
            WarehouseId = warehouseId,
            Parameters = new List<SqlStatementParameter>(parameters)
        };
    }

    /// <summary>
    /// The SQL statement to execute. The statement can optionally be parameterized, see <see cref="Parameters"/>.
    /// </summary>
    [JsonPropertyName("statement")]
    public string Statement { get; set; }

    /// <summary>
    /// Warehouse upon which to execute a statement.
    /// </summary>
    [JsonPropertyName("warehouse_id")]
    public string WarehouseId { get; set; }

    /// <summary>
    /// Sets default catalog for statement execution, similar to USE CATALOG in SQL.
    /// </summary>
    [JsonPropertyName("catalog")]
    public string Catalog { get; set; }

    /// <summary>
    /// Sets default schema for statement execution, similar to USE SCHEMA in SQL.
    /// </summary>
    [JsonPropertyName("schema")]
    public string Schema { get; set; }

    /// <summary>
    /// A list of parameters to pass into a SQL statement containing parameter markers.
    /// </summary>
    [JsonPropertyName("parameters")]
    public ICollection<SqlStatementParameter> Parameters { get; set; } = new List<SqlStatementParameter>();

    /// <summary>
    /// Applies the given row limit to the statement's result set, but unlike the LIMIT clause in SQL, it also sets the truncated field in the response to indicate whether the result was trimmed due to the limit or not.
    /// </summary>
    [JsonPropertyName("row_limit")]
    public long? RowLimit { get; set; }

    /// <summary>
    /// Applies the given byte limit to the statement's result size. Byte counts are based on internal data representations and might not match the final size in the requested format. If the result was truncated due to the byte limit, then truncated in the response is set to true. When using EXTERNAL_LINKS disposition, a default byte_limit of 100 GiB is applied if byte_limit is not explcitly set.
    /// </summary>
    [JsonPropertyName("byte_limit")]
    public long? ByteLimit { get; set; }

    /// <summary>
    /// The fetch disposition provides two modes of fetching results: INLINE and EXTERNAL_LINKS.
    /// </summary>
    [JsonPropertyName("disposition")]
    public SqlStatementDisposition Disposition { get; set; }

    /// <summary>
    /// Statement execution supports three result formats: JSON_ARRAY (default), ARROW_STREAM, and CSV.
    /// </summary>
    /// <remarks>
    /// Important: The formats ARROW_STREAM and CSV are supported only with EXTERNAL_LINKS disposition. JSON_ARRAY is supported in INLINE and EXTERNAL_LINKS disposition.
    /// </remarks>
    [JsonPropertyName("format")]
    public StatementFormat Format { get; set; } = StatementFormat.JSON_ARRAY;

    /// <summary>
    /// The time in seconds the call will wait for the statement's result set as Ns, where N can be set to 0 or to a value between 5 and 50.
    /// </summary>
    [JsonPropertyName("wait_timeout")]
    public string WaitTimeout { get; set; }

    /// <summary>
    /// When wait_timeout > 0s, the call will block up to the specified time. If the statement execution doesn't finish within this time, on_wait_timeout determines whether the execution should continue or be canceled. When set to CONTINUE, the statement execution continues asynchronously and the call returns a statement ID which can be used for polling with statementexecution/getstatement. When set to CANCEL, the statement execution is canceled and the call returns with a CANCELED state.
    /// </summary>
    [JsonPropertyName("on_wait_timeout")]
    public SqlStatementOnWaitTimeout OnWaitTimeout { get; set; } = SqlStatementOnWaitTimeout.CONTINUE;
}

public enum SqlStatementOnWaitTimeout
{
    CONTINUE,
    CANCEL
}

public static class SqlStatementWaitTimeout
{
    /// <summary>
    /// Create a wait_timeout formatted string.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A string formatted to the wait_timeout specification.</returns>
    public static string Create(int seconds)
        => $"{seconds}s";

    /// <summary>
    /// Create a wait_timeout formatted string.
    /// </summary>
    /// <param name="timeout">The number of seconds.</param>
    /// <returns>A string formatted to the wait_timeout specification.</returns>
    public static string Create(TimeSpan timeout)
        => $"{Convert.ToInt32(timeout.TotalSeconds)}s";
}

public enum SqlStatementDisposition
{
    /// <remarks>
    /// Statements executed with INLINE disposition will return result data inline, in JSON_ARRAY format, in a series of chunks. If a given statement produces a result set with a size larger than 25 MiB, that statement execution is aborted, and no result set will be available.
    /// </remarks>
    INLINE,

    /// <remarks>
    /// Statements executed with EXTERNAL_LINKS disposition will return result data as external links: URLs that point to cloud storage internal to the workspace. Using EXTERNAL_LINKS disposition allows statements to generate arbitrarily sized result sets for fetching up to 100 GiB. The resulting links have two important properties:
    ///   1. They point to resources external to the Databricks compute; therefore any associated authentication information (typically a personal access token, OAuth token, or similar) must be removed when fetching from these links.
    ///   2. These are presigned URLs with a specific expiration, indicated in the response. The behavior when attempting to use an expired link is cloud specific.
    /// </remarks>
    EXTERNAL_LINKS
}

public enum StatementFormat
{
    /// <remarks>
    /// When specifying format=JSON_ARRAY, result data will be formatted as an array of arrays of values, where each value is either the string representation of a value, or null. For example, the output of SELECT concat('id-', id) AS strCol, id AS intCol, null AS nullCol FROM range(3) would look like this:
    ///   [
    ///     ["id-1", "1", null],
    ///     ["id-2", "2", null],
    ///     ["id-3", "3", null],
    ///   ]
    ///   
    /// When specifying format=JSON_ARRAY and disposition=EXTERNAL_LINKS, each chunk in the result contains compact JSON with no indentation or extra whitespace.
    /// </remarks>
    JSON_ARRAY,

    /// <remarks>
    /// When specifying format=ARROW_STREAM and disposition=EXTERNAL_LINKS, each chunk in the result will be formatted as Apache Arrow Stream. 
    /// </remarks>
    ARROW_STREAM,

    /// <remarks>
    /// When specifying format=CSV and disposition=EXTERNAL_LINKS, each chunk in the result will be a CSV according to RFC 4180 standard. All the columns values will have string representation similar to the JSON_ARRAY format, and null values will be encoded as “null”. Only the first chunk in the result would contain a header row with column names. For example, the output of SELECT concat('id-', id) AS strCol, id AS intCol, null as nullCol FROM range(3) would look like this:
    ///   strCol, intCol, nullCol
    ///   id-1,1,null
    ///   id-2,2,null
    ///   id-3,3,null
    /// </remarks>
    CSV
}

public record SqlStatementParameter
{
    /// <summary>
    /// The name of a parameter marker to be substituted in the statement.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The value to substitute, represented as a string. If omitted, the value is interpreted as NULL.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// The data type, given as a string. For example: INT, STRING, DECIMAL(10,2). If no type is given the type is assumed to be STRING. Complex types, such as ARRAY, MAP, and STRUCT are not supported. For valid types, refer to the section Data types of the SQL language reference.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// Create a instance based on the specified data type.
    /// </summary>
    /// <typeparam name="T">The type of the parameter value.</typeparam>
    /// <param name="name">The parameter name.</param>
    /// <param name="value">The parameter value.</param>
    /// <returns>An instance that has the best fit type and converted value format.</returns>
    public static SqlStatementParameter Create<T>(string name, T value)
    {
        return new SqlStatementParameter
        {
            Name = name,
            Value = Convert(value),
            Type = SqlStatementParameterTypes.From(value)
        };
    }

    private static string Convert<T>(T value)
        => value switch
        {
            null => string.Empty,
            byte[] bytes => System.Convert.ToHexString(bytes),
            DateOnly date => date.ToString("yyyy-MM-dd"),
            DateTime timestamp => timestamp.ToString("O"),
            DateTimeOffset timestamp => timestamp.ToString("O"),
            _ => value.ToString()
        };
}

public static class SqlStatementParameterTypes
{
    public static readonly string Boolean = "BOOLEAN";
    public static readonly string DateOnly = "DATE";
    public static readonly string DateTime = "TIMESTAMP";
    public static string Decimal(int precision = 10, int scale = 0) => $"DECIMAL({precision}, {scale})";
    public static readonly string Double = "DOUBLE";
    public static readonly string Float = "FLOAT";
    public static readonly string Binary = "BINARY";
    public static readonly string Byte = "TINYINT";
    public static readonly string Int16 = "SMALLINT";
    public static readonly string Int32 = "INT";
    public static readonly string Int64 = "BIGINT";
    public static readonly string String = "STRING";

    /// <summary>
    /// Attempts to convert the value to a best fit parameter data type.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value of the type.</param>
    /// <returns>The best parameter data type for the provided value type.</returns>
    public static string From<T>(T value)
        => value switch
        {
            bool _ => Boolean,
            DateOnly _ => DateOnly,
            DateTime _ => DateTime,
            decimal _ => Decimal(),
            double _ => Double,
            float _ => Float,
            byte[] _ => Binary,
            byte _ => Byte,
            short _ => Int16,
            int _ => Int32,
            long _ => Int64,
            _ => String
        };
}

public record StatementExecution
{
    /// <summary>
    /// The statement ID is returned upon successfully submitting a SQL statement, and is a required reference for all subsequent calls.
    /// </summary>
    [JsonPropertyName("statement_id")]
    public string StatementId { get; set; }

    /// <summary>
    /// The status response includes execution state and if relevant, error information.
    /// </summary>
    [JsonPropertyName("status")]
    public StatementExecutionStatus Status { get; set; }

    /// <summary>
    /// The result manifest provides schema and metadata for the result set.
    /// </summary>
    [JsonPropertyName("manifest")]
    public StatementExecutionManifest Manifest { get; set; }

    /// <summary>
    /// Contains the result data of a single chunk when using INLINE disposition. When using EXTERNAL_LINKS disposition, the array external_links is used instead to provide presigned URLs to the result data in cloud storage. Exactly one of these alternatives is used. (While the external_links array prepares the API to return multiple links in a single response. Currently only a single link is returned.)
    /// </summary>
    [JsonPropertyName("result")]
    public StatementExecutionResultChunk Result { get; set; }
}

public record StatementExecutionStatus
{
    /// <summary>
    /// The statement execution state.
    /// </summary>
    [JsonPropertyName("state")]
    public StatementExecutionState State { get; set; }

    /// <summary>
    /// The statement error information, if available.
    /// </summary>
    [JsonPropertyName("error")]
    public StatementExecutionError Error { get; set; }
}

public enum StatementExecutionState
{
    /// <summary>
    /// Waiting for warehouse.
    /// </summary>
    PENDING,

    RUNNING,

    /// <summary>
    /// Execution was successful, result data available for fetch
    /// </summary>
    SUCCEEDED,

    /// <summary>
    /// Execution failed; reason for failure described in accompanying error message
    /// </summary>
    FAILED,

    /// <summary>
    /// User canceled; can come from explicit cancel call, or timeout with on_wait_timeout=CANCEL
    /// </summary>
    CANCELED,

    /// <summary>
    /// Execution successful, and statement closed; result no longer available for fetch
    /// </summary>
    CLOSED
}

public record StatementExecutionError
{
    /// <summary>
    /// The statement error code.
    /// </summary>
    [JsonPropertyName("error_code")]
    public StatementExecutionErrorCode ErrorCode { get; set; }

    /// <summary>
    /// A brief summary of the error condition.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public enum StatementExecutionErrorCode
{
    UNKNOWN,
    INTERNAL_ERROR,
    TEMPORARILY_UNAVAILABLE,
    IO_ERROR,
    BAD_REQUEST,
    SERVICE_UNDER_MAINTENANCE,
    WORKSPACE_TEMPORARILY_UNAVAILABLE,
    DEADLINE_EXCEEDED,
    CANCELLED,
    RESOURCE_EXHAUSTED,
    ABORTED,
    NOT_FOUND,
    ALREADY_EXISTS,
    UNAUTHENTICATED
}

public record StatementExecutionManifest
{
    /// <summary>
    /// The statement execution result format.
    /// </summary>
    [JsonPropertyName("format")]
    public StatementFormat Format { get; set; }

    /// <summary>
    /// The schema is an ordered list of column descriptions.
    /// </summary>
    [JsonPropertyName("schema")]
    public StatementExecutionSchema Schema { get; set; }

    /// <summary>
    /// The total number of chunks that the result set has been divided into.
    /// </summary>
    [JsonPropertyName("total_chunk_count")]
    public int TotalChunkCount { get; set; }

    /// <summary>
    /// Array of result set chunk metadata.
    /// </summary>
    [JsonPropertyName("chunks")]
    public StatementExecutionChunk[] Chunks { get; set; } = Array.Empty<StatementExecutionChunk>();

    /// <summary>
    /// The total number of rows in the result set.
    /// </summary>
    [JsonPropertyName("total_row_count")]
    public long TotalRowCount { get; set; }

    /// <summary>
    /// The total number of bytes in the result set. This field is not available when using INLINE disposition.
    /// </summary>
    [JsonPropertyName("total_byte_count")]
    public long TotalByteCount { get; set; }

    /// <summary>
    /// Indicates whether the result is truncated due to row_limit or byte_limit.
    /// </summary>
    [JsonPropertyName("truncated")]
    public bool Truncated { get; set; }

    public virtual bool Equals(StatementExecutionManifest other)
    {
        return other is not null
            && Format.Equals(other.Format)
            && Schema.Equals(other.Schema)
            && TotalChunkCount == other.TotalChunkCount
            && Chunks.SequenceEqual(other.Chunks)
            && TotalRowCount == other.TotalRowCount
            && TotalByteCount == other.TotalByteCount
            && Truncated == other.Truncated;
    }

    public override int GetHashCode()
    {
        var hash = HashCode.Combine(Format, Schema, TotalChunkCount, TotalRowCount, TotalByteCount, Truncated);
        foreach (var chunk in Chunks)
        {
            hash *= chunk.GetHashCode();
        }
        return hash;
    }
}

public record StatementExecutionSchema
{
    /// <summary>
    /// The number of columns.
    /// </summary>
    [JsonPropertyName("column_count")]
    public int ColumnCount { get; set; }

    /// <summary>
    /// The columns in the schema.
    /// </summary>
    [JsonPropertyName("columns")]
    public IEnumerable<StatementExecutionSchemaColumn> Columns { get; set; }

    public virtual bool Equals(StatementExecutionSchema other)
    {
        return other is not null
            && ColumnCount == other.ColumnCount
            && Columns.SequenceEqual(other.Columns);
    }

    public override int GetHashCode()
    {
        var hash = ColumnCount.GetHashCode();
        foreach (var column in Columns)
        {
            hash *= column.GetHashCode();
        }
        return hash;
    }
}

public record StatementExecutionSchemaColumn
{
    /// <summary>
    /// The name of the column.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The full SQL type specification.
    /// </summary>
    [JsonPropertyName("type_text")]
    public string TypeText { get; set; }

    /// <summary>
    /// The name of the base data type. This doesn't include details for complex types such as STRUCT, MAP or ARRAY.
    /// </summary>
    [JsonPropertyName("type_name")]
    public string TypeName { get; set; }

    /// <summary>
    /// Specifies the number of digits in a number. This applies to the DECIMAL type.
    /// </summary>
    [JsonPropertyName("type_precision")]
    public int TypePrecision { get; set; }

    /// <summary>
    /// Specifies the number of digits to the right of the decimal point in a number. This applies to the DECIMAL type.
    /// </summary>
    [JsonPropertyName("type_scale")]
    public int TypeScale { get; set; }

    /// <summary>
    /// The format of the interval type.
    /// </summary>
    [JsonPropertyName("type_interval_type")]
    public string TypeIntervalType { get; set; }

    /// <summary>
    /// The ordinal position of the column (starting at position 0).
    /// </summary>
    [JsonPropertyName("position")]
    public int Position { get; set; }
}

public static class StatementExecutionSchemaColumnTypes
{
    public static readonly string Boolean = "BOOLEAN";
    public static readonly string Byte = "BYTE";
    public static readonly string Short = "SHORT";
    public static readonly string Int = "INT";
    public static readonly string Long = "LONG";
    public static readonly string Float = "FLOAT";
    public static readonly string Double = "DOUBLE";
    public static readonly string Date = "DATE";
    public static readonly string Timestamp = "TIMESTAMP";
    public static readonly string String = "STRING";
    public static readonly string Binary = "BINARY";
    public static readonly string Decimal = "DECIMAL";
    public static readonly string Interval = "INTERVAL";
    public static readonly string Array = "ARRAY";
    public static readonly string Struct = "STRUCT";
    public static readonly string Map = "MAP";
    public static readonly string Char = "CHAR";
    public static readonly string Null = "NULL";
    public static readonly string UserDefinedType = "USER_DEFINED_TYPE";
}

public record StatementExecutionChunk
{
    /// <summary>
    /// The position within the sequence of result set chunks.
    /// </summary>
    [JsonPropertyName("chunk_index")]
    public int ChunkIndex { get; set; }

    /// <summary>
    /// The starting row offset within the result set.
    /// </summary>
    [JsonPropertyName("row_offset")]
    public long RowOffset { get; set; }

    /// <summary>
    /// The number of rows within the result chunk.
    /// </summary>
    [JsonPropertyName("row_count")]
    public long RowCount { get; set; }

    /// <summary>
    /// The number of bytes in the result chunk. This field is not available when using INLINE disposition.
    /// </summary>
    [JsonPropertyName("byte_count")]
    public long ByteCount { get; set; }
}

public record StatementExecutionResult : StatementExecutionChunk
{
    /// <summary>
    ///  When fetching, provides the chunk_index for the next chunk. If absent, indicates there are no more chunks.
    /// </summary>
    [JsonPropertyName("next_chunk_index")]
    public int NextChunkIndex { get; set; }

    /// <summary>
    /// When fetching, provides a link to fetch the next chunk. If absent, indicates there are no more chunks. This link is an absolute path to be joined with your $DATABRICKS_HOST, and should be treated as an opaque link. This is an alternative to using next_chunk_index.
    /// </summary>
    [JsonPropertyName("next_chunk_internal_link")]
    public string NextChunkInternalLink { get; set; }
}

public record StatementExecutionResultChunk : StatementExecutionResult
{
    /// <summary>
    /// The JSON_ARRAY format is an array of arrays of values, where each non-null value is formatted as a string. Null values are encoded as JSON null.
    /// </summary>
    [JsonPropertyName("data_array")]
    public string[][] DataArray { get; set; } = Array.Empty<string[]>();

    /// <summary>
    /// The list of external links to the result data in cloud storage. This field is not available when using INLINE disposition.
    /// </summary>
    [JsonPropertyName("external_links")]
    public IEnumerable<StatementExecutionExternalLink> ExternalLinks { get; set; } = Array.Empty<StatementExecutionExternalLink>();

    public virtual bool Equals(StatementExecutionResultChunk other)
    {
        if (other is null)
        {
            return false;
        }

        if (DataArray.Length != other.DataArray.Length)
        {
            return false;
        }

        return base.Equals(other)
            && ExternalLinks.SequenceEqual(other.ExternalLinks);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public record StatementExecutionExternalLink : StatementExecutionResult
{
    /// <summary>
    /// A presigned URL pointing to a chunk of result data, hosted by an external service, with a short expiration time (<= 15 minutes). As this URL contains a temporary credential, it should be considered sensitive and the client should not expose this URL in a log.
    /// </summary>
    [JsonPropertyName("external_link")]
    public string ExternalLink { get; set; }

    /// <summary>
    /// Indicates the date-time that the given external link will expire and becomes invalid, after which point a new external_link must be requested.
    /// </summary>
    [JsonPropertyName("expiration")]
    public DateTime Expiration { get; set; }
}
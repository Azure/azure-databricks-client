using System.Text.Json.Serialization;

using Microsoft.Azure.Databricks.Client.Converters;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client;

[JsonSerializable(typeof(SqlStatement))]
[JsonSerializable(typeof(StatementExecution))]
[JsonSerializable(typeof(StatementExecutionResultChunk))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    PropertyNameCaseInsensitive = true,
    Converters = new System.Type[] {
        typeof(JsonStringEnumConverter),
        typeof(MillisecondEpochDateTimeConverter),
        typeof(LibraryConverter),
        typeof(SecretScopeConverter),
        typeof(AclPermissionItemConverter),
        typeof(DepedencyConverter),
        typeof(TableConstraintConverter),
    }
)]
internal sealed partial class DatabricksSerializationContext : JsonSerializerContext;

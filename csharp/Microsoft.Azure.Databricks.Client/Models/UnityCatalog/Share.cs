using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.UnityCatalog;

public record Share : ShareAttributes
{
    /// <summary>
    /// Username of current owner of share.
    /// </summary>
    [JsonPropertyName("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// A list of shared data objects within the share.
    /// </summary>
    [JsonPropertyName("objects")]
    public IEnumerable<ShareObject> Objects { get; set; }

    /// <summary>
    /// Time at which this share was created, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    /// Username of share creator.
    /// </summary>
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Time at which this share was last modified, in epoch milliseconds.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Username of user who last modified share.
    /// </summary>
    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; }

    /// <summary>
    /// Storage Location URL (full path) for the share.
    /// </summary>
    [JsonPropertyName("storage_location")]
    public string StorageLocation { get; set; }
}

public record ShareObject
{
    /// <summary>
    /// A fully qualified name that uniquely identifies a data object.
    /// For example, a table's fully qualified name is in the format of catalog.schema.table.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// A user-provided comment when adding the data object to the share.
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; }

    /// <summary>
    /// A user-provided new name for the data object within the share.
    /// If this new name is not provided, the object's original name will be used as the shared_as name.
    /// The shared_as name must be unique within a share.
    /// For tables, the new name must follow the format of schema.table.
    /// </summary>
    [JsonPropertyName("shared_as")]
    public string SharedAs { get; set; }

    /// <summary>
    /// A user-provided new name for the data object within the share.
    /// If this new name is not provided, the object's original name will be used as the string_shared_as name.
    /// The string_shared_as name must be unique within a share.
    /// For notebooks, the new name should be the new notebook file name.
    /// </summary>
    [JsonPropertyName("string_shared_as")]
    public string StringSharedAs { get; set; }

    /// <summary>
    /// The content of the notebook file when the data object type is NOTEBOOK_FILE.
    /// This should be base64 encoded.
    /// Required for adding a NOTEBOOK_FILE, optional for updating, ignored for other types.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; }

    /// <summary>
    /// Whether to enable or disable sharing of data history.
    /// If not specified, the default is DISABLED.
    /// </summary>
    [JsonPropertyName("history_data_sharing_status")]
    public HistoryDataSharingStatus? HistoryDataSharingStatus { get; set; }

    /// <summary>
    /// Array of partitions for the shared data.
    /// </summary>
    [JsonPropertyName("partitions")]
    public IEnumerable<SharePartition> Partitions { get; set; }

    /// <summary>
    /// Whether to enable cdf or indicate if cdf is enabled on the shared object.
    /// </summary>
    [JsonPropertyName("cdf_enabled")]
    public bool? CdfEnabled { get; set; }

    /// <summary>
    /// The start version associated with the object.
    /// This allows data providers to control the lowest object version that is accessible by clients.
    /// If specified, clients can query snapshots or changes for versions >= start_version.
    /// If not specified, clients can only query starting from the version of the object at the time it was added to the share.
    /// NOTE: The start_version should be less or equal the current version of the object.
    /// </summary>
    [JsonPropertyName("start_version")]
    public long? StartVersion { get; set; }

    /// <summary>
    /// The type of the data object.
    /// Enum: TABLE | SCHEMA | VIEW | MATERIALIZED_VIEW | STREAMING_TABLE | MODEL | NOTEBOOK_FILE | FUNCTION | FEATURE_SPEC
    /// </summary>
    [JsonPropertyName("data_object_type")]
    public DataObjectType? DataObjectType { get; set; }

    /// <summary>
    /// The time when this data object is added to the share.
    /// </summary>
    [JsonPropertyName("added_at")]
    public DateTimeOffset? AddedAt { get; set; }

    /// <summary>
    /// Username of the sharer.
    /// </summary>
    [JsonPropertyName("added_by")]
    public string AddedBy { get; set; }

    /// <summary>
    /// One of: ACTIVE, PERMISSION_DENIED.
    /// </summary>
    [JsonPropertyName("status")]
    public ShareObjectStatus? Status { get; set; }
}

public enum DataObjectType
{
    TABLE,
    SCHEMA,
    VIEW,
    MATERIALIZED_VIEW,
    STREAMING_TABLE,
    MODEL,
    NOTEBOOK_FILE,
    FUNCTION,
    FEATURE_SPEC
}

public enum ShareObjectStatus
{
    ACTIVE,
    PERMISSION_DENIED
}

public enum HistoryDataSharingStatus
{
    ENABLED,
    DISABLED
}

public enum ShareObjectUpdateAction
{
    ADD,
    REMOVE,
    UPDATE
}

public record ShareObjectUpdate
{
    /// <summary>
    /// One of: ADD, REMOVE, UPDATE.
    /// </summary>
    [JsonPropertyName("action")]
    public ShareObjectUpdateAction? Action { get; set; }

    /// <summary>
    /// The data object that is being added, removed, or updated.
    /// </summary>
    [JsonPropertyName("data_object")]
    public ShareObject Object { get; set; }
}

public record SharePartition
{
    /// <summary>
    /// An array of partition values.
    /// </summary>
    [JsonPropertyName("values")]
    public IEnumerable<SharePartitionValue> Values { get; set; }
}

public record SharePartitionValue
{
    /// <summary>
    /// The name of the partition column.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The value of the partition column. When this value is not set, it means null value.
    /// When this field is set, field recipient_property_key can not be set.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// The key of a Delta Sharing recipient's property. For example "databricks-account-id".
    /// When this field is set, field value can not be set.
    /// </summary>
    [JsonPropertyName("recipient_property_key")]
    public string RecipientPropertyKey { get; set; }

    /// <summary>
    /// The operator to apply for the value.
    /// Enum: EQUAL | LIKE
    /// </summary>
    [JsonPropertyName("op")]
    public string Op { get; set; }
}

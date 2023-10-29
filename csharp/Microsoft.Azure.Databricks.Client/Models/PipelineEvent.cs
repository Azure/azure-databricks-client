using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record PipelineEventsList
{
    /// <summary>
    /// If present, a token to fetch the next page of events.
    /// </summary>
    [JsonPropertyName("next_page_token")]
    public string NextPageToken { get; set; }

    /// <summary>
    /// If present, a token to fetch the previous page of events.
    /// </summary>
    [JsonPropertyName("prev_page_token")]
    public string PreviousPageToken { get; set; }

    /// <summary>
    /// The list of events matching the request criteria.
    /// </summary>
    [JsonPropertyName("events")]
    public IEnumerable<PipelineEvent> Events { get; set; }
}

public record PipelineEvent
{
    /// <summary>
    /// A time-based, globally unique id.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// A sequencing object to identify and order events.
    /// </summary>
    [JsonPropertyName("sequence")]
    public PipelineEventSequence Sequence { get; set; }

    /// <summary>
    /// Describes where the event originates from.
    /// </summary>
    [JsonPropertyName("origin")]
    public PipelineOrigin Origin { get; set; }

    /// <summary>
    /// The time of the event.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }

    /// <summary>
    /// The display message associated with the event.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// The severity level of the event.
    /// </summary>
    [JsonPropertyName("level")]
    public PipelineEventLevel? Level { get; set; }

    /// <summary>
    /// Information about an error captured by the event.
    /// </summary>
    [JsonPropertyName("error")]
    public PipelineEventError Error { get; set; }

    /// <summary>
    /// The event type. Should always correspond to the details
    /// </summary>
    [JsonPropertyName("event_type")]
    public string EventType { get; set; }

    /// <summary>
    /// Maturity level for event_type.
    /// </summary>
    [JsonPropertyName("maturity_level")]
    public MaturityLevel? MaturityLevel { get; set; }


}

public record PipelineEventSequence
{
    /// <summary>
    /// the ID assigned by the data plane.
    /// </summary>
    [JsonPropertyName("data_plane_id")]
    public PipelineEventSequenceDataPlane DataPlaneId { get; set; }

    /// <summary>
    /// A sequence number, unique and increasing within the control plane.
    /// </summary>
    [JsonPropertyName("control_plane_seq_no")]
    public long ControlPlaneSeqNo { get; set; }
}

public record PipelineEventSequenceDataPlane
{
    /// <summary>
    /// The instance name of the data plane emitting an event.
    /// </summary>
    [JsonPropertyName("instance")]
    public string Instance { get; set; }

    /// <summary>
    /// A sequence number, unique and increasing within the data plane instance.
    /// </summary>
    [JsonPropertyName("seq_no")]
    public object SequenceNo { get; set; }
}

public record PipelineOrigin
{
    /// <summary>
    /// The cloud provider, e.g., AWS or Azure.
    /// </summary>
    [JsonPropertyName("cloud")]
    public string Cloud { get; set; }

    /// <summary>
    /// The cloud region.
    /// </summary>
    [JsonPropertyName("region")]
    public string Region { get; set; }

    /// <summary>
    /// The org id of the user. Unique within a cloud.
    /// </summary>
    [JsonPropertyName("org_id")]
    public long OrgId { get; set; }

    /// <summary>
    /// The id of the pipeline. Globally unique.
    /// </summary>
    [JsonPropertyName("pipeline_id")]
    public string PipelineId { get; set; }

    /// <summary>
    /// The name of the pipeline. Not unique.
    /// </summary>
    [JsonPropertyName("pipeline_name")]
    public string PipelineName { get; set; }

    /// <summary>
    /// The id of the cluster where an execution happens. Unique within a region.
    /// </summary>
    [JsonPropertyName("cluster_id")]
    public string ClustedId { get; set; }

    /// <summary>
    /// The id of an execution. Globally unique.
    /// </summary>
    [JsonPropertyName("update_id")]
    public string UpdateId { get; set; }

    /// <summary>
    /// The id of a maintenance run. Globally unique.
    /// </summary>
    [JsonPropertyName("maintenance_id")]
    public string MaintenanceId { get; set; }

    /// <summary>
    /// The id of a (delta) table. Globally unique.
    /// </summary>
    [JsonPropertyName("table_id")]
    public string TableId { get; set; }

    /// <summary>
    /// The name of a dataset. Unique within a pipeline.
    /// </summary>
    [JsonPropertyName("dataset_name")]
    public string DatasetName { get; set; }

    /// <summary>
    /// The id of the flow. Globally unique. Incremental queries will generally reuse the same id while complete queries will have a new id per update.
    /// </summary>
    [JsonPropertyName("flow_id")]
    public string FlowId { get; set; }

    /// <summary>
    /// The name of the flow. Not unique.
    /// </summary>
    [JsonPropertyName("flow_name")]
    public string FlowName { get; set; }

    /// <summary>
    /// The id of a batch. Unique within a flow.
    /// </summary>
    [JsonPropertyName("batch_id")]
    public long BatchId { get; set; }

    /// <summary>
    /// The id of the request that caused an update.
    /// </summary>
    [JsonPropertyName("request_id")]
    public string RequestId { get; set; }

    /// <summary>
    /// The Unity Catalog id of the MV or ST being updated.
    /// </summary>
    [JsonPropertyName("uc_resource_id")]
    public string UcResourceId { get; set; }

    /// <summary>
    /// The optional host name where the event was triggered.
    /// </summary>
    [JsonPropertyName("host")]
    public string Host { get; set; }

    /// <summary>
    /// Materialization name.
    /// </summary>
    [JsonPropertyName("materialization_name")]
    public string MaterializationName { get; set; }
}

public record PipelineEventError
{
    /// <summary>
    /// Whether this error is considered fatal, that is, unrecoverable.
    /// </summary>
    [JsonPropertyName("fatal")]
    public bool Fatal { get; set; }

    /// <summary>
    /// The exception thrown for this error, with its chain of cause.
    /// </summary>
    [JsonPropertyName("exceptions")]
    public IEnumerable<PipelineEventException> Exceptions { get; set; }
}

public record PipelineEventException
{
    /// <summary>
    /// Runtime class of the exception
    /// </summary>
    [JsonPropertyName("class_name")]
    public string ClassName { get; set; }

    /// <summary>
    /// Exception message
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// Stack trace consisting of a list of stack frames
    /// </summary>
    [JsonPropertyName("stack")]
    public IEnumerable<PipelineEventExceptionStack> Stack { get; set; }
}

public record PipelineEventExceptionStack
{
    /// <summary>
    /// Class from which the method call originated
    /// </summary>
    [JsonPropertyName("declaring_class")]
    public string DeclaringClass { get; set; }

    /// <summary>
    /// Name of the method which was called
    /// </summary>
    [JsonPropertyName("method_name")]
    public string MethodName { get; set; }

    /// <summary>
    /// File where the method is defined
    /// </summary>
    [JsonPropertyName("file_name")]
    public string FileName { get; set; }

    /// <summary>
    /// Line from which the method was called
    /// </summary>
    [JsonPropertyName("line_number")]
    public long LineNumber { get; set; }
}

public enum PipelineEventLevel
{
    INFO,
    WARN,
    ERROR,
    METRICS
}

public enum MaturityLevel
{
    STABLE,
    EVOLVING,
    DEPRECATED
}
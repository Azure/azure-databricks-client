using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models.MachineLearning.Experiment;

public record Run
{
    [JsonPropertyName("info")]
    public Info Info { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }

    [JsonPropertyName("inputs")]
    public Inputs Inputs { get; set; }
}

public record Info
{
    [JsonPropertyName("run_id")]
    public string RunId { get; set; }

    [JsonPropertyName("experiment_id")]
    public string ExperimentId { get; set; }

    [Obsolete("This field is deprecated as of MLflow 1.0, and will be removed in a future MLflow release. Use 'mlflow.user' tag instead.")]
    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("start_time")]
    public DateTimeOffset? StartTime { get; set; }

    [JsonPropertyName("end_time")]
    public DateTimeOffset? EndTime { get; set; }

    [JsonPropertyName("artifact_uri")]
    public string ArtifactUri { get; set; }

    [JsonPropertyName("lifecycle_stage")]
    public string LifecycleStage { get; set; }
}

public record Data
{
    [JsonPropertyName("metrics")]
    public IEnumerable<Metric> Metrics { get; set; }

    [JsonPropertyName("params")]
    public IEnumerable<Param> Params { get; set; }

    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; }
}

public record Metric
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; set; }

    [JsonPropertyName("step")]
    public string Step { get; set; }
}

public record Param
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public record Tag
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}

public record Inputs
{
    [JsonPropertyName("dataset_inputs")]
    public IEnumerable<DatasetInput> DatasetInputs { get; set; }
}

public record DatasetInput
{
    [JsonPropertyName("tags")]
    public IEnumerable<Tag> Tags { get; set; }

    [JsonPropertyName("dataset")]
    public Dataset Dataset { get; set; }
}

public record Dataset
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    [JsonPropertyName("source_type")]
    public string SourceType { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("schema")]
    public string Schema { get; set; }

    [JsonPropertyName("profile")]
    public string Profile { get; set; }
}

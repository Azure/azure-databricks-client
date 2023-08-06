// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// A warehouses and its configurations.
    /// </summary>
    public record WarehouseAttributes
    {
        /// <summary>
        /// The logical name for the cluster.
        /// </summary>
        /// <value>The logical name for the cluster.</value>
        /// <remarks>
        /// Supported values:
        /// - Must be unique within an org.
        /// - Must be less than 100 characters.
        /// </remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The size of the clusters allocated for this warehouse. Increasing the size of a spark cluster allows you to run larger queries on it. If you want to increase the number of concurrent queries, please tune max_num_clusters.
        /// </summary>
        /// <value>The size of the clusters allocated for this warehouse.</value>
        /// <remarks>
        /// Supported values:
        /// - 2X-Small
        /// - X-Small
        /// - Small
        /// - Medium
        /// - Large
        /// - X-Large
        /// - 2X-Large
        /// - 3X-Large
        /// - 4X-Large
        /// </remarks>
        [JsonPropertyName("cluster_size")]
        public string ClusterSize { get; set; }

        /// <summary>
        /// The minimum number of available clusters that will be maintained for this SQL warehouse. Increasing this will ensure that a larger number of clusters are always running and therefore may reduce the cold start time for new queries. This is similar to reserved vs. revocable cores in a resource manager.
        /// </summary>
        /// <value>The minimum number of available clusters that will be maintained for this SQL warehouse.</value>
        /// <remarks>
        /// Supported values:
        /// - Must be > 0
        /// - Must be <= min(max_num_clusters, 30)
        /// - Defaults to 1
        /// </remarks>
        [JsonPropertyName("min_num_clusters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int MinNumClusters { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of clusters that the autoscaler will create to handle concurrent queries.
        /// </summary>
        /// <value>The maximum number of clusters that the autoscaler will create to handle concurrent queries.</value>
        /// <remarks>
        /// Supported values:
        /// - Must be >= min_num_clusters
        /// - Must be <= 30.
        /// - Defaults to min_clusters if unset.
        /// </remarks>
        [JsonPropertyName("max_num_clusters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int MaxNumClusters { get; set; }

        /// <summary>
        /// Gets or sets the amount of time in minutes that a SQL warehouse must be idle (i.e., no RUNNING queries) before it is automatically stopped.
        /// </summary>
        /// <value>The amount of time in minutes that a SQL warehouse must be idle (i.e., no RUNNING queries) before it is automatically stopped.</value>
        /// <remarks>
        /// Supported values:
        /// - Must be == 0 or >= 10 mins
        /// - 0 indicates no autostop.
        /// - Defaults to 120 mins
        /// </remarks>
        [JsonPropertyName("auto_stop_mins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public int AutoStopMins { get; set; } = 120;

        /// <summary>
        /// Gets or sets the warehouse creator name.
        /// </summary>
        /// <value>The warehouse creator name.</value>
        [JsonPropertyName("creator_name")]
        public string CreatorName { get; set; }

        /// <summary>
        /// Gets or sets the set of key-value pairs that will be tagged on all resources (e.g., AWS instances and EBS volumes) associated with this SQL warehouse.
        /// </summary>
        /// <value>The set of key-value pairs that will be tagged on all resources associated with this SQL warehouse.</value>
        /// <remarks>
        /// Supported values:
        /// - Number of tags < 45.
        /// </remarks>
        [JsonPropertyName("tags")]
        public Tags Tags { get; set; }

        /// <summary>
        /// Gets or sets the configurations whether the warehouse should use spot instances.
        /// </summary>
        /// <value>The configurations whether the warehouse should use spot instances.</value>
        /// <remarks>
        /// Supported values:
        /// - "POLICY_UNSPECIFIED"
        /// - "COST_OPTIMIZED"
        /// - "RELIABILITY_OPTIMIZED"
        /// </remarks>
        [JsonPropertyName("spot_instance_policy")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public SpotInstancePolicy SpotInstancePolicy { get; set; } = SpotInstancePolicy.POLICY_UNSPECIFIED;

        /// <summary>
        /// Gets or sets a value indicating whether the warehouse should use Photon optimized clusters.
        /// </summary>
        /// <value><c>true</c> if the warehouse should use Photon optimized clusters; otherwise, <c>false</c>.</value>
        /// <remarks>Defaults to false.</remarks>
        [JsonPropertyName("enable_photon")]
        public bool EnablePhoton { get; set; }

        /// <summary>
        /// Gets or sets the channel details.
        /// </summary>
        /// <value>The channel details.</value>
        [JsonPropertyName("channel")]
        public Channel Channel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the warehouse should use serverless compute.
        /// </summary>
        /// <value><c>true</c> if the warehouse should use serverless compute; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Databricks strongly recommends that you always explicitly set this field.
        /// If omitted, the default is false for most workspaces.
        /// However, if this workspace used the SQL Warehouses API to create a warehouse between September 1, 2022 and April 30, 2023, the default remains the previous behavior which is default to true if the workspace is enabled for serverless and fits the requirements for serverless SQL warehouses.
        /// To avoid ambiguity, especially for organizations with many workspaces, Databricks recommends that you always set this field.
        /// If your account needs updated terms of use, workspace admins are prompted in the Databricks SQL UI.
        /// A workspace must meet the requirements and might require an update to its instance profile role to add a trust relationship.
        /// </remarks>
        [JsonPropertyName("enable_serverless_compute")]
        public bool EnableServerlessCompute { get; set; }

        /// <summary>
        /// Gets or sets the warehouse type.
        /// </summary>
        /// <value>The warehouse type.</value>
        /// <remarks>
        /// Supported values:
        /// - "TYPE_UNSPECIFIED"
        /// - "CLASSIC"
        /// - "PRO"
        /// If you want to use serverless compute, you must set to PRO and also set the field enable_serverless_compute to true.
        /// </remarks>
        [JsonPropertyName("warehouse_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public WarehouseType WarehouseType { get; set; } = WarehouseType.TYPE_UNSPECIFIED;
    }

    public record WarehouseInfo : WarehouseAttributes
    {
        /// <summary>
        /// The unique identifier for warehouse.
        /// </summary>
        /// <value>The unique identifier for warehouse.</value>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the current number of clusters running for the service.
        /// </summary>
        /// <value>The current number of clusters running for the service.</value>
        [JsonPropertyName("num_clusters")]
        public int NumClusters { get; set; }

        /// <summary>
        /// Gets or sets the current number of active sessions for the warehouse.
        /// </summary>
        /// <value>The current number of active sessions for the warehouse.</value>
        [JsonPropertyName("num_active_sessions")]
        public long NumActiveSessions { get; set; }

        /// <summary>
        /// Gets or sets the state of the warehouse.
        /// </summary>
        /// <value>The state of the warehouse.</value>
        /// <remarks>
        /// Supported values:
        /// - "STARTING"
        /// - "RUNNING"
        /// - "STOPPING"
        /// - "STOPPED"
        /// - "DELETING"
        /// - "DELETED"
        /// </remarks>
        [JsonPropertyName("state")]
        public WarehouseState State { get; set; }

        /// <summary>
        /// Gets or sets the JDBC connection string for this warehouse.
        /// </summary>
        /// <value>The JDBC connection string for this warehouse.</value>
        [JsonPropertyName("jdbc_url")]
        public string JDBCUrl { get; set; }

        /// <summary>
        /// Gets or sets the ODBC parameters for the SQL warehouse.
        /// </summary>
        /// <value>The ODBC parameters for the SQL warehouse.</value>
        [JsonPropertyName("odbc_params")]
        public ODBCParams ODBCParams { get; set; }

        /// <summary>
        /// Gets or sets the optional health status. Assume the warehouse is healthy if this field is not set.
        /// </summary>
        /// <value>The optional health status.</value>
        [JsonPropertyName("health")]
        public Health Health { get; set; }
    }

    public record Tags
    {
        [JsonPropertyName("custom_tags")]
        public List<Dictionary<string, string>> CustomTags { get; set; }
    }

    /// <summary>
    /// Represents the configurations whether the warehouse should use spot instances.
    /// </summary>
    public enum SpotInstancePolicy
    {
        POLICY_UNSPECIFIED,
        COST_OPTIMIZED,
        RELIABILITY_OPTIMIZED
    }

    /// <summary>
    /// Represents the channel details.
    /// </summary>
    public record Channel
    {
        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        /// <value>The name of the channel.</value>
        /// <remarks>
        /// Supported values:
        /// - "CHANNEL_NAME_UNSPECIFIED"
        /// - "CHANNEL_NAME_PREVIEW"
        /// - "CHANNEL_NAME_CURRENT"
        /// - "CHANNEL_NAME_PREVIOUS"
        /// - "CHANNEL_NAME_CUSTOM"
        /// </remarks>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ChannelName Name { get; set; } = ChannelName.CHANNEL_NAME_UNSPECIFIED;

        /// <summary>
        /// Gets or sets the DBSQL version.
        /// </summary>
        /// <value>The DBSQL version.</value>
        [JsonPropertyName("dbsql_version")]
        public string DBSQLVersion { get; set; }
    }

    /// <summary>
    /// Represents the warehouse type.
    /// </summary>
    public enum WarehouseType
    {
        TYPE_UNSPECIFIED,
        CLASSIC,
        PRO
    }

    /// <summary>
    /// Represents the ODBC parameters for the SQL warehouse.
    /// </summary>
    public record ODBCParams
    {
        /// <summary>
        /// Gets or sets the ODBC hostname.
        /// </summary>
        /// <value>The ODBC hostname.</value>
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the ODBC path.
        /// </summary>
        /// <value>The ODBC path.</value>
        [JsonPropertyName("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the ODBC protocol.
        /// </summary>
        /// <value>The ODBC protocol.</value>
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the ODBC port.
        /// </summary>
        /// <value>The ODBC port.</value>
        [JsonPropertyName("port")]
        public int Port { get; set; }
    }

    /// <summary>
    /// Represents the optional health status.
    /// </summary>
    public record Health
    {
        /// <summary>
        /// Gets or sets the health status of the warehouse.
        /// </summary>
        /// <value>The health status of the warehouse.</value>
        /// <remarks>
        /// Supported values:
        /// - "STATUS_UNSPECIFIED"
        /// - "HEALTHY"
        /// - "DEGRADED"
        /// - "FAILED"
        /// </remarks>
        [JsonPropertyName("status")]
        public HealthStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the reason for failure to bring up clusters for this warehouse. This is available when status is 'FAILED' and sometimes when it is DEGRADED.
        /// </summary>
        /// <value>The reason for failure to bring up clusters for this warehouse.</value>
        [JsonPropertyName("failure_reason")]
        public FailureReason FailureReason { get; set; }

        /// <summary>
        /// Gets or sets a short summary of the health status in case of degraded/failed warehouses.
        /// </summary>
        /// <value>A short summary of the health status in case of degraded/failed warehouses.</value>
        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets details about errors that are causing current degraded/failed status.
        /// </summary>
        /// <value>Details about errors that are causing current degraded/failed status.</value>
        [JsonPropertyName("details")]
        public string Details { get; set; }
    }

    /// <summary>
    /// Represents the reason for failure to bring up clusters for the warehouse.
    /// </summary>
    public record FailureReason
    {
        /// <summary>
        /// Gets or sets the status code indicating why the cluster was terminated.
        /// </summary>
        /// <value>The status code indicating why the cluster was terminated.</value>
        [JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the type of the termination.
        /// </summary>
        /// <value>The type of the termination.</value>
        /// <remarks>
        /// Supported values:
        /// - "SUCCESS"
        /// - "CLIENT_ERROR"
        /// - "SERVICE_FAULT"
        /// - "CLOUD_FAILURE"
        /// </remarks>
        [JsonPropertyName("type")]
        public TerminationType Type { get; set; }
    }

    /// <summary>
    /// Represents the channel name.
    /// </summary>
    public enum ChannelName
    {
        CHANNEL_NAME_UNSPECIFIED,
        CHANNEL_NAME_PREVIEW,
        CHANNEL_NAME_CURRENT,
        CHANNEL_NAME_PREVIOUS,
        CHANNEL_NAME_CUSTOM
    }

    /// <summary>
    /// Represents the health status of the warehouse.
    /// </summary>
    public enum HealthStatus
    {
        STATUS_UNSPECIFIED,
        HEALTHY,
        DEGRADED,
        FAILED
    }

    /// <summary>
    /// Represents the type of termination.
    /// </summary>
    public enum TerminationType
    {
        SUCCESS,
        CLIENT_ERROR,
        SERVICE_FAULT,
        CLOUD_FAILURE
    }

    /// <summary>
    /// Represents the state of the warehouse.
    /// </summary>
    public enum WarehouseState
    {
        STARTING,
        RUNNING,
        STOPPING,
        STOPPED,
        DELETING,
        DELETED
    }
}

using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    public abstract record AccessControlRequest
    {
        [JsonPropertyName("permission_level")]
        public virtual JobPermissionLevel PermissionLevel { get; set; }
    }

    public record AccessControlRequestForUser : AccessControlRequest
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }
    }

    public record AccessControlRequestForGroup : AccessControlRequest
    {
        [JsonPropertyName("group_name")]
        public string GroupName { get; set; }

        [JsonPropertyName("permission_level")]
        public override JobPermissionLevel PermissionLevel
        {
            get => base.PermissionLevel;
            set
            {
                if (value == JobPermissionLevel.IS_OWNER)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "PermissionLevel for AccessControlRequestForGroup cannot be IS_OWNER.");
                }

                base.PermissionLevel = value;
            }
        }
    }

    public record AccessControlRequestForServicePrincipal : AccessControlRequest
    {
        [JsonPropertyName("service_principal_name")]
        public string ServicePrincipalName { get; set; }
    }

    public enum JobPermissionLevel
    {
        /// <summary>
        /// Permission to manage the job.
        /// </summary>
        CAN_MANAGE,

        /// <summary>
        /// Permission to run and/or manage runs for the job.
        /// </summary>
        CAN_MANAGE_RUN,

        /// <summary>
        /// Permission to view the settings of the job.
        /// </summary>
        CAN_VIEW,

        /// <summary>
        /// Permission that represents ownership of the job.
        /// </summary>
        IS_OWNER
    }
}

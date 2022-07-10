using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models
{
    /// <summary>
    /// An optional specification for a remote repository containing the notebooks used by this job's notebook tasks.
    /// </summary>
    public record GitSource
    {
        /// <summary>
        /// URL of the repository to be cloned by this job.
        /// The maximum length is 300 characters.
        /// </summary>
        /// <example>
        /// https://github.com/databricks/databricks-cli
        /// </example>
        [JsonPropertyName("git_url")]
        public string GitUrl { get; set; }

        /// <summary>
        /// Unique identifier of the service used to host the Git repository. The value is case insensitive.
        /// </summary>
        /// <example>
        /// github
        /// </example>
        [JsonPropertyName("git_provider")]
        public GitProvider GitProvider { get; set; }

        /// <summary>
        /// Name of the branch to be checked out and used by this job. This field cannot be specified in conjunction with git_tag or git_commit.
        /// The maximum length is 255 characters.
        /// </summary>
        /// <example>
        /// main
        /// </example>
        [JsonPropertyName("git_branch")]
        public string GitBranch { get; set; }

        /// <summary>
        /// Name of the tag to be checked out and used by this job. This field cannot be specified in conjunction with git_branch or git_commit.
        /// The maximum length is 255 characters.
        /// </summary>
        /// <example>
        /// release-1.0.0
        /// </example>
        [JsonPropertyName("git_tag")]
        public string GitTag { get; set; }

        /// <summary>
        /// Commit to be checked out and used by this job. This field cannot be specified in conjunction with git_branch or git_tag.
        /// The maximum length is 64 characters.
        /// </summary>
        /// <example>
        /// e0056d01
        /// </example>
        [JsonPropertyName("git_commit")]
        public string GitCommit { get; set; }

        /// <summary>
        /// Read-only state of the remote repository at the time the job was run. This field is only included on job runs.
        /// </summary>
        [JsonPropertyName("git_snapshot")]
        public GitSnapshot GitSnapshot { get; set; }
    }
    public class GitSnapshot
    {
        /// <summary>
        /// Commit that was used to execute the run. If git_branch was specified, this points to the HEAD of the branch at the time of the run; if git_tag was specified, this points to the commit the tag points to.
        /// </summary>
        /// <example>
        /// 4506fdf41e9fa98090570a34df7a5bce163ff15f
        /// </example>
        [JsonPropertyName("used_commit")]
        public string UsedCommit { get; set; }
    }

    /// <summary>
    /// Unique identifier of the service used to host the Git repository. The value is case insensitive.
    /// </summary>
    public enum GitProvider
    {
        gitHub,
        bitbucketCloud,
        azureDevOpsServices,
        gitHubEnterprise,
        bitbucketServer,
        gitLab,
        gitLabEnterpriseEdition,
        awsCodeCommit
    }
}

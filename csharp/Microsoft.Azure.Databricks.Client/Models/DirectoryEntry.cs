using System;
using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record DirectoryEntry
{
    /// <summary>
    /// The absolute path of the file or directory.
    /// </summary>
    [JsonPropertyName("path")]
    public string Path { get; set; }

    /// <summary>
    /// True if the path is a directory.
    /// </summary>
    [JsonPropertyName("is_directory")]
    public bool IsDirectory { get; set; }

    /// <summary>
    /// The length of the file in bytes. This field is omitted for directories.
    /// </summary>
    [JsonPropertyName("file_size")]
    public long FileSize { get; set; }

    /// <summary>
    /// Last modification time of given file in milliseconds since unix epoch.
    /// </summary>
    [JsonPropertyName("last_modified")]
    public DateTimeOffset? LastModified { get; set; }

    /// <summary>
    /// The name of the file or directory. This is the last component of the path.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

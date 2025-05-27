using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IFilesApi : IDisposable
    {
        /// <summary>
        /// Returns the contents of a directory.
        /// If there is no directory at the specified path, the API returns an HTTP 404 error.
        /// </summary>
        /// <param name="directoryPath">The absolute path of a directory.</param>
        /// <param name="pageSize">
        /// The maximum number of directory entries to return. The response may contain fewer entries. If the response contains a next_page_token, there may be more entries, even if fewer than page_size entries are in the response.
        /// We recommend not to set this value unless you are intentionally listing less than the complete directory contents.
        /// If unspecified, at most 1000 directory entries will be returned. The maximum value is 1000. Values above 1000 will be coerced to 1000.
        /// </param>
        /// <param name="pageToken">
        /// An opaque page token which was the next_page_token in the response of the previous request to list the contents of this directory.
        /// Provide this token to retrieve the next page of directory entries.
        /// When providing a page_token, all other parameters provided to the request must match the previous request.
        /// To list all the entries in a directory, it is necessary to continue requesting pages of entries until the response contains no next_page_token.
        /// Note that the number of entries returned must not be used to determine when the listing is complete.
        /// </param>
        Task<DirectoriesList> ListDirectoryContents(string directoryPath, long? pageSize = default, string pageToken = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the metadata of a directory. The response HTTP headers contain the metadata. There is no response body.
        /// This method is useful to check if a directory exists and the caller has access to it.
        /// If you wish to ensure the directory exists, you can instead use PUT, which will create the directory if it does not exist, and is idempotent (it will succeed if the directory already exists).
        /// </summary>
        /// <param name="directoryPath">The absolute path of a directory.</param>
        Task<HttpContentHeaders> GetDirectoryMetadata(string directoryPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates an empty directory.
        /// If necessary, also creates any parent directories of the new, empty directory (like the shell command mkdir -p).
        /// If called on an existing directory, returns a success response; this method is idempotent (it will succeed if the directory already exists).
        /// </summary>
        /// <param name="directoryPath">The absolute path of a directory.</param>
        Task CreateDirectory(string directoryPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an empty directory.
        /// To delete a non-empty directory, first delete all of its contents.
        /// This can be done by listing the directory contents and deleting each file and subdirectory recursively.
        /// </summary>
        /// <param name="directoryPath">The absolute path of a directory.</param>
        Task DeleteDirectory(string directoryPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file.
        /// The file contents are the response body.
        /// This is a standard HTTP file download, not a JSON RPC. It supports the `Range` and `If-Unmodified-Since` HTTP headers.
        ///
        /// The file contents will be written asynchronously to the stream passed as argument.
        /// </summary>
        /// <param name="filePath">The absolute path of the file.</param>
        /// <param name="stream">The data stream to write to.</param>
        /// <param name="range">The range of bytes to retrieve. The range is inclusive and zero-based, see RFC 9110 for further details.</param>
        /// <param name="ifUnmodifiedSince">Download the file only if it has not been modified since the specified timestamp. If it has, a 412 Precondition Failed error will be returned. See RFC 9110 for further details.</param>
        Task Download(string filePath, Stream stream, string range = default, string ifUnmodifiedSince = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the metadata of a file.
        /// The response HTTP headers contain the metadata.
        /// There is no response body.
        /// </summary>
        /// <param name="filePath">The absolute path of the file.</param>
        /// <param name="range">The range of bytes to retrieve. The range is inclusive and zero-based, see RFC 9110 for further details.</param>
        /// <param name="ifUnmodifiedSince">Download the file only if it has not been modified since the specified timestamp. If it has, a 412 Precondition Failed error will be returned. See RFC 9110 for further details.</param>
        Task<HttpContentHeaders> GetFileMetadata(string filePath, string range = default, string ifUnmodifiedSince = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file up to 5 GiB.
        /// The file contents should be sent as the request body as raw bytes (an octet stream); do not encode or otherwise modify the bytes before sending.
        /// The contents of the resulting file will be exactly the bytes sent in the request body.
        /// If the request is successful, there is no response body.
        /// </summary>
        /// <param name="filePath">The absolute path of the file.</param>
        /// <param name="stream">The data stream to read from.</param>
        /// <param name="overwrite">If true or unspecified, an existing file will be overwritten. If false, an error will be returned if the path points to an existing file.</param>
        Task Upload(string filePath, Stream stream, bool? overwrite = default, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file.
        /// If the request is successful, there is no response body.
        /// </summary>
        /// <param name="filePath">The absolute path of the file.</param>
        Task Delete(string filePath, CancellationToken cancellationToken = default);
    }
}

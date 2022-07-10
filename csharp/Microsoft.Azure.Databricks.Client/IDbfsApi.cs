using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = Microsoft.Azure.Databricks.Client.Models.FileInfo;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IDbfsApi : IDisposable
    {
        /// <summary>
        /// Opens a stream to write to a file and returns a handle to this stream. There is a 10 minute idle timeout on this handle. If a file or directory already exists on the given path and overwrite is set to false, this call will throw an exception with RESOURCE_ALREADY_EXISTS. A typical workflow for file upload would be:
        /// 1. Issue a create call and get a handle.
        /// 2. Issue one or more add-block calls with the handle you have.
        /// 3. Issue a close call with the handle you have.
        /// </summary>
        /// <param name="path">The path of the new file. The path should be the absolute DBFS path (e.g. “/mnt/foo.txt”). This field is required.</param>
        /// <param name="overwrite">The flag that specifies whether to overwrite existing file/files.</param>
        /// <returns>Handle which should subsequently be passed into the AddBlock and Close calls when writing to a file through a stream.</returns>
        Task<long> Create(string path, bool overwrite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Appends a block of data to the stream specified by the input handle. If the handle does not exist, this call will throw an exception with RESOURCE_DOES_NOT_EXIST. If the block of data exceeds 1 MB, this call will throw an exception with MAX_BLOCK_SIZE_EXCEEDED.
        /// </summary>
        /// <param name="fileHandle">The handle on an open stream. This field is required.</param>
        /// <param name="data">The base64-encoded data to append to the stream. This has a limit of 1 MB. This field is required.</param>
        /// <returns></returns>
        Task AddBlock(long fileHandle, byte[] data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Closes the stream specified by the input handle. If the handle does not exist, this call will throw an exception with RESOURCE_DOES_NOT_EXIST.
        /// </summary>
        /// <param name="fileHandle">The handle on an open stream. This field is required.</param>
        Task Close(long fileHandle, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a stream to the specified path
        /// </summary>
        Task Upload(string path, bool overwrite, Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the file or directory (optionally recursively delete all files in the directory). This call will throw an exception with IO_ERROR if the path is a non-empty directory and recursive is set to false or on other similar errors.
        /// </summary>
        /// <param name="path">The path of the file or directory to delete. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        /// <param name="recursive">Whether or not to recursively delete the directory’s contents. Deleting empty directories can be done without providing the recursive flag.</param>
        /// <returns></returns>
        Task Delete(string path, bool recursive, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the contents of a directory, or details of the file. If the file or directory does not exist, this call will throw an exception with RESOURCE_DOES_NOT_EXIST.
        /// </summary>
        /// <param name="path">The path of the file or directory. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        Task<FileInfo> GetStatus(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the specified path.
        /// </summary>
        /// <param name="path">The path of the file or directory. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        Task<IEnumerable<FileInfo>> List(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the given directory and necessary parent directories if they do not exist. If there exists a file (not a directory) at any prefix of the input path, this call will throw an exception with RESOURCE_ALREADY_EXISTS. Note that if this operation fails it may have succeeded in creating some of the necessary parent directories.
        /// </summary>
        /// <param name="path">The path of the new directory. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        Task Mkdirs(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Move a file from one location to another location within DBFS. If the source file does not exist, this call will throw an exception with RESOURCE_DOES_NOT_EXIST. If there already exists a file in the destination path, this call will throw an exception with RESOURCE_ALREADY_EXISTS. If the given source path is a directory, this call will always recursively move all files.
        /// </summary>
        /// <param name="sourcePath">The source path of the file or directory. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        /// <param name="destinationPath">The destination path of the file or directory. The path should be the absolute DBFS path (e.g. “/mnt/bar/”). This field is required.</param>
        /// <returns></returns>
        Task Move(string sourcePath, string destinationPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a file through the use of multipart form post. It is mainly used for streaming uploads, but can also be used as a convenient single call for data upload. 
        /// </summary>
        /// <param name="path">The path of the new file. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        /// <param name="contents">This parameter might be absent, and instead a posted file will be used.</param>
        /// <param name="overwrite">The flag that specifies whether to overwrite existing file/files.</param>
        Task Put(string path, byte[] contents, bool overwrite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the contents of a file. If the file does not exist, this call will throw an exception with RESOURCE_DOES_NOT_EXIST. If the path is a directory, the read length is negative, or if the offset is negative, this call will throw an exception with INVALID_PARAMETER_VALUE. If the read length exceeds 1 MB, this call will throw an exception with MAX_READ_SIZE_EXCEEDED. If offset + length exceeds the number of bytes in a file, we will read contents until the end of file.
        /// </summary>
        /// <param name="path">The path of the file to read. The path should be the absolute DBFS path (e.g. “/mnt/foo/”). This field is required.</param>
        /// <param name="offset">The offset to read from in bytes.</param>
        /// <param name="length">The number of bytes to read starting from the offset. This has a limit of 1 MB, and a default value of 0.5 MB.</param>
        Task<FileReadBlock> Read(string path, long offset, long length, CancellationToken cancellationToken = default);

        /// <summary>
        /// Download the stream of the specified path to a stream.
        /// </summary>
        Task Download(string path, Stream stream, CancellationToken cancellationToken = default);
    }
}

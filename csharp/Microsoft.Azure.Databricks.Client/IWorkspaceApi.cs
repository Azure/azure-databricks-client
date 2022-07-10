using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IWorkspaceApi : IDisposable
    {
        /// <summary>
        /// Deletes an object or a directory (and optionally recursively deletes all objects in the directory). If path does not exist, this call returns an error RESOURCE_DOES_NOT_EXIST. If path is a non-empty directory and recursive is set to false, this call returns an error DIRECTORY_NOT_EMPTY. Object deletion cannot be undone and deleting a directory recursively is not atomic.
        /// </summary>
        /// <param name="path">The absolute path of the notebook or directory. This field is required.</param>
        /// <param name="recursive">The flag that specifies whether to delete the object recursively. It is false by default. Please note this deleting directory is not atomic. If it fails in the middle, some of objects under this directory may be deleted and cannot be undone.</param>
        Task Delete(string path, bool recursive, CancellationToken cancellationToken = default);

        /// <summary>
        /// Exports a notebook or contents of an entire directory. If path does not exist, this call returns an error RESOURCE_DOES_NOT_EXIST. One can only export a directory in DBC format. If the exported data would exceed size limit, this call returns an error MAX_NOTEBOOK_SIZE_EXCEEDED. This API does not support exporting a library.
        /// </summary>
        /// <param name="path">The absolute path of the notebook or directory. Exporting directory is only support for DBC format. This field is required.</param>
        /// <param name="format">This specifies the format of the exported file. By default, this is SOURCE. However it may be one of: SOURCE, HTML, JUPYTER, DBC. The value is case sensitive.</param>
        Task<byte[]> Export(string path, ExportFormat format, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the status of an object or a directory. If path does not exist, this call returns an error RESOURCE_DOES_NOT_EXIST.
        /// </summary>
        /// <param name="path">The absolute path of the notebook or directory. This field is required.</param>
        Task<ObjectInfo> GetStatus(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Imports a notebook or the contents of an entire directory. If path already exists and overwrite is set to false, this call returns an error RESOURCE_ALREADY_EXISTS. One can only use DBC format to import a directory.
        /// </summary>
        /// <param name="path">The absolute path of the notebook or directory. Importing directory is only support for DBC format. This field is required.</param>
        /// <param name="format">This specifies the format of the file to be imported. By default, this is SOURCE. However it may be one of: SOURCE, HTML, JUPYTER, DBC. The value is case sensitive.</param>
        /// <param name="language">The language. If format is set to SOURCE, this field is required; otherwise, it will be ignored.</param>
        /// <param name="content">The base64-encoded content. This has a limit of 10 MB. If the limit (10MB) is exceeded, exception with error code MAX_NOTEBOOK_SIZE_EXCEEDED will be thrown. This parameter might be absent, and instead a posted file will be used.</param>
        /// <param name="overwrite">The flag that specifies whether to overwrite existing object. It is false by default. For DBC format, overwrite is not supported since it may contain a directory.</param>
        Task Import(string path, ExportFormat format, Language? language, byte[] content, bool overwrite, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the contents of a directory, or the object if it is not a directory. If the input path does not exist, this call returns an error RESOURCE_DOES_NOT_EXIST.
        /// </summary>
        /// <param name="path">The absolute path of the notebook or directory. This field is required.</param>
        Task<IEnumerable<ObjectInfo>> List(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the given directory and necessary parent directories if they do not exists. If there exists an object (not a directory) at any prefix of the input path, this call returns an error RESOURCE_ALREADY_EXISTS. Note that if this operation fails it may have succeeded in creating some of the necessary parrent directories.
        /// </summary>
        /// <param name="path">The absolute path of the directory. If the parent directories do not exist, it will also create them. If the directory already exists, this command will do nothing and succeed. This field is required.</param>
        Task Mkdirs(string path, CancellationToken cancellationToken = default);
    }
}
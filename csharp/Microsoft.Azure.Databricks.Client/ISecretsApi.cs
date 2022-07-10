using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface ISecretsApi : IDisposable
    {
        /// <summary>
        /// Creates a new Databricks-backed secret scope.
        /// The scope name must be unique within a workspace, must consist of alphanumeric characters, dashes, underscores, and periods, and may not exceed 128 characters.The maximum number of scopes in a workspace is 100.
        /// </summary>
        /// <remarks>
        /// If initial_manage_principal is specified, the initial ACL applied to the scope is applied to the supplied principal (user or group) with MANAGE permissions. The only supported principal for this option is the group users, which contains all users in the workspace. If initial_manage_principal is not specified, the initial ACL with MANAGE permission applied to the scope is assigned to the API request issuer’s user identity.
        /// Throws RESOURCE_ALREADY_EXISTS if a scope with the given name already exists.Throws RESOURCE_LIMIT_EXCEEDED if maximum number of scopes in the workspace is exceeded.Throws INVALID_PARAMETER_VALUE if the scope name is invalid.
        /// </remarks>
        /// <param name="scope">Scope name requested by the user. Scope names are unique. This field is required.</param>
        /// <param name="initialManagePrincipal">The principal that is initially granted MANAGE permission to the created scope.</param>
        [Obsolete("This method has been renamed to " + nameof(CreateDatabricksBackedScope) + ".")]
        Task CreateScope(string scope, string initialManagePrincipal, CancellationToken cancellationToken = default);


        /// <summary>
        /// Creates a new Databricks-backed secret scope.
        /// The scope name must be unique within a workspace, must consist of alphanumeric characters, dashes, underscores, and periods, and may not exceed 128 characters.The maximum number of scopes in a workspace is 100.
        /// </summary>
        /// <remarks>
        /// If initial_manage_principal is specified, the initial ACL applied to the scope is applied to the supplied principal (user or group) with MANAGE permissions. The only supported principal for this option is the group users, which contains all users in the workspace. If initial_manage_principal is not specified, the initial ACL with MANAGE permission applied to the scope is assigned to the API request issuer’s user identity.
        /// Throws RESOURCE_ALREADY_EXISTS if a scope with the given name already exists.Throws RESOURCE_LIMIT_EXCEEDED if maximum number of scopes in the workspace is exceeded.Throws INVALID_PARAMETER_VALUE if the scope name is invalid.
        /// </remarks>
        /// <param name="scope">Scope name requested by the user. Scope names are unique. This field is required.</param>
        /// <param name="initialManagePrincipal">The principal that is initially granted MANAGE permission to the created scope.</param>
        Task CreateDatabricksBackedScope(string scope, string initialManagePrincipal, CancellationToken cancellationToken = default);

        /*
         This API call is currently not working per https://github.com/MicrosoftDocs/azure-docs/issues/65000. Comment out for now.
        /// <summary>
        /// Creates a new Azure Key Vault-backed secret scope.
        /// The scope name must be unique within a workspace, must consist of alphanumeric characters, dashes, underscores, and periods, and may not exceed 128 characters.The maximum number of scopes in a workspace is 100.
        /// </summary>
        /// <remarks>
        /// If initial_manage_principal is specified, the initial ACL applied to the scope is applied to the supplied principal (user or group) with MANAGE permissions. The only supported principal for this option is the group users, which contains all users in the workspace. If initial_manage_principal is not specified, the initial ACL with MANAGE permission applied to the scope is assigned to the API request issuer’s user identity.
        /// Throws RESOURCE_ALREADY_EXISTS if a scope with the given name already exists.Throws RESOURCE_LIMIT_EXCEEDED if maximum number of scopes in the workspace is exceeded.Throws INVALID_PARAMETER_VALUE if the scope name is invalid.
        /// </remarks>
        /// <param name="scope">Scope name requested by the user. Scope names are unique. This field is required.</param>
        /// <param name="initialManagePrincipal">The principal that is initially granted MANAGE permission to the created scope.</param>
        /// <param name="akvResourceId">The resource id of the backend Azure Key Vault. E.g. "/subscriptions/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/resourceGroups/azure-rg/providers/Microsoft.KeyVault/vaults/my-azure-kv"</param>
        /// <param name="akvDnsName">The DNS name of the backend Azure Key Vault. E.g. "https://my-azure-kv.vault.azure.net/"</param>
        Task CreateAkvBackedScope(string scope, string initialManagePrincipal, string akvResourceId, string akvDnsName, CancellationToken cancellationToken = default);
        */

        /// <summary>
        /// Deletes a secret scope.
        /// </summary>
        /// <remarks>
        /// Throws RESOURCE_DOES_NOT_EXIST if the scope does not exist. Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">Name of the scope to delete. This field is required.</param>
        Task DeleteScope(string scope, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists all secret scopes available in the workspace.
        /// </summary>
        /// <remarks>
        /// Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        Task<IEnumerable<SecretScope>> ListScopes(CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a secret under the provided scope with the given name. If a secret already exists with the same name, this command overwrites the existing secret’s value. The server encrypts the secret using the secret scope’s encryption settings before storing it. You must have WRITE or MANAGE permission on the secret scope.
        /// The secret key must consist of alphanumeric characters, dashes, underscores, and periods, and cannot exceed 128 characters.The maximum allowed secret value size is 128 KB.The maximum number of secrets in a given scope is 1000.
        /// </summary>
        /// <remarks>
        /// The input fields "string_value" or "bytes_value" specify the type of the secret, which will determine the value returned when the secret value is requested. Exactly one must be specified.
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope exists.Throws RESOURCE_LIMIT_EXCEEDED if maximum number of secrets in scope is exceeded.Throws INVALID_PARAMETER_VALUE if the key name or value length is invalid.Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="secretValue">The value will be stored in UTF-8 (MB4) form.</param>
        /// <param name="scope">The name of the scope to which the secret will be associated with. This field is required.</param>
        /// <param name="key">A unique name to identify the secret. This field is required.</param>
        Task PutSecret(string secretValue, string scope, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a secret under the provided scope with the given name. If a secret already exists with the same name, this command overwrites the existing secret’s value. The server encrypts the secret using the secret scope’s encryption settings before storing it. You must have WRITE or MANAGE permission on the secret scope.
        /// The secret key must consist of alphanumeric characters, dashes, underscores, and periods, and cannot exceed 128 characters.The maximum allowed secret value size is 128 KB.The maximum number of secrets in a given scope is 1000.
        /// </summary>
        /// <remarks>
        /// The input fields "string_value" or "bytes_value" specify the type of the secret, which will determine the value returned when the secret value is requested. Exactly one must be specified.
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope exists.Throws RESOURCE_LIMIT_EXCEEDED if maximum number of secrets in scope is exceeded.Throws INVALID_PARAMETER_VALUE if the key name or value length is invalid.Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="secretValue">The value will be stored as bytes.</param>
        /// <param name="scope">The name of the scope to which the secret will be associated with. This field is required.</param>
        /// <param name="key">A unique name to identify the secret. This field is required.</param>
        Task PutSecret(byte[] secretValue, string scope, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the secret stored in this secret scope. You must have WRITE or MANAGE permission on the secret scope.
        /// </summary>
        /// <remarks>
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope or secret exists. Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">The name of the scope that contains the secret to delete. This field is required.</param>
        /// <param name="key">Name of the secret to delete. This field is required.</param>
        Task DeleteSecret(string scope, string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the secret keys that are stored at this scope. This is a metadata-only operation; secret data cannot be retrieved using this API. Users need the READ permission to make this call.
        /// </summary>
        /// <remarks>
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope exists. Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">The name of the scope whose secrets you want to list. This field is required.</param>
        Task<IEnumerable<SecretMetadata>> ListSecrets(string scope, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates or overwrites the ACL associated with the given principal (user or group) on the specified scope point. In general, a user or group will use the most powerful permission available to them, and permissions are ordered as follows:
        ///     MANAGE - Allowed to change ACLs, and read and write to this secret scope.
        ///     WRITE - Allowed to read and write to this secret scope.
        ///     READ - Allowed to read this secret scope and list what secrets are available.
        /// Note that in general, secret values can only be read from within a command on a cluster (for example, through a notebook). There is no API to read the actual secret value material outside of a cluster.However, the user’s permission will be applied based on who is executing the command, and they must have at least READ permission.
        /// 
        /// Users must have the MANAGE permission to invoke this API.
        /// </summary>
        /// <remarks>
        /// The principal is a user or group name corresponding to an existing Databricks principal to be granted or revoked access.
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope exists.Throws RESOURCE_ALREADY_EXISTS if a permission for the principal already exists.Throws INVALID_PARAMETER_VALUE if the permission is invalid.Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">The name of the scope to apply permissions to. This field is required.</param>
        /// <param name="principal">The principal to which the permission is applied. This field is required.</param>
        /// <param name="permission">The permission level applied to the principal. This field is required.</param>
        Task PutSecretAcl(string scope, string principal, AclPermission permission, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the given ACL on the given scope.
        /// Users must have the MANAGE permission to invoke this API.
        /// </summary>
        /// <remarks>
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope, principal, or ACL exists.Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">The name of the scope to remove permissions from. This field is required.</param>
        /// <param name="principal">The principal to remove an existing ACL from. This field is required.</param>
        Task DeleteSecretAcl(string scope, string principal, CancellationToken cancellationToken = default);

        /// <summary>
        /// Describes the details about the given ACL, such as the group and permission.
        /// Users must have the MANAGE permission to invoke this API.
        /// </summary>
        /// <remarks>
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope exists. Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">The name of the scope to fetch ACL information from. This field is required.</param>
        /// <param name="principal">The principal to fetch ACL information for. This field is required.</param>
        Task<AclItem> GetSecretAcl(string scope, string principal, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists the ACLs set on the given scope.
        /// Users must have the MANAGE permission to invoke this API.
        /// </summary>
        /// <remarks>
        /// Throws RESOURCE_DOES_NOT_EXIST if no such secret scope exists. Throws PERMISSION_DENIED if the user does not have permission to make this API call.
        /// </remarks>
        /// <param name="scope">The name of the scope to fetch ACL information from. This field is required.</param>
        Task<IEnumerable<AclItem>> ListSecretAcl(string scope, CancellationToken cancellationToken = default);
    }
}

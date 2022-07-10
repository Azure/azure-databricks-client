using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client
{
    public interface IGroupsApi : IDisposable
    {
        /// <summary>
        /// Adds a user or group to a group. This call returns an error RESOURCE_DOES_NOT_EXIST if a user or group with the given name does not exist, or if a group with the given parent name does not exist.
        /// </summary>
        /// <param name="parentGroupName">Name of the parent group to which the new member will be added. This field is required.</param>
        /// <param name="principalName">Name of the user or group to be added to the parent group.</param>
        Task AddMember(string parentGroupName, PrincipalName principalName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new group with the given name. This call returns an error RESOURCE_ALREADY_EXISTS if a group with the given name already exists.
        /// </summary>
        /// <param name="groupName">Name for the group; must be unique among groups owned by this organization. This field is required.</param>
        Task<string> Create(string groupName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all of the members of a particular group. This call returns an error RESOURCE_DOES_NOT_EXIST if a group with the given name does not exist.
        /// </summary>
        /// <param name="groupName">The group whose members we want to retrieve. This field is required.</param>
        Task<IEnumerable<PrincipalName>> ListMembers(string groupName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns all of the groups in an organization.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> List(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all groups in which a given user or group is a member (note: this method is non-recursive - it will return all groups in which the given user or group is a member but not the groups in which those groups are members). This call returns an error RESOURCE_DOES_NOT_EXIST if a user or group with the given name does not exist.
        /// </summary>
        /// <param name="principalName">Specify user or group.</param>
        Task<IEnumerable<string>> ListParent(PrincipalName principalName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a user or group from a group. This call returns an error RESOURCE_DOES_NOT_EXIST if a user or group with the given name does not exist, or if a group with the given parent name does not exist.
        /// </summary>
        /// <param name="parentGroupName">Name of the parent group from which the member will be removed. This field is required.</param>
        /// <param name="principalName">Name of the user or group to be removed from the parent group.</param>
        /// <returns></returns>
        Task RemoveMember(string parentGroupName, PrincipalName principalName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a group from this organization. This call returns an error RESOURCE_DOES_NOT_EXIST if a group with the given name does not exist.
        /// </summary>
        /// <param name="groupName">The group to remove. This field is required.</param>
        Task Delete(string groupName, CancellationToken cancellationToken = default);
    }
}

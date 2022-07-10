using System;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestGroupsApi(DatabricksClient client)
    {
        Console.WriteLine("Listing groups");
        var groupsList = await client.Groups.List();
        foreach (var group in groupsList)
        {
            Console.WriteLine("Group name: {0}", group);
        }

        const string newGroupName = "sample group";

        Console.WriteLine("Creating new group \"{0}\"", newGroupName);
        await client.Groups.Create(newGroupName);

        Console.WriteLine($"Adding members in {newGroupName} group");
        await client.Groups.AddMember(newGroupName, new PrincipalName { UserName = DatabricksUserName });

        Console.WriteLine($"Listing members in {newGroupName} group");
        var members = await client.Groups.ListMembers(newGroupName);
        foreach (var member in members)
        {
            if (!string.IsNullOrEmpty(member.UserName))
            {
                Console.WriteLine("Member (User): {0}", member.UserName);
            }
            else
            {
                Console.WriteLine("Member (Group): {0}", member.GroupName);
            }
        }

        Console.WriteLine($"Removing members in {newGroupName} group");
        await client.Groups.RemoveMember(newGroupName, new PrincipalName { UserName = DatabricksUserName });


        Console.WriteLine("Deleting group \"{0}\"", newGroupName);
        await client.Groups.Delete(newGroupName);
    }
}
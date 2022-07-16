using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestSecretsApi(DatabricksClient client)
    {
        Console.WriteLine("Listing secrets scope");
        var scopes = await client.Secrets.ListScopes();
        foreach (var scope in scopes)
        {
            Console.WriteLine("Secret scope: {0}, backend type: {1}", scope.Name, scope.BackendType);
        }

        const string scopeName = "SampleScope";
        Console.WriteLine("Creating secrets scope");
        await client.Secrets.CreateDatabricksBackedScope(scopeName, null);

        Console.WriteLine("Creating text secret");
        await client.Secrets.PutSecret("textvalue", scopeName, "secretkey.text");

        Console.WriteLine("Creating binary secret");
        await client.Secrets.PutSecret(new byte[] { 0x01, 0x02, 0x03, 0x04 }, scopeName, "secretkey.bin");

        Console.WriteLine("Listing secrets");
        var secrets = await client.Secrets.ListSecrets(scopeName);
        foreach (var secret in secrets)
        {
            Console.WriteLine("Secret key {0}, last updated: {1:s}", secret.Key, secret.LastUpdatedTimestamp);
        }

        Console.WriteLine("Deleting secrets");
        await client.Secrets.DeleteSecret(scopeName, "secretkey.text");
        await client.Secrets.DeleteSecret(scopeName, "secretkey.bin");

        Console.WriteLine("Deleting secrets scope");
        await client.Secrets.DeleteScope(scopeName);
    }
}
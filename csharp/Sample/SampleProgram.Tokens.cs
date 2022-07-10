using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestTokenApi(DatabricksClient client)
    {
        Console.WriteLine("Creating token without expiry");
        var (tokenValue, tokenInfo) = await client.Token.Create(null, "SampleProgram token");
        Console.WriteLine("Token value: {0}", tokenValue);
        Console.WriteLine("Token Id {0}", tokenInfo.TokenId);
        Console.WriteLine("Token comment {0}", tokenInfo.Comment);
        Console.WriteLine("Token creation time {0:s}", tokenInfo.CreationTime);
        Console.WriteLine("Token expiry time {0:s}", tokenInfo.ExpiryTime);
        Console.WriteLine("Deleting token");
        await client.Token.Revoke(tokenInfo.TokenId);

        Console.WriteLine("Creating token with expiry");
        (tokenValue, tokenInfo) = await client.Token.Create(3600, "SampleProgram token");
        Console.WriteLine("Token value: {0}", tokenValue);
        Console.WriteLine("Token comment {0}", tokenInfo.Comment);
        Console.WriteLine("Token creation time {0:s}", tokenInfo.CreationTime);
        Console.WriteLine("Token expiry time {0:s}", tokenInfo.ExpiryTime);
        Console.WriteLine("Deleting token");
        await client.Token.Revoke(tokenInfo.TokenId);

        Console.WriteLine("Listing tokens");
        var tokens = await client.Token.List();
        foreach (var token in tokens)
        {
            Console.WriteLine("Token Id {0}\tComment {1}", token.TokenId, token.Comment);
        }
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client.Models;
namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestReposApi(DatabricksClient client)
    {
        static string getRepoString(Repo repo) => $"Repo: {repo.Id}, path: {repo.Path}, URL: {repo.Url}, provider: {repo.Provider}, branch: {repo.Branch}, head: {repo.HeadCommitId}";

        Console.WriteLine("Creating repo");
        var repo = await client.Repos.Create("https://github.com/mlflow/mlflow-example", RepoProvider.gitHub, $"/Repos/{DatabricksUserName}/mlflow-example");
        Console.WriteLine(getRepoString(repo));

        Console.WriteLine("Update repo");
        await client.Repos.Update(repo.Id, tag: "v1.0.0");
        repo = await client.Repos.Get(repo.Id);
        Console.WriteLine(getRepoString(repo));

        await client.Repos.Update(repo.Id, branch: "master");
        repo = await client.Repos.Get(repo.Id);
        Console.WriteLine(getRepoString(repo));

        Console.WriteLine("Delete repo");
        await client.Repos.Delete(repo.Id);

        Console.WriteLine("Listing repos");
        var (repos, _) = await client.Repos.List();

        foreach (var item in repos)
        {
            Console.WriteLine(getRepoString(item));
        }
    }
}
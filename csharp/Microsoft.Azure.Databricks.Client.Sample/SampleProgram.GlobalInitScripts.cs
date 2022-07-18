// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestGlobalInitScriptsApi(DatabricksClient client)
    {
        Console.WriteLine("Listing global init scripts...");
        foreach (var script in await client.GlobalInitScriptsApi.List())
        {
            Console.WriteLine(
                $"\tGlobal Init Script: {script.ScriptId}, Name: {script.Name}, Created at: {script.CreatedAt}, Created by: {script.CreatedBy}");
        }

        Console.WriteLine("Creating global init script");
        var scriptId = await client.GlobalInitScriptsApi.Create("Test script", "echo hello");

        Console.WriteLine("Listing global init scripts...");

        foreach (var script in await client.GlobalInitScriptsApi.List())
        {
            Console.WriteLine(
                $"\tGlobal Init Script: {script.ScriptId}, Name: {script.Name}, Created at: {script.CreatedAt}, Created by: {script.CreatedBy}");
        }

        Console.WriteLine("Getting global init script");
        var scriptObj = await client.GlobalInitScriptsApi.Get(scriptId);
        Console.WriteLine($"\tScript Name: {scriptObj.Name}");
        Console.WriteLine($"\tScript Content: {scriptObj.Script}");

        Console.WriteLine($"Editing global init script: {scriptId}");
        await client.GlobalInitScriptsApi.Update(scriptId, "Test script 2");

        Console.WriteLine($"Deleting global init script {scriptId}");
        await client.GlobalInitScriptsApi.Delete(scriptId);
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestClusterPoliciesApi(DatabricksClient client)
    {
        Console.WriteLine("Creating cluster policy");
        const string policyDef = "{\"spark_conf.spark.databricks.cluster.profile\":{\"type\":\"forbidden\",\"hidden\":true}}";
        var policyId = await client.ClusterPolicies.Create("Test policy", policyDef);

        Console.WriteLine("Listing cluster policies...");
        var policies = await client.ClusterPolicies.List();
        foreach (var policy in policies)
        {
            Console.WriteLine(
                $"\tPolicy {policy.Name}, Created at {policy.CreatedAtTimestamp}");
        }

        Console.WriteLine("Getting cluster policy");
        var retrievedPolicy = await client.ClusterPolicies.Get(policyId);
        Console.WriteLine($"\tPolicy {retrievedPolicy.Name}");

        Console.WriteLine($"Editing policy {policyId}");
        await client.ClusterPolicies.Edit(policyId, "Test policy 2", policyDef);

        Console.WriteLine($"Deleting policy {policyId}");
        await client.ClusterPolicies.Delete(policyId);
    }
}
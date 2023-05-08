// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestClusterPoliciesApi(DatabricksClient client)
    {
        Console.WriteLine("Listing policy families");
        var policyFamilies = await client.ClusterPolicies.ListPolicyFamily(2);
        foreach (var policyFamily in policyFamilies.Item1)
        {
            await Console.Out.WriteLineAsync($"\t{policyFamily.Name}");
        }

        policyFamilies = await client.ClusterPolicies.ListPolicyFamily(2, policyFamilies.Item2);
        foreach (var policyFamily in policyFamilies.Item1)
        {
            await Console.Out.WriteLineAsync($"\t{policyFamily.Name}");
        }

        Console.WriteLine("Get policy family by id \"personal-vm\"");
        var family = await client.ClusterPolicies.GetPolicyFamily("personal-vm");
        await Console.Out.WriteLineAsync($"{family.Definition}");

        Console.WriteLine("Creating cluster policy");
        const string policyDef = "{\"spark_conf.spark.databricks.cluster.profile\":{\"type\":\"forbidden\",\"hidden\":true}}";
        var policyId1 = await client.ClusterPolicies.Create("Test policy 1", policyDef);

        Console.WriteLine("Creating cluster policy with policy family");
        const string policyFamilyOverride = "{\"spark_conf.spark.databricks.cluster.profile\":{\"type\":\"forbidden\",\"hidden\":true}}";
        var policyId2 = await client.ClusterPolicies.CreateWithPoiclyFamily("Test policy 2", "personal-vm", policyFamilyOverride, 5);

        Console.WriteLine("Listing cluster policies...");
        var policies = await client.ClusterPolicies.List();
        foreach (var policy in policies)
        {
            await Console.Out.WriteLineAsync(
                $"\tPolicy {policy.Name}, Created at {policy.CreatedAtTimestamp}");
        }

        Console.WriteLine("Getting cluster policy");
        var retrievedPolicy1 = await client.ClusterPolicies.Get(policyId1);
        Console.WriteLine($"\tPolicy {retrievedPolicy1.Name}");

        var retrievedPolicy2 = await client.ClusterPolicies.Get(policyId2);
        Console.WriteLine($"\tPolicy {retrievedPolicy2.Name}");

        Console.WriteLine($"Editing policy {policyId1}");
        await client.ClusterPolicies.Edit(policyId1, "Test policy 1", policyDef);

        Console.WriteLine($"Editing policy {policyId2}");
        await client.ClusterPolicies.EditWithPoiclyFamily(policyId2, "Test policy 2", "personal-vm");

        Console.WriteLine($"Deleting policy {policyId1}");
        await client.ClusterPolicies.Delete(policyId1);

        Console.WriteLine($"Deleting policy {policyId2}");
        await client.ClusterPolicies.Delete(policyId2);
    }
}
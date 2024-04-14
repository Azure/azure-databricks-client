// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Azure.Databricks.Client.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestStatementExecutionApi(DatabricksClient client)
    {
        Console.WriteLine($"Creating new warehouse");

        var w = new WarehouseAttributes
        {
            Name = "Testing Warehouse",
            WarehouseType = WarehouseType.PRO,
            EnableServerlessCompute = true,
            Channel = new Channel { Name = ChannelName.CHANNEL_NAME_UNSPECIFIED },
            SpotInstancePolicy = SpotInstancePolicy.POLICY_UNSPECIFIED,
            ClusterSize = "2X-Small",
            AutoStopMins = 20,
            MaxNumClusters = 2,
        };

        var id = await client.SQL.Warehouse.Create(w);

        Console.WriteLine($"Starting warehouse id {id}");
        await client.SQL.Warehouse.Start(id);
        Thread.Sleep(10 * 1000);

        Console.WriteLine($"Querying warehouse id {id}");
        var s = SqlStatement.Create("select * from main.information_schema.catalogs", id);

        var result = await client.SQL.StatementExecution.Execute(s);
        Console.WriteLine(result.Status.State);
        Console.WriteLine($"Row count: {result.Manifest.TotalRowCount}");

        Console.WriteLine($"Stopping warehouse id {id}");
        await client.SQL.Warehouse.Stop(id);

        Console.WriteLine($"Deleting warehouse id {id}");
        await client.SQL.Warehouse.Delete(id);
    }
}

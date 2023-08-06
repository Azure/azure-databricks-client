// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using System;
using Microsoft.Azure.Databricks.Client.Models;
using System.Threading;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestWarehouseApi(DatabricksClient client)
    {
        Console.WriteLine($"Listing warehouse");
        var warehouses = await client.SQL.Warehouse.List();

        foreach (var item in warehouses)
        {
            Console.WriteLine(item.Name);
        }

        Console.WriteLine($"Creating new warehouse");

        var w = new WarehouseAttributes
        {
            Name = "Testing Warehouse",
            Channel = new Channel { Name = ChannelName.CHANNEL_NAME_UNSPECIFIED },
            SpotInstancePolicy = SpotInstancePolicy.POLICY_UNSPECIFIED,
            ClusterSize = "2X-Small",
            AutoStopMins = 20,
            MaxNumClusters = 2,
        };

        var id = await client.SQL.Warehouse.Create(w);

        Console.WriteLine($"Starting warehouse id {id}");
        await client.SQL.Warehouse.Start(id);

        Console.WriteLine($"Querying warehouse id {id}");
        var result = await client.SQL.Warehouse.Get(id);
        Console.WriteLine(result.State);
        Thread.Sleep(30 * 1000);

        Console.WriteLine($"Stopping warehouse id {id}");
        await client.SQL.Warehouse.Stop(id);

        Console.WriteLine($"Deleting warehouse id {id}");
        await client.SQL.Warehouse.Delete(id);
    }
}

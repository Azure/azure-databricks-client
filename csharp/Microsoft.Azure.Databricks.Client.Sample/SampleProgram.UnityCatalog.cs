using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Sample;

internal static partial class SampleProgram
{
    private static async Task TestUnityCatalogApi(DatabricksClient client)
    {
        var catalogAttributes = new CatalogAttributes()
        {
            Name = "9E2B6ACC-2624-4D22-8616-D9808A53EEF3",
            Comment = "Catalog created during C# client sample run"
        };

        PrintDelimiter();

        Console.WriteLine("Listing metastores");
        var metastoresList = await client.UnityCatalog.Metastores.List();
        foreach (var metastore in metastoresList)
        {
            Console.WriteLine($"\t{metastore.MetastoreId}, {metastore.Name}");
        }
        PrintDelimiter();

        Console.WriteLine("Creating a catalog...");
        var catalog = await client.UnityCatalog.Catalogs.Create(catalogAttributes);
        Console.WriteLine($"Created a catalog of name: {catalog.Name}");
        PrintDelimiter();

        Console.WriteLine("Listing catalogs...");
        var catalogsList = await client.UnityCatalog.Catalogs.List();
        foreach (var catalogItem in catalogsList)
        {
            Console.WriteLine($"\t{catalogItem.Name}, {catalogItem.CatalogType}");
        }
        PrintDelimiter();

        Console.WriteLine("Listing schemas in created catalog...");
        var schemasList = (await client.UnityCatalog.Schemas.List(catalog.FullName)).ToArray();
        foreach (var schema in schemasList)
        {
            Console.WriteLine($"\t{schema.Name}");
        }
        PrintDelimiter();

        Console.WriteLine("Listing system schemas...");
        var systemSchemasList = (await client.UnityCatalog.SystemSchemas.List(catalog.MetastoreId)).ToArray();
        foreach (var schema in systemSchemasList)
        {
            Console.WriteLine($"\t{schema.Schema}, {schema.State}");
        }
        PrintDelimiter();

        Console.WriteLine("Deleting schemas...");
        var schemasToDelete = schemasList
            .Select(x => x.Name)
            .Except(systemSchemasList.Select(x => x.Schema))
            .ToList();

        foreach (var schema in schemasToDelete)
        {
            var fullSchemaName = $"{catalog.Name}.{schema}";
            Console.WriteLine($"Deleting schema {fullSchemaName}...");
            await client.UnityCatalog.Schemas.Delete(fullSchemaName);
        }

        Console.WriteLine("Schemas deleted");
        PrintDelimiter();

        Console.WriteLine($"Deleting created catalog {catalog.Name}...");
        await client.UnityCatalog.Catalogs.Delete(catalog.Name);
        Console.WriteLine("Catalog deleted");
    }
}
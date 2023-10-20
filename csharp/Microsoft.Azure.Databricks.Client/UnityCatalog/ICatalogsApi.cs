using Microsoft.Azure.Databricks.Client.Models.UnityCatalog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.UnityCatalog;

public interface ICatalogsApi : IDisposable
{
    Task<CatalogsList> List(CancellationToken cancellationToken = default);

    Task<Catalog> Create(Catalog catalog, CancellationToken cancellationToken = default);

    Task<Catalog> Get(string catalogName,  CancellationToken cancellationToken = default);

    Task<Catalog> Update(
        string catalogName,
        string name = default,
        string owner = default,
        string comment = default,
        Dictionary<string, string> properties = default,
        IsolationMode? isolationMode = IsolationMode.OPEN,
        CancellationToken cancellationToken = default);

    Task Delete(string catalogName, bool forceDeletion = false, CancellationToken cancellationToken = default);
}

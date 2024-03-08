using Microsoft.Azure.Databricks.Client.UnityCatalog;
using Microsoft.Azure.Databricks.Client.UnityCatalog.Interfaces;
using System;
using System.Net.Http;

namespace Microsoft.Azure.Databricks.Client;

public class UnityCatalogClient : ApiClient, IDisposable
{
    public UnityCatalogClient(HttpClient httpClient) : base(httpClient)
    {
        this.Catalogs = new CatalogsApiClient(httpClient);
        this.Connections = new ConnectionsApiClient(httpClient);
        this.ExternalLocations = new ExternalLocationsApiClient(httpClient);
        this.Functions = new FunctionsApiClient(httpClient);
        this.Metastores = new MetastoresApiClient(httpClient);
        this.Schemas = new SchemasApiClient(httpClient);
        this.SecurableWorkspaceBindings = new SecurableWorkspaceBindingsApiClient(httpClient);
        this.StorageCredentials = new StorageCredentialsApiClient(httpClient);
        this.SystemSchemas = new SystemSchemasApiClient(httpClient);
        this.TableConstraints = new TableConstraintsApiClient(httpClient);
        this.Tables = new TablesApiClient(httpClient);
        this.UnityCatalogPermissions = new UnityCatalogPermissionsApiClient(httpClient);
        this.Volumes = new VolumesApiClient(httpClient);
        this.Lineage = new LineageApiClient(httpClient);
    }

    public ICatalogsApi Catalogs { get; set; }

    public IConnectionsApi Connections { get; set; }

    public IExternalLocationsApi ExternalLocations { get; set; }

    public IFunctionsApi Functions { get; set; }

    public IMetastoresApi Metastores { get; set; }

    public ISchemasApi Schemas { get; set; }

    public ISecurableWorkspaceBindingsApi SecurableWorkspaceBindings { get; set; }

    public IStorageCredentialsApi StorageCredentials { get; set; }

    public ISystemSchemas SystemSchemas { get; set; }

    public ITableConstraintsApi TableConstraints { get; set; }

    public ITablesApi Tables { get; set; }

    public IUnityCatalogPermissionsApi UnityCatalogPermissions { get; set; }

    public IVolumesApi Volumes { get; set; }

    public ILineageApi Lineage { get; set; }
}

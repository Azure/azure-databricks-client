using Microsoft.Azure.Databricks.Client.MachineLearning;
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
        this.Shares = new SharesApiClient(httpClient);
        this.StorageCredentials = new StorageCredentialsApiClient(httpClient);
        this.SystemSchemas = new SystemSchemasApiClient(httpClient);
        this.TableConstraints = new TableConstraintsApiClient(httpClient);
        this.Tables = new TablesApiClient(httpClient);
        this.UnityCatalogPermissions = new UnityCatalogPermissionsApiClient(httpClient);
        this.Volumes = new VolumesApiClient(httpClient);
        this.Lineage = new LineageApiClient(httpClient);
        this.ModelVersion = new ModelVersionApiClient(httpClient);
        this.RegisteredModels = new RegisteredModelsApiClient(httpClient);
    }

    public virtual ICatalogsApi Catalogs { get; set; }

    public virtual IConnectionsApi Connections { get; set; }

    public virtual IExternalLocationsApi ExternalLocations { get; set; }

    public virtual IFunctionsApi Functions { get; set; }

    public virtual IMetastoresApi Metastores { get; set; }

    public virtual ISchemasApi Schemas { get; set; }

    public virtual ISecurableWorkspaceBindingsApi SecurableWorkspaceBindings { get; set; }

    public virtual ISharesApi Shares { get; set; }

    public virtual IStorageCredentialsApi StorageCredentials { get; set; }

    public virtual ISystemSchemas SystemSchemas { get; set; }

    public virtual ITableConstraintsApi TableConstraints { get; set; }

    public virtual ITablesApi Tables { get; set; }

    public virtual IUnityCatalogPermissionsApi UnityCatalogPermissions { get; set; }

    public virtual IVolumesApi Volumes { get; set; }

    public virtual ILineageApi Lineage { get; set; }

    public virtual IModelVersionApi ModelVersion { get; set; }

    public virtual IRegisteredModelsApi RegisteredModels { get; set; }
}

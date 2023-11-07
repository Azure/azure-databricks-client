using Castle.DynamicProxy.Contributors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.Test.UnityCatalog;

[TestClass]
public class UnityCatalogApiClientTest : ApiClientTest
{
    protected static new readonly Uri BaseApiUri = new("https://test-server/api/2.1/unity-catalog/");

}

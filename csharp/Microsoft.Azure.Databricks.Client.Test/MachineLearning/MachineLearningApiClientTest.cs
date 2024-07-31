namespace Microsoft.Azure.Databricks.Client.Test.MachineLearning;

[TestClass]
public class MachineLearningApiClientTest : ApiClientTest
{
    protected static readonly Uri MlflowBaseUri = new(BaseApiUri, "2.0/mlflow");
}

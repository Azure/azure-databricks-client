namespace Microsoft.Azure.Databricks.Cli
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var app = new DatabricksCommandLineBuilder().Build();
            var exitCode = app.Execute(args);
            return exitCode;
        }
    }
}

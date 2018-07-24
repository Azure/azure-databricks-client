using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var app = new DatabricksCommandLineBuilder().Build();
            app.Execute(args);
            return 0;
        }
    }
}

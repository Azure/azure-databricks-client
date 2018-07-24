using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public interface ICommandLineApplicationBuilder
    {
        CommandLineApplication Build();
    }
}
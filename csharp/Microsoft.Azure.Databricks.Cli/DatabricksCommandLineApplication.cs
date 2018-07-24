using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public class DatabricksCommandLineBuilder : ICommandLineApplicationBuilder
    {
        public CommandLineApplication Build()
        {
            var app = new CommandLineApplication(false)
            {
                Name = "databricks"
            };

            app.HelpOption("-?|-h|--help");
            app.OnExecute(async () =>
            {
                app.ShowHelp();
                return await Task.FromResult(0);
            });

            app.Option("-u|--cluster-base-url", "Cluster base URL (e.g. https://southcentralus.azuredatabricks.net)", CommandOptionType.SingleValue, true);
            app.Option("-t|--access-token", "Cluster access token", CommandOptionType.SingleValue, true);

            ICommandLineApplicationBuilder jobsCommandBuilder = new JobsCommandBuilder(app);
            app.Commands.Add(jobsCommandBuilder.Build());
            
            return app;
        }
    }
}

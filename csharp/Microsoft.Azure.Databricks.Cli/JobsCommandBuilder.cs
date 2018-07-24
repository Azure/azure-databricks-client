using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public class JobsCommandBuilder : ICommandLineApplicationBuilder
    {
        private readonly CommandLineApplication _parent;
        
        public JobsCommandBuilder(CommandLineApplication parent)
        {
            _parent = parent;
        }

        public CommandLineApplication Build()
        {
            var command = new CommandLineApplication(false)
            {
                Name = "jobs",
                Description = "Utility to interact with jobs.",
                Parent = this._parent
            };

            command.HelpOption("-?|-h|--help");

            command.OnExecute(async () =>
            {
                command.ShowHelp();
                return await Task.FromResult(0);
            });

            ICommandLineApplicationBuilder jobCreateCommandBuilder = new JobCreateCommandBuilder(command);

            command.Commands.Add(jobCreateCommandBuilder.Build());

            return command;
        }
    }
}
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public class DbfsCommandBuilder : ICommandLineApplicationBuilder
    {
        private readonly CommandLineApplication _parent;
        
        public DbfsCommandBuilder(CommandLineApplication parent)
        {
            _parent = parent;
        }

        public CommandLineApplication Build()
        {
            var command = new CommandLineApplication(false)
            {
                Name = "dbfs",
                Description = "Utility to interact with dbfs.",
                Parent = this._parent
            };

            command.HelpOption("-?|-h|--help");

            command.OnExecute(async () =>
            {
                command.ShowHelp();
                return await Task.FromResult(0);
            });

            ICommandLineApplicationBuilder dbfsUploadCommandBuilder = new DbfsUploadCommandBuilder(command);

            command.Commands.Add(dbfsUploadCommandBuilder.Build());

            return command;
        }
    }
}
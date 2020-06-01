using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public class DbfsUploadCommandBuilder : ICommandLineApplicationBuilder
    {
        private readonly CommandLineApplication _parent;

        public DbfsUploadCommandBuilder(CommandLineApplication parent)
        {
            _parent = parent;
        }

        public CommandLineApplication Build()
        {
            var cmdUpload = new CommandLineApplication(false)
            {
                Name = "upload",
                Description = "Upload a file.",
                Parent = this._parent
            };

            cmdUpload.HelpOption("-?|-h|--help");

            var localFilePathOption = cmdUpload.Option("-src|--source", "Source file path", CommandOptionType.SingleValue);
            var dbfsFilePathOption = cmdUpload.Option("-dst|--destination", "Destination DBFS file path", CommandOptionType.SingleValue);
            var overwriteOption = cmdUpload.Option("-o|--overwrite", "Overwrite existing file", CommandOptionType.NoValue);

            cmdUpload.OnExecute(async () =>
            {
                var service = new DatabricksApiService(cmdUpload);
                var dbfsPath = dbfsFilePathOption.Value();
                var localFilePath = localFilePathOption.Value();
                var overwrite = overwriteOption.HasValue();
                await service.UploadFile(localFilePath, dbfsPath, overwrite);
                return await Task.FromResult(0);
            });
            
            return cmdUpload;
        }
    }
}
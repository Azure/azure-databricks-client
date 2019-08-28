using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Databricks.Client;
using Microsoft.Extensions.CommandLineUtils;

namespace Microsoft.Azure.Databricks.Cli
{
    public class JobCreateCommandBuilder : ICommandLineApplicationBuilder
    {
        private readonly CommandLineApplication _parent;
        
        public JobCreateCommandBuilder(CommandLineApplication parent)
        {
            _parent = parent;
        }

        public CommandLineApplication Build()
        {
            var cmdJobCreate = new CommandLineApplication(false)
            {
                Name = "create",
                Description = "Creates a job.",
                Parent = this._parent
            };

            cmdJobCreate.HelpOption("-?|-h|--help");

            var clusterIdOption = cmdJobCreate.Option("-ecid|--existing-cluster-id", "Existing cluster id", CommandOptionType.SingleValue);
            var autoScaleOption = cmdJobCreate.Option("-as|--auto-scale", "New cluster auto scale (min-max)", CommandOptionType.SingleValue);
            var numOfWorkersOption = cmdJobCreate.Option("-nw|--num-workers", "New cluster number of workers", CommandOptionType.SingleValue);
            var instancePoolOption = cmdJobCreate.Option("-ipid|--instance-pool-id", "Instance pool Id", CommandOptionType.SingleValue);
            var python3Option = cmdJobCreate.Option("-p3|--python3", "Enables Python3", CommandOptionType.NoValue);
            var nodeTypeOption = cmdJobCreate.Option("-nt|--node-type", "Node type for header and workers. Default value: Standard_D3_v2", CommandOptionType.SingleValue);
            var runtimeVersionOption = cmdJobCreate.Option("-rv|--runtime-version", "Runtime version. Default value: 4.2.x-scala2.11", CommandOptionType.SingleValue);
            var clusterLogOption = cmdJobCreate.Option("-cld|--cluster-log-delivery", "Specify cluster log delivery path.", CommandOptionType.SingleValue);
            var tableAccessControlOption = cmdJobCreate.Option("-tac|--table-access-control", "Enable table access control", CommandOptionType.NoValue);

            var jobNameOption = cmdJobCreate.Option("-n|--job-name", "Job name", CommandOptionType.SingleValue);

            var jarMainClassOption = cmdJobCreate.Option("-jmc|--jar-main-class", "Main class full name", CommandOptionType.SingleValue);
            var jarPathOption = cmdJobCreate.Option("-jpath|--jar-path", "Path to jars (either use semicolon delimited list, or specify multiple option)", CommandOptionType.MultipleValue);

            var notebookPathOption = cmdJobCreate.Option("-npath|--notebook-path", "Path to notebook", CommandOptionType.SingleValue);

            var waitOption = cmdJobCreate.Option("-w|--wait", "Wait for job to complete", CommandOptionType.NoValue);
            var deleteOption = cmdJobCreate.Option("-d|--delete", "Delete successful job", CommandOptionType.NoValue);

            var sparkConfOption = cmdJobCreate.Option("-conf|--conf", "Spark config", CommandOptionType.MultipleValue);

            cmdJobCreate.OnExecute(async () =>
            {
                var jobSettings = GetJobSettings(jarMainClassOption, jobNameOption, cmdJobCreate,
                    jarPathOption, notebookPathOption);

                if (jobSettings == null)
                {
                    ConsoleLogger.WriteLineError("Must specify one of --jar-main-class and --notebook-path.");
                    return await Task.FromResult(-1);
                }

                if (clusterIdOption.HasValue())
                {
                    jobSettings.ExistingClusterId = clusterIdOption.Value();
                }
                else
                {
                    var newCluster = GetNewClusterConfiguration(autoScaleOption, numOfWorkersOption,
                        python3Option, nodeTypeOption, runtimeVersionOption, tableAccessControlOption,
                        instancePoolOption, clusterLogOption, sparkConfOption);

                    if (newCluster == null)
                    {
                        ConsoleLogger.WriteLineError("Must specify one of --auto-scale and --num-workers when --existing-cluster-id is not specified.");
                        return await Task.FromResult(-1);
                    }

                    jobSettings.NewCluster = newCluster;
                }

                var service = new DatabricksApiService(cmdJobCreate);

                ConsoleLogger.WriteLineInfo($"Submitting job {jobSettings.Name}");
                var jobId = await service.CreateJob(jobSettings);
                ConsoleLogger.WriteLineInfo($"Job submitted with Id {jobId}");

                if (!waitOption.HasValue())
                {
                    return await Task.FromResult(0);
                }

                var runState = await service.RunNow(jobId);
                ConsoleLogger.WriteLineInfo($"Run finished with life cycle state {runState.LifeCycleState}");

                if (runState.LifeCycleState == RunLifeCycleState.INTERNAL_ERROR ||
                    runState.LifeCycleState == RunLifeCycleState.SKIPPED)
                {
                    ConsoleLogger.WriteLineError($"State message: {runState.StateMessage}");
                    return await Task.FromResult(-1);
                }

                if (runState.ResultState == null || runState.ResultState != RunResultState.SUCCESS)
                {
                    ConsoleLogger.WriteLineError($"State message: {runState.StateMessage}");
                    ConsoleLogger.WriteLineError($"Result of job run does not indicate success.");
                    return await Task.FromResult(-1);
                }

                ConsoleLogger.WriteLineInfo("Job run succeeded.");

                if (deleteOption.HasValue())
                {
                    await service.DeleteJob(jobId);
                }

                return await Task.FromResult(0);
            });

            return cmdJobCreate;
        }

        private static JobSettings GetJobSettings(CommandOption jarMainClassOption, CommandOption jobNameOption,
            CommandLineApplication cmdJobCreate, CommandOption jarPathOption, CommandOption notebookPathOption)
        {
            var jarPaths = jarPathOption.Values.SelectMany(path => path.Split(';'));

            JobSettings jobSettings;
            if (jarMainClassOption.HasValue())
            {
                jobSettings = JobSettings.GetNewSparkJarJobSettings(
                    jobNameOption.Value(),
                    jarMainClassOption.Value(),
                    cmdJobCreate.RemainingArguments,
                    jarPaths
                );
            }

            else if (notebookPathOption.HasValue())
            {
                var notebookParams = cmdJobCreate.RemainingArguments.ToDictionary(
                    v => v.Split('=')[0],
                    v => v.Split('=')[1]
                );

                jobSettings = JobSettings.GetNewNotebookJobSettings(
                    jobNameOption.Value(),
                    notebookPathOption.Value(),
                    notebookParams
                );
            }
            else
            {
                return null;
            }

            return jobSettings;
        }

        private static ClusterInfo GetNewClusterConfiguration(CommandOption autoScaleOption, CommandOption numOfWorkersOption,
            CommandOption python3Option, CommandOption nodeTypeOption, CommandOption runtimeVersionOption,
            CommandOption tableAccessControlOption, CommandOption instancePoolOption, CommandOption clusterLogOption, CommandOption sparkConfigOption)
        {
            var newCluster = ClusterInfo.GetNewClusterConfiguration();

            if (clusterLogOption.HasValue())
            {
                newCluster.WithClusterLogConf(clusterLogOption.Value());
            }

            if (autoScaleOption.HasValue())
            {
                var autoScale = autoScaleOption.Value();
                var split = autoScale.Split('-', '~', ',').Select(int.Parse).ToArray();
                var (min, max) = Tuple.Create(split[0], split[1]);

                newCluster.WithAutoScale(min, max);
            }
            else if (numOfWorkersOption.HasValue())
            {
                var numOfWorkers = int.Parse(numOfWorkersOption.Value());
                newCluster.WithNumberOfWorkers(numOfWorkers);
            }
            else
            {
                return null;
            }

            if (instancePoolOption.HasValue())
            {
                newCluster.InstancePoolId = instancePoolOption.Value();
            }
            else
            {
                var nodeType = nodeTypeOption.HasValue()
                    ? nodeTypeOption.Value()
                    : NodeTypes.Standard_D3_v2;
                newCluster.WithNodeType(nodeType);
            }

            newCluster.WithPython3(python3Option.HasValue());

            var runtimeVersion = runtimeVersionOption.HasValue()
                ? runtimeVersionOption.Value()
                : RuntimeVersions.Runtime_5_5;

            newCluster.WithRuntimeVersion(runtimeVersion);
            newCluster.WithTableAccessControl(tableAccessControlOption.HasValue());

            var sparkConf = from conf in sparkConfigOption.Values
                            let kvp = conf.Split('=')
                            where kvp.Length > 1
                            let key = kvp[0]
                            let value = kvp[1]
                            select (key, value);

            newCluster.SparkConfiguration = sparkConf.ToDictionary(tuple => tuple.key, tuple => tuple.value);
            return newCluster;
        }
    }
}
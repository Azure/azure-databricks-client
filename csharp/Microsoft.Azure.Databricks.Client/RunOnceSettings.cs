using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunOnceSettings : RunSettings<RunOnceSettings>
    {
        public static RunOnceSettings GetOneTimeSparkJarRunSettings(string runName, string mainClass,
            IEnumerable<string> parameters, IEnumerable<string> jarLibs, string idempotencyToken = null)
        {
            var runOnceSettings = new RunOnceSettings
            {
                RunName = runName,
                SparkJarTask = new SparkJarTask
                {
                    MainClassName = mainClass,
                    Parameters = parameters.ToList()
                },
                Libraries = jarLibs.Select(jarLib => new JarLibrary(jarLib)).Cast<Library>().ToList(),
                SparkPythonTask = null,
                SparkSubmitTask = null,
                NotebookTask = null,
                IdempotencyToken = idempotencyToken
            };

            return runOnceSettings;
        }

        public static RunOnceSettings GetOneTimeNotebookRunSettings(string runName, string notebookPath,
            Dictionary<string, string> parameters, string idempotencyToken = null)
        {
            var runOnceSettings = new RunOnceSettings
            {
                RunName = runName,
                NotebookTask = new NotebookTask
                {
                    NotebookPath = notebookPath,
                    BaseParameters = parameters,
                },
                IdempotencyToken = idempotencyToken,
                SparkPythonTask = null,
                SparkSubmitTask = null,
                SparkJarTask = null
            };

            return runOnceSettings;
        }

        /// <summary>
        /// An optional name for the run. The default value is Untitled.
        /// </summary>
        [JsonProperty(PropertyName = "run_name")]
        public string RunName { get; set; }

        /// <summary>
        /// An optional token for the run to ensure the same workload is not run mutliple times.
        /// </summary>
        [JsonProperty(PropertyName = "idempotency_token")]
        public string IdempotencyToken { get; set; }
    }
}
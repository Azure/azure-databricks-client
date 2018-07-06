using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunOnceSettings : RunSettings<RunOnceSettings>
    {
        public static RunOnceSettings GetOneTimeSparkJarRunSettings(string runName, string mainClass,
            IEnumerable<string> parameters, IEnumerable<string> jarLibs)
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
                NotebookTask = null
            };

            return runOnceSettings;
        }

        /// <summary>
        /// An optional name for the run. The default value is Untitled.
        /// </summary>
        [JsonProperty(PropertyName = "run_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string RunName { get; set; }
    }
}
using Microsoft.Azure.Databricks.Client.MachineLearning;
using System;
using System.Net.Http;

namespace Microsoft.Azure.Databricks.Client;

public class MachineLearningClient : ApiClient, IDisposable
{
    public MachineLearningClient(HttpClient httpClient) : base(httpClient)
    {
        this.Experiments = new ExperimentApiClient(httpClient);
    }

    public virtual IExperimentApi Experiments { get; set; }
}

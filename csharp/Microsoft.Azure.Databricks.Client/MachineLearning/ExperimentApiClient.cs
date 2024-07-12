using Azure;
using Microsoft.Azure.Databricks.Client.Models.MachineLearning.Experiment;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Azure.Databricks.Client.MachineLearning;

public class ExperimentApiClient : ApiClient, IExperimentApi
{
    public ExperimentApiClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<Run> GetRun(string run_id, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{BaseMLFlowApiUri}/runs/get?run_id={run_id}";
        var jsonResponse = await HttpGet<JsonObject>(HttpClient, requestUri, cancellationToken).ConfigureAwait(false);
        jsonResponse.TryGetPropertyValue("run", out var runs);
        return JsonSerializer.Deserialize<Run>(runs);
      
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.Databricks.Client
{
    public class RunState
    {
        /// <summary>
        /// A description of a run’s current location in the run lifecycle. This field is always available in the response.
        /// </summary>
        [JsonProperty(PropertyName = "life_cycle_state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunLifeCycleState LifeCycleState { get; set; }

        /// <summary>
        /// The result state of a run. If it is not available, the response won’t include this field. <see cref="RunResultState"/> for details about the availability of result_state.
        /// </summary>
        [JsonProperty(PropertyName = "result_state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RunResultState? ResultState { get; set; }

        /// <summary>
        /// A descriptive message for the current state.
        /// </summary>
        [JsonProperty(PropertyName = "state_message")]
        public string StateMessage { get; set; }
    }
}

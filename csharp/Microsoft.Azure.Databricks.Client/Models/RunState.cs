using System.Text.Json.Serialization;

namespace Microsoft.Azure.Databricks.Client.Models;

public record RunState
{
    /// <summary>
    /// A description of a run’s current location in the run lifecycle. This field is always available in the response.
    /// </summary>
    [JsonPropertyName("life_cycle_state")]
    public RunLifeCycleState LifeCycleState { get; set; }

    /// <summary>
    /// The result state of a run. If it is not available, the response won’t include this field. <see cref="RunResultState"/> for details about the availability of result_state.
    /// </summary>
    [JsonPropertyName("result_state")]
    public RunResultState? ResultState { get; set; }

    /// <summary>
    /// A descriptive message for the current state.
    /// </summary>
    [JsonPropertyName("state_message")]
    public string StateMessage { get; set; }

    /// <summary>
    /// Whether a run was canceled manually by a user or by the scheduler
    /// because the run timed out.
    /// </summary>
    [JsonPropertyName("user_cancelled_or_timedout")]
    public bool UserCancelledOrTimedOut { get; set; }
}
using TaskCore.Common;
using TaskCore.Services.States;
using TaskCore.Services.States.Development;

namespace TaskCore.Services.Handlers;

public class DevelopmentHandler : ITaskTypeHandler
{
    public int TaskTypeId => 2;
    public string TypeName => "Development";
    public int FinalStatus => 4;

    private readonly Dictionary<int, ITaskState> _states = new()
    {
        [1] = new DevelopmentCreatedState(),
        [2] = new DevelopmentSpecCompletedState(),
        [3] = new DevelopmentDevCompletedState(),
        [4] = new DevelopmentDistributionCompletedState(),
    };

    public IReadOnlyList<ITaskState> Statuses =>
        _states.Values.OrderBy(s => s.StatusNumber).ToList();

    public Result ValidateTransitionData(int targetStatus, string? customData)
    {
        if (!_states.TryGetValue(targetStatus, out var state))
            return Result.Fail($"Status {targetStatus} does not exist for Development tasks.");

        return state.ValidateEntryData(customData);
    }
}

using TaskCore.Common;
using TaskCore.Services.States;
using TaskCore.Services.States.Procurement;

namespace TaskCore.Services.Handlers;

public class ProcurementHandler : ITaskTypeHandler
{
    public int TaskTypeId => 1;
    public string TypeName => "Procurement";
    public int FinalStatus => 3;

    private readonly Dictionary<int, ITaskState> _states = new()
    {
        [1] = new ProcurementCreatedState(),
        [2] = new ProcurementOffersReceivedState(),
        [3] = new ProcurementPurchaseCompletedState(),
    };

    public IReadOnlyList<ITaskState> Statuses =>
        _states.Values.OrderBy(s => s.StatusNumber).ToList();

    public Result ValidateTransitionData(int targetStatus, string? customData)
    {
        if (!_states.TryGetValue(targetStatus, out var state))
            return Result.Fail($"Status {targetStatus} does not exist for Procurement tasks.");

        return state.ValidateEntryData(customData);
    }
}

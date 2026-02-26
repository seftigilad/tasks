using TaskCore.Common;

namespace TaskCore.Services.States.Procurement;

public class ProcurementCreatedState : ITaskState
{
    public int StatusNumber => 1;
    public string StatusName => "Created";

    public Result ValidateEntryData(string? customData) => Result.Ok();
}

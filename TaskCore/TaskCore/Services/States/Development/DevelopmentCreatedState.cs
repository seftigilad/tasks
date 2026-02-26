using TaskCore.Common;

namespace TaskCore.Services.States.Development;

/// <summary>Status 1 — Created. No data required.</summary>
public class DevelopmentCreatedState : ITaskState
{
    public int StatusNumber => 1;
    public string StatusName => "Created";

    public Result ValidateEntryData(string? customData) => Result.Ok();
}

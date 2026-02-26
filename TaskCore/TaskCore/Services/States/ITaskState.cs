using TaskCore.Common;

namespace TaskCore.Services.States;

/// <summary>
/// Represents one status in a task's lifecycle.
/// Each state owns the rules for what data is required to enter it.
/// </summary>
public interface ITaskState
{
    int StatusNumber { get; }
    string StatusName { get; }

    /// <summary>
    /// Validates the custom data required to transition INTO this state.
    /// Called by the handler before the status change is persisted.
    /// </summary>
    Result ValidateEntryData(string? customData);
}

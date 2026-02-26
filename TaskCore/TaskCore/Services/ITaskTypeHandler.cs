using TaskCore.Common;
using TaskCore.Services.States;

namespace TaskCore.Services;

/// <summary>
/// Encapsulates all rules that are specific to one task type.
/// Implement this interface (and register in DI) to add a new task type
/// without touching any existing code.
/// </summary>
public interface ITaskTypeHandler
{
    /// <summary>Unique identifier stored in Tasks.TaskTypeId.</summary>
    int TaskTypeId { get; }

    /// <summary>Human-readable name of this task type.</summary>
    string TypeName { get; }

    /// <summary>
    /// The status number from which the task may be closed.
    /// The general workflow enforces this — handlers just declare it.
    /// </summary>
    int FinalStatus { get; }

    /// <summary>All statuses for this task type, ordered by StatusNumber.</summary>
    IReadOnlyList<ITaskState> Statuses { get; }

    /// <summary>
    /// Validates that <paramref name="customData"/> satisfies the requirements
    /// for moving to <paramref name="targetStatus"/>.
    /// Return <see cref="Result.Ok"/> if no data is required or data is valid.
    /// </summary>
    Result ValidateTransitionData(int targetStatus, string? customData);
}

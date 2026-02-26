namespace TaskCore.DTOs;

/// <summary>
/// CustomData is a JSON string whose shape is defined by each task type's handler.
/// e.g. Procurement status-2: { "quotes": ["quote1", "quote2"] }
///      Development status-3: { "branchName": "feature/x" }
/// </summary>
public record ChangeStatusRequest(int TargetStatus, int NextAssignedUserId, string? CustomData);

using System.Text.Json;
using TaskCore.Common;

namespace TaskCore.Services.States.Development;

/// <summary>
/// Status 3 — Development completed.
/// Requires: { "branchName": "..." }
/// </summary>
public class DevelopmentDevCompletedState : ITaskState
{
    public int StatusNumber => 3;
    public string StatusName => "Development Completed";

    public Result ValidateEntryData(string? customData)
    {
        if (string.IsNullOrWhiteSpace(customData))
            return Result.Fail("Status 3 requires: { \"branchName\": \"...\" }");

        try
        {
            using var doc = JsonDocument.Parse(customData);
            if (!doc.RootElement.TryGetProperty("branchName", out var value) ||
                string.IsNullOrWhiteSpace(value.GetString()))
                return Result.Fail("'branchName' must be a non-empty string.");

            return Result.Ok();
        }
        catch (JsonException)
        {
            return Result.Fail("CustomData is not valid JSON.");
        }
    }
}

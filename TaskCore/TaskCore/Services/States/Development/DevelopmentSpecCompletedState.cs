using System.Text.Json;
using TaskCore.Common;

namespace TaskCore.Services.States.Development;

/// <summary>
/// Status 2 — Specification completed.
/// Requires: { "specification": "..." }
/// </summary>
public class DevelopmentSpecCompletedState : ITaskState
{
    public int StatusNumber => 2;
    public string StatusName => "Specification Completed";

    public Result ValidateEntryData(string? customData)
    {
        if (string.IsNullOrWhiteSpace(customData))
            return Result.Fail("Status 2 requires: { \"specification\": \"...\" }");

        try
        {
            using var doc = JsonDocument.Parse(customData);
            if (!doc.RootElement.TryGetProperty("specification", out var value) ||
                string.IsNullOrWhiteSpace(value.GetString()))
                return Result.Fail("'specification' must be a non-empty string.");

            return Result.Ok();
        }
        catch (JsonException)
        {
            return Result.Fail("CustomData is not valid JSON.");
        }
    }
}

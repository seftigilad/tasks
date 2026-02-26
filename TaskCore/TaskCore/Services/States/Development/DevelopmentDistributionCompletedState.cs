using System.Text.Json;
using TaskCore.Common;

namespace TaskCore.Services.States.Development;

/// <summary>
/// Status 4 — Distribution completed. Final status.
/// Requires: { "version": "..." }
/// </summary>
public class DevelopmentDistributionCompletedState : ITaskState
{
    public int StatusNumber => 4;
    public string StatusName => "Distribution Completed";

    public Result ValidateEntryData(string? customData)
    {
        if (string.IsNullOrWhiteSpace(customData))
            return Result.Fail("Status 4 requires: { \"version\": \"...\" }");

        try
        {
            using var doc = JsonDocument.Parse(customData);
            if (!doc.RootElement.TryGetProperty("version", out var value) ||
                string.IsNullOrWhiteSpace(value.GetString()))
                return Result.Fail("'version' must be a non-empty string.");

            return Result.Ok();
        }
        catch (JsonException)
        {
            return Result.Fail("CustomData is not valid JSON.");
        }
    }
}

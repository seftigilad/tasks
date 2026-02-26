using System.Text.Json;
using TaskCore.Common;

namespace TaskCore.Services.States.Procurement;

public class ProcurementPurchaseCompletedState : ITaskState
{
    public int StatusNumber => 3;
    public string StatusName => "Purchase Completed";

    public Result ValidateEntryData(string? customData)
    {
        if (string.IsNullOrWhiteSpace(customData))
            return Result.Fail("Status 3 requires: { \"receipt\": \"...\" }");

        try
        {
            using var doc = JsonDocument.Parse(customData);

            if (!doc.RootElement.TryGetProperty("receipt", out var receipt) ||
                string.IsNullOrWhiteSpace(receipt.GetString()))
                return Result.Fail("'receipt' must be a non-empty string.");

            return Result.Ok();
        }
        catch (JsonException)
        {
            return Result.Fail("CustomData is not valid JSON.");
        }
    }
}

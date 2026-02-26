using System.Text.Json;
using TaskCore.Common;

namespace TaskCore.Services.States.Procurement;

public class ProcurementOffersReceivedState : ITaskState
{
    public int StatusNumber => 2;
    public string StatusName => "Supplier Offers Received";

    public Result ValidateEntryData(string? customData)
    {
        if (string.IsNullOrWhiteSpace(customData))
            return Result.Fail("Status 2 requires: { \"quotes\": [\"...\", \"...\"] }");

        try
        {
            using var doc = JsonDocument.Parse(customData);

            if (!doc.RootElement.TryGetProperty("quotes", out var quotes) ||
                quotes.ValueKind != JsonValueKind.Array)
                return Result.Fail("'quotes' array is required.");

            var items = quotes.EnumerateArray()
                              .Select(q => q.GetString())
                              .ToList();

            if (items.Count != 2 || items.Any(string.IsNullOrWhiteSpace))
                return Result.Fail("'quotes' must contain exactly 2 non-empty price-quote strings.");

            return Result.Ok();
        }
        catch (JsonException)
        {
            return Result.Fail("CustomData is not valid JSON.");
        }
    }
}

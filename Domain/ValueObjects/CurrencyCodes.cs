namespace Domain.ValueObjects;

public static class CurrencyCodes
{
    private static readonly HashSet<string> SupportedCurrencies = new(
        new[]
        {
            "USD", "EUR", "GBP", "TRY", "JPY", "CHF", "CAD", "AUD", "CNY", "RUB"
        },
        StringComparer.OrdinalIgnoreCase);

    public static bool IsValid(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            return false;
        }

        return SupportedCurrencies.Contains(currencyCode);
    }
}


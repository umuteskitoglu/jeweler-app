namespace Domain.ValueObjects;

public class Money : ValueObject
{
    private Money()
    {
    }

    public string Currency { get; private set; } = null!;
    public decimal Amount { get; private set; }

    public Money(decimal amount, string currency)
    {
        if (!CurrencyCodes.IsValid(currency))
            throw new ArgumentException($"Unsupported currency code '{currency}'.", nameof(currency));

        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount cannot be negative.");

        Currency = currency.ToUpperInvariant();
        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
    }

    public Money ConvertTo(string targetCurrency, decimal conversionRate)
    {
        if (!CurrencyCodes.IsValid(targetCurrency))
            throw new ArgumentException($"Unsupported currency code '{targetCurrency}'.", nameof(targetCurrency));

        if (conversionRate <= 0)
            throw new ArgumentOutOfRangeException(nameof(conversionRate), "Conversion rate must be positive.");

        var convertedAmount = decimal.Round(Amount * conversionRate, 2, MidpointRounding.AwayFromZero);
        return new Money(convertedAmount, targetCurrency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Currency;
        yield return Amount;
    }

    public override bool Equals(object obj)
    {
        if (obj is Money other)
        {
            return Amount == other.Amount && Currency == other.Currency;
        }

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
}
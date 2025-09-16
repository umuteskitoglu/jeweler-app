namespace Domain.ValueObjects;

public class Money : ValueObject
{
    private Money()
    {
    }

    public string Currency { get; set; }
    public decimal Amount { get; set; }

    public Money(string currency, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty");
        Currency = currency;
        Amount = amount;
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
namespace Domain.ValueObjects;

public class ProductName : ValueObject
{
    private ProductName() { }
    public ProductName(string value, string slug)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Product name cannot be empty");
        Value = value;
        Slug = slug;
    }

    public string Value { get; set; }

    public string Slug { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Slug;
    }

    public override bool Equals(object obj)
    {
        if (obj is ProductName other)
        {
            return Value == other.Value && Slug == other.Slug;
        }

        return false;
    }

    public override int GetHashCode() => HashCode.Combine(Value, Slug);
}
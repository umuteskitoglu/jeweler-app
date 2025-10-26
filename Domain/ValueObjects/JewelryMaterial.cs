namespace Domain.ValueObjects;

/// <summary>
/// Represents jewelry material information
/// </summary>
public class JewelryMaterial : ValueObject
{
    public string Type { get; private set; } = null!; // Gold, Silver, Platinum, etc.
    public string? Purity { get; private set; } // 14K, 18K, 24K, 925 Sterling, etc.
    public decimal? Weight { get; private set; } // Weight in grams
    public string? WeightUnit { get; private set; } // grams, ounces, etc.

    private JewelryMaterial()
    {
    }

    public JewelryMaterial(string type, string? purity = null, decimal? weight = null, string? weightUnit = "gram")
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Material type cannot be empty", nameof(type));

        Type = type;
        Purity = purity;
        Weight = weight;
        WeightUnit = weightUnit;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Purity ?? string.Empty;
        yield return Weight ?? 0;
        yield return WeightUnit ?? string.Empty;
    }
}


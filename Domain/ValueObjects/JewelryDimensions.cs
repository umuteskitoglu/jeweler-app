namespace Domain.ValueObjects;

/// <summary>
/// Represents jewelry dimensions
/// </summary>
public class JewelryDimensions : ValueObject
{
    public decimal? Length { get; private set; }
    public decimal? Width { get; private set; }
    public decimal? Height { get; private set; }
    public decimal? RingSize { get; private set; }
    public string Unit { get; private set; } = "mm";

    private JewelryDimensions()
    {
    }

    public JewelryDimensions(
        decimal? length = null, 
        decimal? width = null, 
        decimal? height = null, 
        decimal? ringSize = null,
        string unit = "mm")
    {
        Length = length;
        Width = width;
        Height = height;
        RingSize = ringSize;
        Unit = unit;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Length ?? 0;
        yield return Width ?? 0;
        yield return Height ?? 0;
        yield return RingSize ?? 0;
        yield return Unit;
    }
}


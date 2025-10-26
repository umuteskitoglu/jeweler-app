namespace Domain.ValueObjects;

/// <summary>
/// Specific properties for rings
/// </summary>
public class RingSpecification : ValueObject
{
    public decimal? Size { get; private set; }
    public bool IsResizable { get; private set; }
    public decimal? MinSize { get; private set; }
    public decimal? MaxSize { get; private set; }
    public string? Style { get; private set; } // Solitaire, Eternity, Cocktail, etc.
    public string? Setting { get; private set; } // Prong, Bezel, Pave, Channel, etc.

    private RingSpecification()
    {
    }

    public RingSpecification(
        decimal? size = null,
        bool isResizable = false,
        decimal? minSize = null,
        decimal? maxSize = null,
        string? style = null,
        string? setting = null)
    {
        Size = size;
        IsResizable = isResizable;
        MinSize = minSize;
        MaxSize = maxSize;
        Style = style;
        Setting = setting;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Size ?? 0;
        yield return IsResizable;
        yield return MinSize ?? 0;
        yield return MaxSize ?? 0;
        yield return Style ?? string.Empty;
        yield return Setting ?? string.Empty;
    }
}


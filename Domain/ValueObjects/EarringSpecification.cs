namespace Domain.ValueObjects;

/// <summary>
/// Specific properties for earrings
/// </summary>
public class EarringSpecification : ValueObject
{
    public string? BackingType { get; private set; } // Post, Hook, Clip, Lever, etc.
    public bool IsPair { get; private set; }
    public decimal? DropLength { get; private set; } // For drop/dangle earrings
    public bool IsHypoallergenic { get; private set; }

    private EarringSpecification()
    {
    }

    public EarringSpecification(
        string? backingType = null,
        bool isPair = true,
        decimal? dropLength = null,
        bool isHypoallergenic = false)
    {
        BackingType = backingType;
        IsPair = isPair;
        DropLength = dropLength;
        IsHypoallergenic = isHypoallergenic;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BackingType ?? string.Empty;
        yield return IsPair;
        yield return DropLength ?? 0;
        yield return IsHypoallergenic;
    }
}


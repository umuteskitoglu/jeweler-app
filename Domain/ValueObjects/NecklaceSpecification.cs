using Domain.Enums;

namespace Domain.ValueObjects;

/// <summary>
/// Specific properties for necklaces
/// </summary>
public class NecklaceSpecification : ValueObject
{
    public decimal? ChainLength { get; private set; } // in cm or inches
    public ChainType ChainType { get; private set; }
    public ClaspType ClaspType { get; private set; }
    public bool HasPendant { get; private set; }
    public string? PendantDescription { get; private set; }
    public bool IsAdjustable { get; private set; }
    public decimal? MinLength { get; private set; }
    public decimal? MaxLength { get; private set; }

    private NecklaceSpecification()
    {
    }

    public NecklaceSpecification(
        decimal? chainLength = null,
        ChainType chainType = ChainType.None,
        ClaspType claspType = ClaspType.None,
        bool hasPendant = false,
        string? pendantDescription = null,
        bool isAdjustable = false,
        decimal? minLength = null,
        decimal? maxLength = null)
    {
        ChainLength = chainLength;
        ChainType = chainType;
        ClaspType = claspType;
        HasPendant = hasPendant;
        PendantDescription = pendantDescription;
        IsAdjustable = isAdjustable;
        MinLength = minLength;
        MaxLength = maxLength;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ChainLength ?? 0;
        yield return ChainType;
        yield return ClaspType;
        yield return HasPendant;
        yield return PendantDescription ?? string.Empty;
        yield return IsAdjustable;
        yield return MinLength ?? 0;
        yield return MaxLength ?? 0;
    }
}


namespace Domain.ValueObjects;

/// <summary>
/// Represents gemstone information
/// </summary>
public class Gemstone : ValueObject
{
    public string Type { get; private set; } = null!; // Diamond, Ruby, Emerald, Sapphire, etc.
    public decimal? Carat { get; private set; }
    public string? Cut { get; private set; } // Round, Princess, Oval, etc.
    public string? Color { get; private set; } // D, E, F for diamonds or color description
    public string? Clarity { get; private set; } // IF, VVS1, VVS2, VS1, etc.
    public string? CertificateNumber { get; private set; }
    public string? CertificateAuthority { get; private set; } // GIA, IGI, HRD, etc.

    private Gemstone()
    {
    }

    public Gemstone(
        string type, 
        decimal? carat = null, 
        string? cut = null, 
        string? color = null, 
        string? clarity = null,
        string? certificateNumber = null,
        string? certificateAuthority = null)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Gemstone type cannot be empty", nameof(type));

        Type = type;
        Carat = carat;
        Cut = cut;
        Color = color;
        Clarity = clarity;
        CertificateNumber = certificateNumber;
        CertificateAuthority = certificateAuthority;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Carat ?? 0;
        yield return Cut ?? string.Empty;
        yield return Color ?? string.Empty;
        yield return Clarity ?? string.Empty;
        yield return CertificateNumber ?? string.Empty;
        yield return CertificateAuthority ?? string.Empty;
    }
}


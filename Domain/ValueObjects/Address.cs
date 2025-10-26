namespace Domain.ValueObjects;

/// <summary>
/// Represents a physical address
/// </summary>
public class Address : ValueObject
{
    public string FullName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string AddressLine1 { get; private set; } = null!;
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; } = null!;
    public string? District { get; private set; }
    public string? State { get; private set; }
    public string PostalCode { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string? TaxId { get; private set; } // TC Kimlik No or Tax Number
    public string? TaxOffice { get; private set; } // Vergi Dairesi

    private Address()
    {
    }

    public Address(
        string fullName,
        string phoneNumber,
        string addressLine1,
        string city,
        string postalCode,
        string country,
        string? addressLine2 = null,
        string? district = null,
        string? state = null,
        string? taxId = null,
        string? taxOffice = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Full name cannot be empty", nameof(fullName));
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));
        
        if (string.IsNullOrWhiteSpace(addressLine1))
            throw new ArgumentException("Address line 1 cannot be empty", nameof(addressLine1));
        
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));
        
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));
        
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country cannot be empty", nameof(country));

        FullName = fullName;
        PhoneNumber = phoneNumber;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        District = district;
        State = state;
        PostalCode = postalCode;
        Country = country;
        TaxId = taxId;
        TaxOffice = taxOffice;
    }

    public string GetFullAddress()
    {
        var parts = new List<string> { AddressLine1 };
        
        if (!string.IsNullOrWhiteSpace(AddressLine2))
            parts.Add(AddressLine2);
        
        if (!string.IsNullOrWhiteSpace(District))
            parts.Add(District);
        
        parts.Add(City);
        
        if (!string.IsNullOrWhiteSpace(State))
            parts.Add(State);
        
        parts.Add(PostalCode);
        parts.Add(Country);

        return string.Join(", ", parts);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullName;
        yield return PhoneNumber;
        yield return AddressLine1;
        yield return AddressLine2 ?? string.Empty;
        yield return City;
        yield return District ?? string.Empty;
        yield return State ?? string.Empty;
        yield return PostalCode;
        yield return Country;
        yield return TaxId ?? string.Empty;
        yield return TaxOffice ?? string.Empty;
    }
}


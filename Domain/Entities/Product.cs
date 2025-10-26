using Domain.ValueObjects;
using Domain.Enums;

namespace Domain.Entities;

public class Product
{
    private readonly List<Gemstone> _gemstones = new();
    private readonly List<string> _imageUrls = new();

    private Product()
    {
    }

    public Product(
        ProductName name, 
        Money price, 
        decimal stock, 
        Guid? categoryId, 
        AuditInfo created,
        JewelryType jewelryType = JewelryType.Other,
        string? description = null,
        string? sku = null,
        JewelryMaterial? material = null,
        JewelryDimensions? dimensions = null,
        string? collectionName = null,
        Gender targetGender = Gender.Unisex)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        JewelryType = jewelryType;
        Description = description;
        SKU = sku ?? GenerateSKU();
        Material = material;
        Dimensions = dimensions;
        CollectionName = collectionName;
        TargetGender = targetGender;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public ProductName Name { get; set; }
    public string? Description { get; private set; }
    public string SKU { get; private set; } = null!;
    public Money Price { get; set; }
    public decimal Stock { get; set; }
    
    // Jewelry-specific properties
    public JewelryType JewelryType { get; private set; }
    public Gender TargetGender { get; private set; }
    public JewelryMaterial? Material { get; private set; }
    public IReadOnlyCollection<Gemstone> Gemstones => _gemstones.AsReadOnly();
    public JewelryDimensions? Dimensions { get; private set; }
    
    // Type-specific specifications
    public NecklaceSpecification? NecklaceSpec { get; private set; }
    public RingSpecification? RingSpec { get; private set; }
    public EarringSpecification? EarringSpec { get; private set; }
    
    public string? CollectionName { get; private set; }
    public bool IsCustomizable { get; private set; }
    public string? CertificateNumber { get; private set; }
    public IReadOnlyCollection<string> ImageUrls => _imageUrls.AsReadOnly();
    
    // Category relationship
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }
    
    // Audit fields
    public AuditInfo Created { get; private set; }
    public AuditInfo Updated { get; private set; }
    public AuditInfo? Deleted { get; private set; }

    public bool IsDeleted => Deleted?.At.HasValue ?? false;

    public void Update(
        ProductName name, 
        Money price, 
        decimal stock, 
        Guid categoryId, 
        AuditInfo updated,
        JewelryType? jewelryType = null,
        string? description = null,
        JewelryMaterial? material = null,
        JewelryDimensions? dimensions = null,
        string? collectionName = null,
        Gender? targetGender = null)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        if (jewelryType.HasValue)
            JewelryType = jewelryType.Value;
        Description = description;
        Material = material;
        Dimensions = dimensions;
        CollectionName = collectionName;
        if (targetGender.HasValue)
            TargetGender = targetGender.Value;
        Updated = updated;
    }

    public void SetNecklaceSpecification(NecklaceSpecification spec)
    {
        if (JewelryType != JewelryType.Necklace)
            throw new InvalidOperationException("Cannot set necklace specification for non-necklace product");
        NecklaceSpec = spec;
    }

    public void SetRingSpecification(RingSpecification spec)
    {
        if (JewelryType != JewelryType.Ring)
            throw new InvalidOperationException("Cannot set ring specification for non-ring product");
        RingSpec = spec;
    }

    public void SetEarringSpecification(EarringSpecification spec)
    {
        if (JewelryType != JewelryType.Earring)
            throw new InvalidOperationException("Cannot set earring specification for non-earring product");
        EarringSpec = spec;
    }

    public void AddGemstone(Gemstone gemstone)
    {
        if (gemstone == null)
            throw new ArgumentNullException(nameof(gemstone));
        
        _gemstones.Add(gemstone);
    }

    public void RemoveGemstone(Gemstone gemstone)
    {
        _gemstones.Remove(gemstone);
    }

    public void ClearGemstones()
    {
        _gemstones.Clear();
    }

    public void AddImage(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty", nameof(imageUrl));
        
        _imageUrls.Add(imageUrl);
    }

    public void RemoveImage(string imageUrl)
    {
        _imageUrls.Remove(imageUrl);
    }

    public void SetCustomizable(bool isCustomizable)
    {
        IsCustomizable = isCustomizable;
    }

    public void SetCertificate(string certificateNumber)
    {
        CertificateNumber = certificateNumber;
    }

    public void Delete(AuditInfo deleted)
    {
        Deleted = deleted;
    }

    public void Restore(AuditInfo restored)
    {
        Deleted = restored;
    }

    private static string GenerateSKU()
    {
        return $"JWL-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
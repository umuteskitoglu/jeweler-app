using Domain.ValueObjects;
using Domain.Enums;

namespace Application.Products.Dtos;

public class CreateProductDto
{
    public ProductName Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public Money Price { get; set; }
    public decimal Stock { get; set; }
    public Guid CategoryId { get; set; }
    
    // Jewelry-specific properties
    public JewelryType JewelryType { get; set; } = JewelryType.Other;
    public Gender TargetGender { get; set; } = Gender.Unisex;
    public JewelryMaterial? Material { get; set; }
    public List<Gemstone> Gemstones { get; set; } = new();
    public JewelryDimensions? Dimensions { get; set; }
    
    // Type-specific specifications
    public NecklaceSpecification? NecklaceSpec { get; set; }
    public RingSpecification? RingSpec { get; set; }
    public EarringSpecification? EarringSpec { get; set; }
    
    public string? CollectionName { get; set; }
    public bool IsCustomizable { get; set; }
    public string? CertificateNumber { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
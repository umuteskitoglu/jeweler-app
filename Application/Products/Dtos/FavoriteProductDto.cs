using Domain.ValueObjects;
using Domain.Enums;

namespace Application.Products.Dtos;

public class FavoriteProductDto
{
    public Guid FavoriteId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductSlug { get; set; } = null!;
    public Money Price { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public JewelryType JewelryType { get; set; }
    public bool IsInStock { get; set; }
    public DateTime? AddedAt { get; set; }
}


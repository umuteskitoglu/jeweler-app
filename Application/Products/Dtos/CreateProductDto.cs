using Domain.ValueObjects;

namespace Application.Products.Dtos;

public class CreateProductDto
{
    public Guid Id { get; set; }
    public ProductName Name { get; set; } = null!;
    public Money Price { get; set; }
    public decimal Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted => DeletedAt.HasValue;
    
}
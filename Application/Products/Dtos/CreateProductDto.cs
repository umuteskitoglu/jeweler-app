using Domain.ValueObjects;

namespace Application.Products.Dtos;

public class CreateProductDto
{
    public ProductName Name { get; set; } = null!;
    public Money Price { get; set; }
    public decimal Stock { get; set; }
    public Guid CategoryId { get; set; }
}
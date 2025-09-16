using Domain.ValueObjects;

namespace Application.Products.Dtos;

public class UpdateProductDto
{
    public Guid Id { get; set; }
    public ProductName Name { get; set; } = null!;
    public Money Price { get; set; }
    public decimal Stock { get; set; }
}
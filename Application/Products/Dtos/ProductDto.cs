using Domain.ValueObjects;

namespace Application.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    public string Slug { get; set; }
    public Money Price { get; set; }
    public decimal Stock { get; set; }
    public AuditInfo Created { get; set; }
    public AuditInfo Updated { get; set; }
}
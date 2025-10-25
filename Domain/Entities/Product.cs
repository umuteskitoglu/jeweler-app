using Domain.ValueObjects;

namespace Domain.Entities;

public class Product
{
    private Product()
    {
    }

    public Product(ProductName name, Money price, decimal stock, Guid? categoryId, AuditInfo created)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public ProductName Name { get; set; }
    public Money Price { get; set; }
    public decimal Stock { get; set; }
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public AuditInfo Created { get; private set; }
    public AuditInfo Updated { get; private set; }
    public AuditInfo? Deleted { get; private set; }

    public bool IsDeleted => Deleted?.At.HasValue ?? false;

    public void Update(ProductName name, Money price, decimal stock, Guid categoryId, AuditInfo updated)
    {
        Name = name;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        Updated = updated;
    }

    public void Delete(AuditInfo deleted)
    {
        Deleted = deleted;
    }

    public void Restore(AuditInfo restored)
    {
        Deleted = restored;
    }
}
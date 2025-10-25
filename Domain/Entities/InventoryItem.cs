using Domain.ValueObjects;

namespace Domain.Entities;

public class InventoryItem
{
    private InventoryItem()
    {
    }

    public InventoryItem(Guid productId, decimal quantity, decimal reservedQuantity, AuditInfo created)
    {
        if (quantity < 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        if (reservedQuantity < 0)
            throw new ArgumentOutOfRangeException(nameof(reservedQuantity));

        ProductId = productId;
        Quantity = quantity;
        ReservedQuantity = reservedQuantity;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public decimal Quantity { get; private set; }
    public decimal ReservedQuantity { get; private set; }
    public AuditInfo Created { get; private set; }
    public AuditInfo Updated { get; private set; }
    public AuditInfo? Deleted { get; private set; }

    public bool IsDeleted => Deleted?.At.HasValue ?? false;

    public decimal AvailableQuantity => Quantity - ReservedQuantity;

    public void AdjustQuantity(decimal adjustment, AuditInfo auditInfo)
    {
        if (Quantity + adjustment < 0)
            throw new InvalidOperationException("Quantity cannot be negative.");

        Quantity += adjustment;
        Updated = auditInfo;
    }

    public void Reserve(decimal quantity, AuditInfo auditInfo)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        if (ReservedQuantity + quantity > Quantity)
            throw new InvalidOperationException("Insufficient inventory to reserve.");

        ReservedQuantity += quantity;
        Updated = auditInfo;
    }

    public void Release(decimal quantity, AuditInfo auditInfo)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        if (ReservedQuantity - quantity < 0)
            throw new InvalidOperationException("Reserved quantity cannot be negative.");

        ReservedQuantity -= quantity;
        Updated = auditInfo;
    }

    public void Delete(AuditInfo auditInfo)
    {
        Deleted = auditInfo;
    }

    public void Restore()
    {
        Deleted = null;
    }
}


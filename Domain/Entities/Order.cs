using System.Collections.Generic;
using System.Linq;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    private Order()
    {
    }

    public Order(Guid customerId, Money totalAmount, string currency, AuditInfo created)
    {
        if (totalAmount.Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(totalAmount));

        CustomerId = customerId;
        TotalAmount = totalAmount;
        Currency = currency;
        Status = OrderStatus.Pending;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CustomerId { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money TotalAmount { get; private set; }
    public string Currency { get; private set; }
    public OrderStatus Status { get; private set; }
    public AuditInfo Created { get; private set; }
    public AuditInfo Updated { get; private set; }
    public AuditInfo? Deleted { get; private set; }

    public bool IsDeleted => Deleted?.At.HasValue ?? false;

    public void AddItem(OrderItem item, AuditInfo auditInfo)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Add(item);
        Updated = auditInfo;
        RecalculateTotal();
    }

    public void RemoveItem(Guid productId, AuditInfo auditInfo)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            Updated = auditInfo;
            RecalculateTotal();
        }
    }

    public void UpdateStatus(OrderStatus status, AuditInfo auditInfo)
    {
        Status = status;
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

    private void RecalculateTotal()
    {
        var totalAmount = _items.Sum(i => i.TotalPrice.Amount);
        TotalAmount = new Money(totalAmount, TotalAmount.Currency);
    }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Paid,
    Shipped,
    Completed,
    Cancelled
}


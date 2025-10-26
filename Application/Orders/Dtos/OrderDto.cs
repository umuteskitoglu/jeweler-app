using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Orders.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Money TotalAmount { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public OrderStatus Status { get; set; }
    public List<OrderItemDetailDto> Items { get; set; } = new();
    
    // Address information
    public Address ShippingAddress { get; set; } = null!;
    public Address BillingAddress { get; set; } = null!;
    public string? CustomerNote { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    
    public AuditInfo Created { get; set; } = null!;
    public AuditInfo Updated { get; set; } = null!;
}

public class OrderItemDetailDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public Money UnitPrice { get; set; } = null!;
    public int Quantity { get; set; }
    public Money TotalPrice { get; set; } = null!;
}


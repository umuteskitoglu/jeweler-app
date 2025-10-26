namespace Application.Orders.Dtos;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public string? Currency { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
    
    // Address information
    public AddressDto ShippingAddress { get; set; } = null!;
    public AddressDto? BillingAddress { get; set; }
    public string? CustomerNote { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddressDto
{
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string? District { get; set; }
    public string? State { get; set; }
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? TaxId { get; set; }
    public string? TaxOffice { get; set; }
}


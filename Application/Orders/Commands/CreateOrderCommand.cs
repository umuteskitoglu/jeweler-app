using Application.Interfaces;
using Application.Orders.Dtos;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Orders.Commands;

public record CreateOrderCommand(CreateOrderDto Order) : IRequest<Guid>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Order.Items == null || !request.Order.Items.Any())
        {
            throw new ArgumentException("Order must contain at least one item.");
        }

        // Calculate total amount
        decimal totalAmount = 0;
        var orderItems = new List<OrderItem>();

        foreach (var item in request.Order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {item.ProductId} not found.");
            }

            if (product.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product {product.Name.Value}.");
            }

            var orderItem = new OrderItem(
                product.Id,
                product.Name.Value,
                product.Price,
                item.Quantity
            );
            
            orderItems.Add(orderItem);
            totalAmount += product.Price.Amount * item.Quantity;
        }

        // Validate addresses
        if (request.Order.ShippingAddress == null)
        {
            throw new ArgumentException("Shipping address is required");
        }

        // Create shipping address
        var shippingAddress = new Address(
            request.Order.ShippingAddress.FullName,
            request.Order.ShippingAddress.PhoneNumber,
            request.Order.ShippingAddress.AddressLine1,
            request.Order.ShippingAddress.City,
            request.Order.ShippingAddress.PostalCode,
            request.Order.ShippingAddress.Country,
            request.Order.ShippingAddress.AddressLine2,
            request.Order.ShippingAddress.District,
            request.Order.ShippingAddress.State,
            request.Order.ShippingAddress.TaxId,
            request.Order.ShippingAddress.TaxOffice
        );

        // Create billing address (use shipping if not provided)
        Address? billingAddress = null;
        if (request.Order.BillingAddress != null)
        {
            billingAddress = new Address(
                request.Order.BillingAddress.FullName,
                request.Order.BillingAddress.PhoneNumber,
                request.Order.BillingAddress.AddressLine1,
                request.Order.BillingAddress.City,
                request.Order.BillingAddress.PostalCode,
                request.Order.BillingAddress.Country,
                request.Order.BillingAddress.AddressLine2,
                request.Order.BillingAddress.District,
                request.Order.BillingAddress.State,
                request.Order.BillingAddress.TaxId,
                request.Order.BillingAddress.TaxOffice
            );
        }

        var order = new Order(
            request.Order.CustomerId,
            new Money(totalAmount, request.Order.Currency ?? "USD"),
            request.Order.Currency ?? "USD",
            shippingAddress,
            new AuditInfo(DateTime.UtcNow, request.Order.CustomerId),
            billingAddress,
            request.Order.CustomerNote
        );

        // Add items to order
        foreach (var item in orderItems)
        {
            order.AddItem(item, new AuditInfo(DateTime.UtcNow, request.Order.CustomerId));
        }

        await _orderRepository.AddAsync(order);
        
        return order.Id;
    }
}


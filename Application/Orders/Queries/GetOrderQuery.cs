using Application.Interfaces;
using Application.Orders.Dtos;
using MediatR;

namespace Application.Orders.Queries;

public record GetOrderQuery(Guid OrderId) : IRequest<OrderDto?>;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderDto?>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            return null;
        }

        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
            Currency = order.Currency,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            BillingAddress = order.BillingAddress,
            CustomerNote = order.CustomerNote,
            TrackingNumber = order.TrackingNumber,
            ShippedAt = order.ShippedAt,
            DeliveredAt = order.DeliveredAt,
            Items = order.Items.Select(i => new OrderItemDetailDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.TotalPrice
            }).ToList(),
            Created = order.Created,
            Updated = order.Updated
        };
    }
}


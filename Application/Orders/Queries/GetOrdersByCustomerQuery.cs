using Application.Interfaces;
using Application.Orders.Dtos;
using MediatR;

namespace Application.Orders.Queries;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IRequest<IEnumerable<OrderDto>>;

public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, IEnumerable<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByCustomerQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByCustomerIdAsync(request.CustomerId);
        
        return orders.Select(order => new OrderDto
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
        }).ToList();
    }
}


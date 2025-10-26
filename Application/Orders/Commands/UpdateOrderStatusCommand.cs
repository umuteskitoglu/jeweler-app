using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Orders.Commands;

public record UpdateOrderStatusCommand(Guid OrderId, OrderStatus Status, Guid UpdatedBy) : IRequest<bool>;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order == null)
        {
            throw new InvalidOperationException($"Order with ID {request.OrderId} not found.");
        }

        order.UpdateStatus(request.Status, new AuditInfo(DateTime.UtcNow, request.UpdatedBy));
        await _orderRepository.UpdateAsync(order);
        
        return true;
    }
}


using Application.Orders.Commands;
using Application.Orders.Dtos;
using Application.Orders.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jewelry.API.Controllers;

/// <summary>
/// Order management operations
/// </summary>
[Authorize]
public class OrdersController : BaseController
{
    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="orderDto">Order details</param>
    /// <returns>Created order ID</returns>
    /// <response code="200">Order created successfully</response>
    /// <response code="400">Invalid order data</response>
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        try
        {
            var command = new CreateOrderCommand(orderDto);
            var orderId = await Mediator.Send(command);
            return Ok(new { orderId, message = "Order created successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Get order by ID
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <returns>Order details</returns>
    /// <response code="200">Returns the order</response>
    /// <response code="404">Order not found</response>
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId)
    {
        var query = new GetOrderQuery(orderId);
        var order = await Mediator.Send(query);
        
        if (order == null)
        {
            return NotFound(new { error = "Order not found" });
        }
        
        return Ok(order);
    }

    /// <summary>
    /// Get all orders for a customer
    /// </summary>
    /// <param name="customerId">Customer ID</param>
    /// <returns>List of customer orders</returns>
    /// <response code="200">Returns customer orders</response>
    [HttpGet("customer/{customerId:guid}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetCustomerOrders(Guid customerId)
    {
        var query = new GetOrdersByCustomerQuery(customerId);
        var orders = await Mediator.Send(query);
        return Ok(orders);
    }

    /// <summary>
    /// Update order status
    /// </summary>
    /// <param name="orderId">Order ID</param>
    /// <param name="request">Status update request</param>
    /// <returns>Success status</returns>
    /// <response code="200">Status updated successfully</response>
    /// <response code="404">Order not found</response>
    [HttpPut("{orderId:guid}/status")]
    public async Task<ActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusRequest request)
    {
        try
        {
            var command = new UpdateOrderStatusCommand(orderId, request.Status, request.UpdatedBy);
            await Mediator.Send(command);
            return Ok(new { message = "Order status updated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}

/// <summary>
/// Update order status request model
/// </summary>
public record UpdateOrderStatusRequest(OrderStatus Status, Guid UpdatedBy);


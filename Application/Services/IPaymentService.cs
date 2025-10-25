using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Services;

public interface IPaymentService
{
    Task<PaymentResult> InitiatePaymentAsync(Order order, string paymentMethod, string callbackUrl);
    Task<PaymentResult> ProcessPaymentCallbackAsync(string token, string conversationId);
    Task<bool> RefundPaymentAsync(Payment payment, Money amount);
}

public record PaymentResult(bool Success, string? PaymentUrl, string? TransactionId, string? ErrorMessage);


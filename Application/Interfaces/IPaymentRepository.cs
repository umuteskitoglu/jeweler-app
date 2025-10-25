using Domain.Entities;

namespace Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id);
    Task<Payment?> GetByOrderIdAsync(Guid orderId);
    Task<Payment?> GetByTransactionIdAsync(string transactionId);
    Task<Payment> AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}


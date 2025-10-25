using Domain.ValueObjects;

namespace Domain.Entities;

public class Payment
{
    private Payment()
    {
    }

    public Payment(Guid orderId, Money amount, string paymentMethod, string provider, AuditInfo created)
    {
        OrderId = orderId;
        Amount = amount;
        PaymentMethod = paymentMethod;
        Provider = provider;
        Status = PaymentStatus.Pending;
        Created = created;
        Updated = new AuditInfo(created.At, created.By);
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OrderId { get; private set; }
    public Money Amount { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public string Provider { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public string? TransactionId { get; private set; }
    public string? ProviderResponse { get; private set; }
    public string? ErrorMessage { get; private set; }
    public AuditInfo Created { get; private set; }
    public AuditInfo Updated { get; private set; }

    public void MarkAsProcessing(string transactionId, AuditInfo auditInfo)
    {
        Status = PaymentStatus.Processing;
        TransactionId = transactionId;
        Updated = auditInfo;
    }

    public void MarkAsCompleted(string providerResponse, AuditInfo auditInfo)
    {
        Status = PaymentStatus.Completed;
        ProviderResponse = providerResponse;
        Updated = auditInfo;
    }

    public void MarkAsFailed(string errorMessage, AuditInfo auditInfo)
    {
        Status = PaymentStatus.Failed;
        ErrorMessage = errorMessage;
        Updated = auditInfo;
    }

    public void MarkAsRefunded(AuditInfo auditInfo)
    {
        Status = PaymentStatus.Refunded;
        Updated = auditInfo;
    }
}

public enum PaymentStatus
{
    Pending,
    Processing,
    Completed,
    Failed,
    Refunded
}


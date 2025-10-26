using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// Represents a user's favorite product (wishlist item)
/// </summary>
public class Favorite
{
    private Favorite()
    {
    }

    public Favorite(Guid userId, Guid productId, AuditInfo created)
    {
        UserId = userId;
        ProductId = productId;
        Created = created;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    
    // Navigation properties
    public User? User { get; private set; }
    public Product? Product { get; private set; }
    
    public AuditInfo Created { get; private set; } = null!;
}


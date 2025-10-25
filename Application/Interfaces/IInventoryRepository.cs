using Domain.Entities;

namespace Application.Interfaces;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(Guid productId);
    Task<IEnumerable<InventoryItem>> GetAllAsync();
    Task<InventoryItem> AddAsync(InventoryItem item);
    Task UpdateAsync(InventoryItem item);
}


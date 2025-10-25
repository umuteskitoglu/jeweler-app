using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class InventoryRepository : IInventoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public InventoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventoryItem?> GetByProductIdAsync(Guid productId)
    {
        return await _dbContext.InventoryItems
            .Include(ii => ii.Product)
            .FirstOrDefaultAsync(ii => ii.ProductId == productId);
    }

    public async Task<IEnumerable<InventoryItem>> GetAllAsync()
    {
        return await _dbContext.InventoryItems
            .Include(ii => ii.Product)
            .Where(ii => !ii.IsDeleted)
            .ToListAsync();
    }

    public async Task<InventoryItem> AddAsync(InventoryItem item)
    {
        _dbContext.InventoryItems.Add(item);
        await _dbContext.SaveChangesAsync();
        return item;
    }

    public async Task UpdateAsync(InventoryItem item)
    {
        _dbContext.InventoryItems.Update(item);
        await _dbContext.SaveChangesAsync();
    }
}


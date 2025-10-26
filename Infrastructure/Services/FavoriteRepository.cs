using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly ApplicationDbContext _context;

    public FavoriteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Favorite?> GetByUserAndProductAsync(Guid userId, Guid productId)
    {
        return await _context.Set<Favorite>()
            .Include(f => f.Product)
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
    }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Set<Favorite>()
            .Include(f => f.Product)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.Created.At)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid productId)
    {
        return await _context.Set<Favorite>()
            .AnyAsync(f => f.UserId == userId && f.ProductId == productId);
    }

    public async Task<int> GetCountByUserIdAsync(Guid userId)
    {
        return await _context.Set<Favorite>()
            .CountAsync(f => f.UserId == userId);
    }

    public async Task<Favorite> AddAsync(Favorite favorite)
    {
        await _context.Set<Favorite>().AddAsync(favorite);
        await _context.SaveChangesAsync();
        return favorite;
    }

    public async Task<bool> RemoveAsync(Guid userId, Guid productId)
    {
        var favorite = await GetByUserAndProductAsync(userId, productId);
        if (favorite == null)
        {
            return false;
        }

        _context.Set<Favorite>().Remove(favorite);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveByIdAsync(Guid favoriteId)
    {
        var favorite = await _context.Set<Favorite>().FindAsync(favoriteId);
        if (favorite == null)
        {
            return false;
        }

        _context.Set<Favorite>().Remove(favorite);
        await _context.SaveChangesAsync();
        return true;
    }
}


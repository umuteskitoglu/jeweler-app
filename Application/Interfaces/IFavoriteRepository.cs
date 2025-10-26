using Domain.Entities;

namespace Application.Interfaces;

public interface IFavoriteRepository
{
    Task<Favorite?> GetByUserAndProductAsync(Guid userId, Guid productId);
    Task<IEnumerable<Favorite>> GetByUserIdAsync(Guid userId);
    Task<bool> ExistsAsync(Guid userId, Guid productId);
    Task<int> GetCountByUserIdAsync(Guid userId);
    Task<Favorite> AddAsync(Favorite favorite);
    Task<bool> RemoveAsync(Guid userId, Guid productId);
    Task<bool> RemoveByIdAsync(Guid favoriteId);
}


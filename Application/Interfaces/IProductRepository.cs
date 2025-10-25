using Domain.Entities;
namespace Application.Interfaces;

public interface IProductRepository
{
    Task<Product> AddAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Product product);
}
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Services;

public class ProductRepository:IProductRepository
{
    public Task<bool> AddAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Product product)
    {
        throw new NotImplementedException();
    }
}
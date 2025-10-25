using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> AddAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(_dbContext.Products
            .Include(p => p.Category)
            .AsEnumerable());
    }

    public Task<Product?> GetByIdAsync(Guid id)
    {
        var product = _dbContext.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Product product)
    {
        _dbContext.Products.Remove(product);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}
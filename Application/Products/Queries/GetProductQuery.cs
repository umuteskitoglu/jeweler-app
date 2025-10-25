using Application.Caching;
using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries;

public class GetProductQuery : ICacheableQuery<ProductDto>
{
    public Guid Id { get; set; }

    public string CacheKey => $"product:{Id}";
    public TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromMinutes(5);
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var p = await _productRepository.GetByIdAsync(request.Id);
        if (p == null)
        {
            throw new KeyNotFoundException($"Product with id {request.Id} not found");
        }

        return new ProductDto()
        {
            Id = p.Id,
            Name = p.Name.Value,
            Slug = p.Name.Slug,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId ?? Guid.Empty,
            CategoryName = p.Category?.Name ?? string.Empty,
            Created = p.Created,
            Updated = p.Updated
        };
    }
}
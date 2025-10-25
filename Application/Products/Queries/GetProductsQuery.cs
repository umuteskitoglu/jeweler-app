using Application.Caching;
using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries;

public class GetProductsQuery : ICacheableQuery<IEnumerable<ProductDto>>
{
    public string CacheKey => "products:all";
    public TimeSpan? AbsoluteExpirationRelativeToNow => TimeSpan.FromMinutes(5);
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return (await _productRepository.GetAllAsync()).Select(p => new ProductDto()
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
        });
    }
}
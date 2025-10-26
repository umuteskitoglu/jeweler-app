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
            Description = p.Description,
            SKU = p.SKU,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId ?? Guid.Empty,
            CategoryName = p.Category?.Name ?? string.Empty,
            JewelryType = p.JewelryType,
            TargetGender = p.TargetGender,
            Material = p.Material,
            Gemstones = p.Gemstones.ToList(),
            Dimensions = p.Dimensions,
            NecklaceSpec = p.NecklaceSpec,
            RingSpec = p.RingSpec,
            EarringSpec = p.EarringSpec,
            CollectionName = p.CollectionName,
            IsCustomizable = p.IsCustomizable,
            CertificateNumber = p.CertificateNumber,
            ImageUrls = p.ImageUrls.ToList(),
            Created = p.Created,
            Updated = p.Updated
        };
    }
}
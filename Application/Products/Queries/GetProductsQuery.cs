using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries;

public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
{
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
            Price = p.Price,
            Stock = p.Stock,
            Created = p.Created,
            Updated = p.Updated
        });
    }
}
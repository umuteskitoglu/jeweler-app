using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Products.Queries;

public class GetProductQuery:IRequest<ProductDto>   
{
    public Guid Id { get; set; }
    public GetProductQuery(Guid id)
    {
        Id = id;
    }
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
            Price = p.Price,
            Stock = p.Stock,
            Created = p.Created,
            Updated = p.Updated
        };
    }
}

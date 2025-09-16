using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Products.Commands;

public abstract class CreateProductCommand(CreateProductDto createProduct) : IRequest<bool>
{
    public CreateProductDto CreateProduct { get; set; } = createProduct;
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = new Product(request.CreateProduct.Name, request.CreateProduct.Price,
            request.CreateProduct.Stock,
            new AuditInfo(DateTime.UtcNow, Guid.Empty));
        return _productRepository.AddAsync(product);
    }
}
using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;
using Domain.Entities;
using Domain.ValueObjects;
using Application.Configuration;

namespace Application.Products.Commands;

public class CreateProductCommand(CreateProductDto createProduct) : IRequest<Product>
{
    public CreateProductDto CreateProduct { get; set; } = createProduct;
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.CreateProduct.Price.Amount < 0)
        {
            throw new ArgumentException("Product price cannot be negative.");
        }

        if (request.CreateProduct.Stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.");
        }

        Product product = new Product(
            request.CreateProduct.Name,
            request.CreateProduct.Price,
            request.CreateProduct.Stock,
            request.CreateProduct.CategoryId,
            new AuditInfo(DateTime.UtcNow, Guid.Empty)); // TODO: Replace Guid.Empty with actual user ID
        return _productRepository.AddAsync(product);
    }
}
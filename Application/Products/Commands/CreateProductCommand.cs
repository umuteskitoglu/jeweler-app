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

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.CreateProduct.Price.Amount < 0)
        {
            throw new ArgumentException("Product price cannot be negative.");
        }

        if (request.CreateProduct.Stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.");
        }

        var product = new Product(
            request.CreateProduct.Name,
            request.CreateProduct.Price,
            request.CreateProduct.Stock,
            request.CreateProduct.CategoryId,
            new AuditInfo(DateTime.UtcNow, Guid.Empty), // TODO: Replace Guid.Empty with actual user ID
            request.CreateProduct.JewelryType,
            request.CreateProduct.Description,
            request.CreateProduct.SKU,
            request.CreateProduct.Material,
            request.CreateProduct.Dimensions,
            request.CreateProduct.CollectionName,
            request.CreateProduct.TargetGender
        );

        // Add gemstones
        foreach (var gemstone in request.CreateProduct.Gemstones)
        {
            product.AddGemstone(gemstone);
        }

        // Add images
        foreach (var imageUrl in request.CreateProduct.ImageUrls)
        {
            product.AddImage(imageUrl);
        }

        // Set type-specific specifications
        if (request.CreateProduct.NecklaceSpec != null)
        {
            product.SetNecklaceSpecification(request.CreateProduct.NecklaceSpec);
        }
        if (request.CreateProduct.RingSpec != null)
        {
            product.SetRingSpecification(request.CreateProduct.RingSpec);
        }
        if (request.CreateProduct.EarringSpec != null)
        {
            product.SetEarringSpecification(request.CreateProduct.EarringSpec);
        }

        // Set other properties
        product.SetCustomizable(request.CreateProduct.IsCustomizable);
        if (!string.IsNullOrWhiteSpace(request.CreateProduct.CertificateNumber))
        {
            product.SetCertificate(request.CreateProduct.CertificateNumber);
        }

        return await _productRepository.AddAsync(product);
    }
}
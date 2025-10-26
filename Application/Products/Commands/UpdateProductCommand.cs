using Application.Interfaces;
using Application.Products.Dtos;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Products.Commands;

public class UpdateProductCommand : IRequest<bool>
{
    public UpdateProductCommand(UpdateProductDto product)
    {
        this.product = product;
    }

    public UpdateProductDto product { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetByIdAsync(request.product.Id);
        if (product == null)
            throw new Exception("Product not found");
        
        if (request.product.Price.Amount < 0)
        {
            throw new ArgumentException("Product price cannot be negative.");
        }

        if (request.product.Stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.");
        }

        product.Update(
            request.product.Name, 
            request.product.Price, 
            request.product.Stock, 
            request.product.CategoryId, 
            new AuditInfo(DateTime.UtcNow, request.product.UpdatedBy),
            request.product.JewelryType,
            request.product.Description,
            request.product.Material,
            request.product.Dimensions,
            request.product.CollectionName,
            request.product.TargetGender
        );

        // Update gemstones
        product.ClearGemstones();
        foreach (var gemstone in request.product.Gemstones)
        {
            product.AddGemstone(gemstone);
        }

        // Update type-specific specifications
        if (request.product.NecklaceSpec != null)
        {
            product.SetNecklaceSpecification(request.product.NecklaceSpec);
        }
        if (request.product.RingSpec != null)
        {
            product.SetRingSpecification(request.product.RingSpec);
        }
        if (request.product.EarringSpec != null)
        {
            product.SetEarringSpecification(request.product.EarringSpec);
        }

        // Note: For images, you might want a more sophisticated update strategy
        // For now, this is a simple example

        // Update other properties
        product.SetCustomizable(request.product.IsCustomizable);
        if (!string.IsNullOrWhiteSpace(request.product.CertificateNumber))
        {
            product.SetCertificate(request.product.CertificateNumber);
        }

        return await _productRepository.UpdateAsync(product);
    }
}
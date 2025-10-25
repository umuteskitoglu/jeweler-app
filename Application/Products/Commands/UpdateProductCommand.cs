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

        product.Update(request.product.Name, request.product.Price, request.product.Stock, request.product.CategoryId, new AuditInfo(DateTime.UtcNow, request.product.UpdatedBy));
        return await _productRepository.UpdateAsync(product);
    }
}
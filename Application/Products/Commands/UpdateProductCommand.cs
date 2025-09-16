using Application.Interfaces;
using Application.Products.Dtos;
using Domain.Entities;
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
        product.Name = request.product.Name;
        product.Price = request.product.Price;
        product.Stock = request.product.Stock;
        return await _productRepository.UpdateAsync(product);
    }
}
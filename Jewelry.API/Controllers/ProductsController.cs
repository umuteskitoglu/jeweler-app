using Application.Products.Commands;
using Application.Products.Dtos;
using Application.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Jewelry.API.Controllers;

/// <summary>
/// Product management operations
/// </summary>
public class ProductsController : BaseController
{
    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    /// <param name="query">The product query containing the product ID</param>
    /// <returns>Product details</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="404">Product not found</response>
    [HttpGet]
    public Task<ProductDto> Get(GetProductQuery query)
    {
        return Mediator.Send(query);
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all products</returns>
    /// <response code="200">Returns all products</response>
    [HttpGet("all")]
    public Task<IEnumerable<ProductDto>> GetAll()
    {
        return Mediator.Send(new GetProductsQuery());
    }
    /// <summary>
    /// Creates the product
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    [HttpPost("create-product")]
    public Task Create(CreateProductDto product)
    {
        return Mediator.Send(new CreateProductCommand(product));
    }
    
}
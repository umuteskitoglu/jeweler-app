using Application.Favorites.Commands;
using Application.Favorites.Queries;
using Application.Products.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jewelry.API.Controllers;

/// <summary>
/// Favorites/Wishlist operations
/// </summary>
[Authorize]
public class FavoritesController : BaseController
{
    /// <summary>
    /// Add a product to favorites
    /// </summary>
    /// <param name="request">Product ID to add</param>
    /// <returns>Created favorite ID</returns>
    /// <response code="200">Product added to favorites successfully</response>
    /// <response code="400">Product already in favorites or invalid request</response>
    /// <response code="404">Product or user not found</response>
    [HttpPost("add")]
    public async Task<ActionResult<Guid>> AddToFavorites([FromBody] AddToFavoritesRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new AddToFavoritesCommand(userId, request.ProductId);
            var favoriteId = await Mediator.Send(command);
            
            return Ok(new 
            { 
                favoriteId, 
                message = "Product added to favorites successfully" 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Remove a product from favorites
    /// </summary>
    /// <param name="productId">Product ID to remove</param>
    /// <returns>Success status</returns>
    /// <response code="200">Product removed from favorites successfully</response>
    /// <response code="404">Favorite not found</response>
    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult> RemoveFromFavorites(Guid productId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var command = new RemoveFromFavoritesCommand(userId, productId);
            await Mediator.Send(command);
            
            return Ok(new { message = "Product removed from favorites successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Toggle product in favorites (add if not exists, remove if exists)
    /// </summary>
    /// <param name="request">Product ID to toggle</param>
    /// <returns>Current favorite status</returns>
    /// <response code="200">Returns whether product is now in favorites</response>
    [HttpPost("toggle")]
    public async Task<ActionResult<object>> ToggleFavorite([FromBody] AddToFavoritesRequest request)
    {
        var userId = GetCurrentUserId();
        
        // Check if already exists
        var existsQuery = new CheckIsFavoriteQuery(userId, request.ProductId);
        var exists = await Mediator.Send(existsQuery);

        if (exists)
        {
            // Remove
            var removeCommand = new RemoveFromFavoritesCommand(userId, request.ProductId);
            await Mediator.Send(removeCommand);
            
            return Ok(new 
            { 
                isFavorite = false, 
                message = "Product removed from favorites" 
            });
        }
        else
        {
            // Add
            try
            {
                var addCommand = new AddToFavoritesCommand(userId, request.ProductId);
                var favoriteId = await Mediator.Send(addCommand);
                
                return Ok(new 
                { 
                    isFavorite = true, 
                    favoriteId,
                    message = "Product added to favorites" 
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    /// <summary>
    /// Get all favorite products for current user
    /// </summary>
    /// <returns>List of favorite products</returns>
    /// <response code="200">Returns list of favorites</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoriteProductDto>>> GetMyFavorites()
    {
        var userId = GetCurrentUserId();
        var query = new GetUserFavoritesQuery(userId);
        var favorites = await Mediator.Send(query);
        
        return Ok(favorites);
    }

    /// <summary>
    /// Check if a product is in favorites
    /// </summary>
    /// <param name="productId">Product ID to check</param>
    /// <returns>True if product is in favorites</returns>
    /// <response code="200">Returns favorite status</response>
    [HttpGet("check/{productId:guid}")]
    public async Task<ActionResult<object>> CheckIsFavorite(Guid productId)
    {
        var userId = GetCurrentUserId();
        var query = new CheckIsFavoriteQuery(userId, productId);
        var isFavorite = await Mediator.Send(query);
        
        return Ok(new { isFavorite });
    }

    /// <summary>
    /// Get count of favorites for current user
    /// </summary>
    /// <returns>Number of favorites</returns>
    /// <response code="200">Returns favorites count</response>
    [HttpGet("count")]
    public async Task<ActionResult<object>> GetFavoritesCount()
    {
        var userId = GetCurrentUserId();
        var query = new GetFavoritesCountQuery(userId);
        var count = await Mediator.Send(query);
        
        return Ok(new { count });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }
        return userId;
    }
}

/// <summary>
/// Request to add product to favorites
/// </summary>
public record AddToFavoritesRequest(Guid ProductId);


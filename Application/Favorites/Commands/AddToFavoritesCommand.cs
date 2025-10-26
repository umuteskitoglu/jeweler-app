using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Favorites.Commands;

public record AddToFavoritesCommand(Guid UserId, Guid ProductId) : IRequest<Guid>;

public class AddToFavoritesCommandHandler : IRequestHandler<AddToFavoritesCommand, Guid>
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public AddToFavoritesCommandHandler(
        IFavoriteRepository favoriteRepository,
        IProductRepository productRepository,
        IUserRepository userRepository)
    {
        _favoriteRepository = favoriteRepository;
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }

        // Check if already in favorites
        var exists = await _favoriteRepository.ExistsAsync(request.UserId, request.ProductId);
        if (exists)
        {
            throw new InvalidOperationException("Product is already in favorites");
        }

        var favorite = new Favorite(
            request.UserId,
            request.ProductId,
            new AuditInfo(DateTime.UtcNow, request.UserId)
        );

        var result = await _favoriteRepository.AddAsync(favorite);
        return result.Id;
    }
}


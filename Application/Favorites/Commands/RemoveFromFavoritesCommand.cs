using Application.Interfaces;
using MediatR;

namespace Application.Favorites.Commands;

public record RemoveFromFavoritesCommand(Guid UserId, Guid ProductId) : IRequest<bool>;

public class RemoveFromFavoritesCommandHandler : IRequestHandler<RemoveFromFavoritesCommand, bool>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public RemoveFromFavoritesCommandHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<bool> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
    {
        var result = await _favoriteRepository.RemoveAsync(request.UserId, request.ProductId);
        
        if (!result)
        {
            throw new InvalidOperationException("Favorite not found");
        }

        return result;
    }
}


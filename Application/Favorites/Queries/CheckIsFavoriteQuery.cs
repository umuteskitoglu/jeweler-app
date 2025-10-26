using Application.Interfaces;
using MediatR;

namespace Application.Favorites.Queries;

public record CheckIsFavoriteQuery(Guid UserId, Guid ProductId) : IRequest<bool>;

public class CheckIsFavoriteQueryHandler : IRequestHandler<CheckIsFavoriteQuery, bool>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public CheckIsFavoriteQueryHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<bool> Handle(CheckIsFavoriteQuery request, CancellationToken cancellationToken)
    {
        return await _favoriteRepository.ExistsAsync(request.UserId, request.ProductId);
    }
}


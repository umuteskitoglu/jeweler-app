using Application.Interfaces;
using MediatR;

namespace Application.Favorites.Queries;

public record GetFavoritesCountQuery(Guid UserId) : IRequest<int>;

public class GetFavoritesCountQueryHandler : IRequestHandler<GetFavoritesCountQuery, int>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public GetFavoritesCountQueryHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<int> Handle(GetFavoritesCountQuery request, CancellationToken cancellationToken)
    {
        return await _favoriteRepository.GetCountByUserIdAsync(request.UserId);
    }
}


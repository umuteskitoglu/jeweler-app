using Application.Interfaces;
using Application.Products.Dtos;
using MediatR;

namespace Application.Favorites.Queries;

public record GetUserFavoritesQuery(Guid UserId) : IRequest<IEnumerable<FavoriteProductDto>>;

public class GetUserFavoritesQueryHandler : IRequestHandler<GetUserFavoritesQuery, IEnumerable<FavoriteProductDto>>
{
    private readonly IFavoriteRepository _favoriteRepository;

    public GetUserFavoritesQueryHandler(IFavoriteRepository favoriteRepository)
    {
        _favoriteRepository = favoriteRepository;
    }

    public async Task<IEnumerable<FavoriteProductDto>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favorites = await _favoriteRepository.GetByUserIdAsync(request.UserId);

        return favorites
            .Where(f => f.Product != null)
            .Select(f => new FavoriteProductDto
            {
                FavoriteId = f.Id,
                ProductId = f.ProductId,
                ProductName = f.Product!.Name.Value,
                ProductSlug = f.Product.Name.Slug,
                Price = f.Product.Price,
                ImageUrl = f.Product.ImageUrls.FirstOrDefault(),
                JewelryType = f.Product.JewelryType,
                IsInStock = f.Product.Stock > 0,
                AddedAt = f.Created.At
            }).ToList();
    }
}


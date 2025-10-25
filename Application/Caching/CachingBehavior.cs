using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Caching;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMemoryCache _memoryCache;
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

    public CachingBehavior(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery<TResponse> cacheableQuery)
        {
            return await next();
        }

        if (_memoryCache.TryGetValue(cacheableQuery.CacheKey, out TResponse cachedResponse))
        {
            return cachedResponse;
        }

        var myLock = Locks.GetOrAdd(cacheableQuery.CacheKey, _ => new SemaphoreSlim(1, 1));
        await myLock.WaitAsync(cancellationToken);

        try
        {
            if (_memoryCache.TryGetValue(cacheableQuery.CacheKey, out cachedResponse))
            {
                return cachedResponse;
            }

            var response = await next();

            if (cacheableQuery.AbsoluteExpirationRelativeToNow.HasValue)
            {
                _memoryCache.Set(cacheableQuery.CacheKey, response, cacheableQuery.AbsoluteExpirationRelativeToNow.Value);
            }
            else
            {
                _memoryCache.Set(cacheableQuery.CacheKey, response);
            }

            return response;
        }
        finally
        {
            myLock.Release();
        }
    }
}


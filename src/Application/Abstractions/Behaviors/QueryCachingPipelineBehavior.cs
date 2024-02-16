

using Application.Abstractions.Caching;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Abstractions.Behaviors;

internal sealed class QueryCachingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<QueryCachingPipelineBehavior<TRequest, TResponse>> _logger;

    public QueryCachingPipelineBehavior(
        ICacheService cacheService,
        ILogger<QueryCachingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse? cachedResult = await _cacheService.GetAsync<TResponse>(
            request.CacheKey,
            cancellationToken);

        string requestName = typeof(TRequest).Name;
        if (cachedResult is null)
        {
            _logger.LogInformation("Cache hit for {RequestName}", requestName);
        }

        _logger.LogInformation("Cache miss for {RequestName}", requestName);

        TResponse result = await next();

        if (result.IsSuccess)
        {
            await _cacheService.SetAsync(
                request.CacheKey,
                result,
                request.Expiration,
                cancellationToken);
        }

        return result;
    }
}

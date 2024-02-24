using Infrastructure.Cache;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.List;

public class ListVideoQueryHandler : IRequestHandler<ListVideoQuery, List<ListVideoDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheService _cache;

    public ListVideoQueryHandler(ApplicationDbContext dbContext, CacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<List<ListVideoDto>> Handle(ListVideoQuery request, CancellationToken cancellationToken)
    {
        var videos = await _cache.GetOrSetAsync(
            CacheKeys.Videos.List(),
            () => _dbContext.Videos
                .Select(video => ListVideoDto.Create(video))
                .ToListAsync(cancellationToken),
            TimeSpan.FromSeconds(30));

        return videos;
    }
}

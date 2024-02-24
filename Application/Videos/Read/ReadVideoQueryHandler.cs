using Domain.Videos;
using Infrastructure.Cache;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.Read;

public class ReadVideoQueryHandler : IRequestHandler<ReadVideoQuery, ReadVideoDto>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheService _cache;

    public ReadVideoQueryHandler(ApplicationDbContext dbContext, CacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<ReadVideoDto> Handle(ReadVideoQuery request, CancellationToken cancellationToken)
    {
        var video = await _cache.GetOrSetAsync(
            CacheKeys.Videos.Read(request.Id),
            () => _dbContext.Videos
                .Select(video => ReadVideoDto.Create(video))
                .FirstOrDefaultAsync(video => video.Id == request.Id, cancellationToken),
            TimeSpan.FromSeconds(30));

        if (video is null)
        {
            throw new VideoNotFoundException(request.Id);
        }

        return video;
    }
}

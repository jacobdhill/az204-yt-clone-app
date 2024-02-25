using Infrastructure.Cache;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Comments.List;

public class ListCommentQueryHandler : IRequestHandler<ListCommentQuery, List<ListCommentDto>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheService _cache;

    public ListCommentQueryHandler(ApplicationDbContext dbContext, CacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<List<ListCommentDto>> Handle(ListCommentQuery request, CancellationToken cancellationToken)
    {
        var comments = await _cache.GetOrSetAsync(
            CacheKeys.Comments.List(request.VideoId),
            () => _dbContext.Comments
                .Where(comment => comment.VideoId == request.VideoId)
                .OrderByDescending(comment => comment.CreatedDateUtc)
                .Select(comment => ListCommentDto.Create(comment))
                .ToListAsync(cancellationToken),
            TimeSpan.FromSeconds(30));

        return comments;
    }
}

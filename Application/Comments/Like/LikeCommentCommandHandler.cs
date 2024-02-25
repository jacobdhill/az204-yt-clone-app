using Domain.Comments;
using Infrastructure.Cache;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Comments.Like;

public class LikeCommentCommandHandler : IRequestHandler<LikeCommentCommand, int>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheService _cache;

    public LikeCommentCommandHandler(ApplicationDbContext dbContext, CacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<int> Handle(LikeCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _dbContext.Comments
            .FirstOrDefaultAsync(comment =>
                comment.VideoId == request.VideoId &&
                comment.Id == request.CommentId,
                cancellationToken);

        if (comment is null)
        {
            throw new CommentNotFoundException(request.VideoId, request.CommentId);
        }

        comment.LikeCount += 1;

        await _dbContext.SaveChangesAsync(cancellationToken);

        _cache.Remove(CacheKeys.Comments.List(request.VideoId));

        return comment.LikeCount;
    }
}

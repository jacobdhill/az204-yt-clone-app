using Domain.Comments;
using Infrastructure.Cache;
using Infrastructure.Contexts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Comments.Create;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
{
    private ApplicationDbContext _dbContext;
    private CacheService _cache;

    public CreateCommentCommandHandler(ApplicationDbContext dbContext, CacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            VideoId = request.VideoId,
            Message = request.Message,
            LikeCount = 0,
            CreatedDateUtc = DateTime.UtcNow,
        };

        await _dbContext.Comments.AddAsync(comment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _cache.Remove(CacheKeys.Comments.List(request.VideoId));

        return comment.Id;
    }
}

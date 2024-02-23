using Domain.Videos;
using Infrastructure.Contexts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.Create;

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, Guid>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateVideoCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        var video = new Video()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags
        };

        await _dbContext.Videos.AddAsync(video, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return video.Id;
    }
}

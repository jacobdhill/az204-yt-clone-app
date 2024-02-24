using Domain.Videos;
using Infrastructure.Contexts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.Read;

public class ReadVideoQueryHandler : IRequestHandler<ReadVideoQuery, ReadVideoDto>
{
    private readonly ApplicationDbContext _dbContext;

    public ReadVideoQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReadVideoDto> Handle(ReadVideoQuery request, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos.FindAsync(request.Id);
        if (video is null)
        {
            throw new VideoNotFoundException(request.Id);
        }

        var videoDto = ReadVideoDto.Create(video);
        return videoDto;
    }
}

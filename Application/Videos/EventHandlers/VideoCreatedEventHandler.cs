using Domain.Videos;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.EventHandlers
{
    public class VideoCreatedEventHandler : IRequestHandler<VideoCreatedEvent>
    {
        private readonly ApplicationDbContext _dbContext;

        public VideoCreatedEventHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(VideoCreatedEvent request, CancellationToken cancellationToken)
        {
            var video = await _dbContext.Videos.FirstOrDefaultAsync(video => video.Id == request.Id, cancellationToken);
            if (video is null)
            {
                throw new VideoNotFoundException(request.Id);
            }
        }
    }
}

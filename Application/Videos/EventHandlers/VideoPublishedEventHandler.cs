using Domain.Videos;
using Infrastructure.Contexts;
using Infrastructure.Email;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.EventHandlers
{
    public class VideoPublishedEventHandler : IRequestHandler<VideoPublishedEvent>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _emailService;

        public VideoPublishedEventHandler(ApplicationDbContext dbContext, IMemoryCache cache, IEmailService emailService)
        {
            _dbContext = dbContext;
            _cache = cache;
            _emailService = emailService;
        }

        public async Task Handle(VideoPublishedEvent request, CancellationToken cancellationToken)
        {
            var video = await _dbContext.Videos.FirstOrDefaultAsync(video => video.Id == request.Id, cancellationToken);
            if (video is null)
            {
                throw new VideoNotFoundException(request.Id);
            }

            _cache.Remove(CacheKeys.Videos.Read(video.Id));

            var emailTo = "";
            var subject = "Your Video has been Successfully Published";
            var content = "<h1>Your Video has been Successfully Published</h1>";

            content = content
                .Replace("[[User Name]]", "John Doe")
                .Replace("[[Video Link]]", $"http://127.0.0.1:4200/videos/{video.Id}");

            await _emailService.SendEmailAsync(emailTo, subject, content, cancellationToken);
        }
    }
}

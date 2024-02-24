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

            var emailTo = "jacob.d.hill@outlook.com";
            var subject = "Your Video has been Successfully Published";
            var content = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"><title>Video Published Notification</title></head><body style=\"font-family:Arial,sans-serif\"><table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td style=\"padding:20px;text-align:center;background-color:#f4f4f4\"><h2>Your Video has been Successfully Published!</h2></td></tr><tr><td style=\"padding:20px\"><p>Hello [[User Name]],</p><p>We are excited to inform you that your video has been successfully published and is now ready to view and share.</p><p>You can access your video by clicking on the link below:</p><p><a href=\"[[Video Link]]\" style=\"color:#007bff\">View Your Video</a></p><p>Thank you for using our service!</p><p>Best regards,<br>Hill Tech Solutions, LLC</p></td></tr></table></body></html>";

            content = content
                .Replace("[[User Name]]", "Jacob Hill")
                .Replace("[[Video Link]]", $"http://127.0.0.1:4200/videos/{video.Id}");

            await _emailService.SendEmailAsync(emailTo, subject, content, cancellationToken);
        }
    }
}

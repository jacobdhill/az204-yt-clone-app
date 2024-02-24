using Domain.Videos;
using Infrastructure.Cache;
using Infrastructure.Contexts;
using Infrastructure.Storage;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.Create;

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, Guid>
{
    private readonly StorageService _storageService;
    private readonly ApplicationDbContext _dbContext;
    private readonly CacheService _cache;

    public CreateVideoCommandHandler(
        ApplicationDbContext dbContext,
        StorageService storageService,
        CacheService cache)
    {
        _dbContext = dbContext;
        _storageService = storageService;
        _cache = cache;
    }

    public async Task<Guid> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        if (File.Exists(request.FileName) == false)
        {
            throw new VideoFileNotFoundException(request.FileName);
        }

        var video = new Video()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags,
            CreatedDateUtc = DateTime.UtcNow
        };

        var fileName = video.Id + ".mp4";
        using (var stream = new FileStream(request.FileName, FileMode.Open))
        {
            await _storageService.UploadFileToStorageAsync(stream, fileName, cancellationToken);
        }

        File.Delete(request.FileName);

        var videoCreatedEvent = new VideoCreatedEvent()
        {
            Id = video.Id
        };
        video.DomainEvents.Add(videoCreatedEvent);

        await _dbContext.Videos.AddAsync(video, cancellationToken);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _storageService.RemoveFileFromStorageAsync(fileName, cancellationToken);
            throw;
        }

        _cache.Remove(CacheKeys.Videos.List());

        return video.Id;
    }
}

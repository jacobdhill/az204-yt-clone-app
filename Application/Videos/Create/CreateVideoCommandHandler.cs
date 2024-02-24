using Domain.Videos;
using Infrastructure.Contexts;
using Infrastructure.Storage;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.Create;

public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, Guid>
{
    private readonly StorageService _storageService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public CreateVideoCommandHandler(
        ApplicationDbContext dbContext,
        StorageService storageService,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _storageService = storageService;
        _configuration = configuration;
    }

    public async Task<Guid> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
        {
            throw new VideoFileInvalidException();
        }

        var video = new Video()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Tags = request.Tags
        };

        var fileInfo = new FileInfo(request.File.FileName);
        var fileName = video.Id + fileInfo.Extension;
        using (var stream = request.File.OpenReadStream())
        {
            await _storageService.UploadFileToStorageAsync(stream, fileName, cancellationToken);
        }

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

        return video.Id;
    }
}

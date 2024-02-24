using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Domain.Videos;
using Infrastructure.Contexts;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Publisher.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher;

public class Publisher
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public Publisher(IConfiguration configuration, ApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [FunctionName(nameof(Publisher))]
    public async Task Run([ServiceBusTrigger("videos-to-publish", Connection = "ServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        try
        {
            var thumbnailCreatedEvent = JsonConvert.DeserializeObject<BlobCreatedEvent>(message.Body.ToString());
            var thumbnailUri = new Uri(thumbnailCreatedEvent.Data.Url);
            var fileInfo = new FileInfo(thumbnailUri.AbsoluteUri);

            var videoIdStr = fileInfo.Name.Replace(fileInfo.Extension, string.Empty);
            var videoId = Guid.Parse(videoIdStr);
            var video = await _context.Videos.FindAsync(videoId);
            if (video is null)
            {
                throw new VideoNotFoundException(videoId);
            }

            var videosContainerClient = new BlobContainerClient(
                _configuration.GetConnectionString("BlobStorage"),
                _configuration.GetSection("BlobStorage:VideosContainerName").Get<string>());
            var videoFileName = videoId + ".mp4";
            var videoBlobClient = videosContainerClient.GetBlobClient(videoFileName);

            var thumbnailsContainerClient = new BlobContainerClient(
                _configuration.GetConnectionString("BlobStorage"),
                _configuration.GetSection("BlobStorage:ThumbnailsContainerName").Get<string>());
            var thumbnailFileName = videoId + ".jpg";
            var thumbnailBlobClient = thumbnailsContainerClient.GetBlobClient(thumbnailFileName);

            var videoExists = await videoBlobClient.ExistsAsync(cancellationToken);
            var thumbnailExists = await thumbnailBlobClient.ExistsAsync(cancellationToken);
            if (videoExists == false || thumbnailExists == false)
            {
                throw new VideoFailedToProcessException(videoId);
            }

            var videoPublishedEvent = new VideoPublishedEvent()
            {
                Id = videoId
            };
            video.DomainEvents.Add(videoPublishedEvent);

            video.SourceUrl = videoBlobClient.Uri.AbsoluteUri;
            video.ThumbnailUrl = thumbnailBlobClient.Uri.AbsoluteUri;

            await _context.SaveChangesAsync(cancellationToken);

            await messageActions.CompleteMessageAsync(message, cancellationToken);
        }
        catch (Exception)
        {
            await messageActions.DeadLetterMessageAsync(message, cancellationToken: cancellationToken);
        }
    }
}

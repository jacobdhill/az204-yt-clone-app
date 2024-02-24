using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NReco.VideoConverter;
using Processor.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Processor;

public class Processor
{
    private readonly IConfiguration _configuration;

    public Processor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [FunctionName(nameof(Processor))]
    public async Task Run([ServiceBusTrigger("videos-to-process", Connection = "ServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        try
        {
            var videoCreatedEvent = JsonConvert.DeserializeObject<BlobCreatedEvent>(message.Body.ToString());
            var videoUri = new Uri(videoCreatedEvent.Data.Url);

            var videosContainerClient = new BlobContainerClient(
                _configuration.GetConnectionString("BlobStorage"),
                _configuration.GetSection("BlobStorage:VideosContainerName").Get<string>());

            var fileInfo = new FileInfo(videoUri.AbsoluteUri);
            var videoBlobClient = videosContainerClient.GetBlobClient(fileInfo.Name);
            var tempVideoFilePath = Path.Join(Path.GetTempPath(), fileInfo.Name);
            await videoBlobClient.DownloadToAsync(tempVideoFilePath, cancellationToken);

            var thumbnailFileName = fileInfo.Name.Replace(fileInfo.Extension, string.Empty) + ".jpg";
            var tempThumbnailFilePath = Path.Join(Path.GetTempPath(), thumbnailFileName);
            var ffMpeg = new FFMpegConverter();
            ffMpeg.GetVideoThumbnail(tempVideoFilePath, tempThumbnailFilePath);

            var thumbnailsContainerClient = new BlobContainerClient(
                _configuration.GetConnectionString("BlobStorage"),
                _configuration.GetSection("BlobStorage:ThumbnailsContainerName").Get<string>());
            var thumbnailBlobClient = thumbnailsContainerClient.GetBlobClient(thumbnailFileName);
            using (var stream = new FileStream(tempThumbnailFilePath, FileMode.Open))
            {
                await thumbnailBlobClient.UploadAsync(stream, cancellationToken);
            }

            File.Delete(tempVideoFilePath);
            File.Delete(tempThumbnailFilePath);

            await messageActions.CompleteMessageAsync(message, cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            await messageActions.DeadLetterMessageAsync(message, cancellationToken: cancellationToken);
        }
    }
}

using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Storage;

public class StorageService
{
    private readonly BlobContainerClient _client;

    public StorageService(IConfiguration configuration)
    {
        _client = new BlobContainerClient(
            configuration.GetConnectionString("BlobStorage"),
            configuration.GetSection("BlobStorage:VideosContainerName").Get<string>());

        _client.CreateIfNotExists();
    }

    public async Task UploadFileToStorageAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = _client.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, cancellationToken);
    }

    public async Task RemoveFileFromStorageAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = _client.GetBlobClient(fileName);
        await blobClient.DeleteAsync(cancellationToken: cancellationToken);
    }
}

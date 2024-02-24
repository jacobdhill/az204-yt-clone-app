using Domain.Videos;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Videos.Upload;

public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, string>
{
    public async Task<string> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
        {
            throw new VideoFileInvalidException();
        }

        var filePath = Path.GetTempPath() + Guid.NewGuid() + ".mp4";
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        return filePath;
    }
}

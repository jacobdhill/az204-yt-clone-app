using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Videos.Upload;

public class UploadVideoCommand : IRequest<string>
{
    public IFormFile File { get; set; }
}

using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Application.Videos.Create;

public class CreateVideoCommand : IRequest<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public IFormFile File { get; set; }
}

using MediatR;
using System;

namespace Application.Comments.Create;

public class CreateCommentCommand : IRequest<Guid>
{
    public Guid VideoId { get; set; }
    public string Message { get; set; }
}

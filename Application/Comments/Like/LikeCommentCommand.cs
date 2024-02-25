using MediatR;
using System;

namespace Application.Comments.Like;

public class LikeCommentCommand : IRequest<int>
{
    public Guid VideoId { get; set; }
    public Guid CommentId { get; set; }
}

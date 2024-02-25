using Domain.Comments;
using System;

namespace Application.Comments.List;

public class ListCommentDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedDateUtc { get; set; }

    public static ListCommentDto Create(Comment comment)
    {
        return new ListCommentDto()
        {
            Id = comment.Id,
            Message = comment.Message,
            LikeCount = comment.LikeCount,
            CreatedDateUtc = comment.CreatedDateUtc
        };
    }
}

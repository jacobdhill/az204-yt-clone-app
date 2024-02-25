using System;

namespace Domain.Comments;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException(Guid videoId, Guid commentId)
        : base($"No comment found for Video Id = '{videoId}' and Comment Id = '{commentId}'")
    {
    }
}

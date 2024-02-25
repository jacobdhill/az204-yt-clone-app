using System;
using System.Text.Json.Serialization;

namespace Domain.Comments;

public class Comment
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    public Guid VideoId { get; set; }
    public string Message { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedDateUtc { get; set; }
}

using Domain.Videos;
using System;
using System.Collections.Generic;

namespace Application.Videos.List;

public class ListVideoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public string ThumbnailUrl { get; set; }

    public static ListVideoDto Create(Video video)
    {
        return new ListVideoDto()
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            Tags = video.Tags,
            ThumbnailUrl = video.ThumbnailUrl
        };
    }
}

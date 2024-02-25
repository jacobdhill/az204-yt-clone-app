using Domain.Videos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Videos.Read;

public class ReadVideoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public string SourceUrl { get; set; }
    public DateTime CreatedDateUtc { get; set; }

    public static ReadVideoDto Create(Video video)
    {
        return new ReadVideoDto()
        {
            Id = video.Id,
            Title = video.Title,
            Description = video.Description,
            SourceUrl = video.SourceUrl,
            CreatedDateUtc = video.CreatedDateUtc,
            Tags = video.Tags
                .OrderBy(tag => tag)
                .ToList()
        };
    }
}

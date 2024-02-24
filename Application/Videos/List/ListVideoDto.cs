using Domain.Videos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Videos.List;

public class ListVideoDto
{
    public const int DescriptionMaxSize = 42;

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public string ThumbnailUrl { get; set; }
    public DateTime CreatedDateUtc { get; set; }

    public static ListVideoDto Create(Video video)
    {
        var description = video.Description;
        if (description.Length > DescriptionMaxSize)
        {
            description = string
                .Join(string.Empty, description.Take(DescriptionMaxSize))
                .TrimEnd();
            description += "...";
        }

        return new ListVideoDto()
        {
            Id = video.Id,
            Title = video.Title,
            Description = description,
            Tags = video.Tags,
            ThumbnailUrl = video.ThumbnailUrl,
            CreatedDateUtc = video.CreatedDateUtc
        };
    }
}

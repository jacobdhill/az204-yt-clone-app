using System;

namespace Domain.Videos;

public class VideoNotFoundException : Exception
{
    public VideoNotFoundException(Guid videoId)
        : base($"Video not found with Id = '{videoId}'")
    {
    }
}

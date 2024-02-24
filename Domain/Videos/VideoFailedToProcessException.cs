using System;

namespace Domain.Videos;

public class VideoFailedToProcessException : Exception
{
    public VideoFailedToProcessException(Guid videoId)
        : base($"Video with Id = '{videoId}' failed to process")
    {
    }
}

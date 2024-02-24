using System;

namespace Domain.Videos;

public class VideoFileInvalidException : Exception
{
    public VideoFileInvalidException()
        : base("Video file is invalid")
    {
    }
}

using System;

namespace Domain.Videos;

public class VideoFileNotFoundException : Exception
{
    public VideoFileNotFoundException(string fileName)
        : base($"Video file '{fileName}' does not exist")
    {
    }
}

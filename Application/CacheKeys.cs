using System;

namespace Application;

public static class CacheKeys
{
    public static class Videos
    {
        public static string List() => "videos";
        public static string Read(Guid id) => "videos-" + id;
    }

    public static class Comments
    {
        public static string List(Guid videoId) => "comments-" + videoId;
    }
}

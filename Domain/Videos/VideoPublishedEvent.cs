using System;

namespace Domain.Videos;

public class VideoPublishedEvent : DomainEvent
{
    public Guid Id { get; set; }
}

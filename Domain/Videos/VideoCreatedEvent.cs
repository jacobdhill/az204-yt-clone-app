using System;

namespace Domain.Videos;

public class VideoCreatedEvent : DomainEvent
{
    public Guid Id { get; set; }
}

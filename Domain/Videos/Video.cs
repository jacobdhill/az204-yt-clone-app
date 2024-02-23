using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Videos;

public class Video : IDomainEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public string SourceUrl { get; set; }
    public string ThumbnailUrl { get; set; }

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}

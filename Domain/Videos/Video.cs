using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Videos;

public class Video : IDomainEntity
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public string SourceUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public bool IsDeleted { get; set; }

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}

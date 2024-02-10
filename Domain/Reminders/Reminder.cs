using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Reminders;

public class Reminder : IDomainEntity
{
    public ReminderId ReminderId { get; set; }
    public string Description { get; set; }
    public int NotifyDaysBefore { get; set; }
    public int DayOfMonth { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    [NotMapped]
    public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
}

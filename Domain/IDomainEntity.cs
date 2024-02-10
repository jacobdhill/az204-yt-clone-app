using System.Collections.Generic;

namespace Domain;

public interface IDomainEntity
{
    public List<DomainEvent> DomainEvents { get; set; }
}

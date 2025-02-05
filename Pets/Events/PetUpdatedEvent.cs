using Pets.Interfaces.Events;
using System;

namespace Pets.Events
{
    public class PetUpdatedEvent : IEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public DateTime OccurredOn { get; set; }

        public PetUpdatedEvent(Guid petId, string name, DateTime occurredOn)
        {
            PetId = petId;
            Name = name;
            OccurredOn = occurredOn;
        }
    }
}

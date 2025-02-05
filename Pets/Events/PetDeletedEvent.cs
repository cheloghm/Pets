using Pets.Interfaces.Events;
using System;

namespace Pets.Events
{
    public class PetDeletedEvent : IEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public Guid PetId { get; set; }
        public string Name { get; set; }
        public DateTime OccurredOn { get; set; }

        public PetDeletedEvent(Guid petId, string name, DateTime occurredOn)
        {
            PetId = petId;
            Name = name;
            OccurredOn = occurredOn;
        }
    }
}

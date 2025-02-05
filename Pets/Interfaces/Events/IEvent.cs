namespace Pets.Interfaces.Events
{
    public interface IEvent
    {
        /// <summary>
        /// A unique identifier for this event.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The date and time when the event occurred.
        /// </summary>
        DateTime OccurredOn { get; }
    }
}

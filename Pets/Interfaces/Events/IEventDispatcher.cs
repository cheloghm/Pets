namespace Pets.Interfaces.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync(IEvent petEvent);
    }
}

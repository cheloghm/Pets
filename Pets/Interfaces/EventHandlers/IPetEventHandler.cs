using System.Threading.Tasks;

namespace Pets.Interfaces.Events
{
    public interface IPetEventHandler
    {
        /// <summary>
        /// Determines whether this handler can process the given event.
        /// </summary>
        bool CanHandle(IEvent petEvent);

        /// <summary>
        /// Handles the event.
        /// </summary>
        Task HandleAsync(IEvent petEvent);
    }
}
